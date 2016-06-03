using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using zapad.Public.WebInterface.Models.ServiceInteraction;

namespace zapad.Public.WebInterface.Models.Tools
{
    public class WebHostCache
    {
        /// <summary>
        /// Сертификат, используемый для отправки к WebHostCache для авторизации портала.
        /// </summary>
        public X509Certificate CertToZone { get; private set; }
        /// <summary>
        /// Список доступных каналов до WebHostCacheChannel
        /// </summary>
        public List<WebHostCacheChannel> ZoneChannelsList { get; private set; }
        /// <summary>
        /// Интервал между отправками уведомлений об отсутствии соединения (ответа ping) с WebHostCache (в мс)
        /// </summary>
        public double msNoPingNotifyInterval { get; private set; }
        #region Инициализация
        /// <summary>
        /// Приватный конструктор инициализации
        /// </summary>
        /// <param name="aZoneChannelsXml">Список каналов, используемых для соединения с WebHostCache в XML формате</param>
        /// <param name="CertificateToZoneThumbprint">Отпечаток сертификата из хранилища личных сертификатов данного пользователя, отправляемый в WebHostCache</param>
        /// <param name="msPingZoneInterval">Интервал между отправкой запросов ping в WebHostCache (в мс)</param>
        /// <param name="aPingRequestsCount">Количество запросов ping, отправляемых в WebHostCache для вычисления среднего значения</param>
        private WebHostCache(XmlDocument aWebHostCacheChannelsXml, string CertificateToZoneThumbprint, long msPingZoneInterval, int aPingRequestsCount, double aNoPingNotifyInterval)
        {
            this.msNoPingNotifyInterval = aNoPingNotifyInterval;
            this.ZoneChannelsList = new List<WebHostCacheChannel>();
            XElement endPoints = XElement.Load(new XmlNodeReader(aWebHostCacheChannelsXml));
            // Заполняем список каналов к WebHostCache с ненулевыми приоритетами
            foreach (XElement endPoint in endPoints.Elements("channel").Where(x => x.Attribute("priority").Value != "0"))
            {
                this.ZoneChannelsList.Add(new WebHostCacheChannel(endPoint.Value, endPoint.Attribute("priority").getValue(0),
                   endPoint.Attribute("bandwith").getValue(0f), this.CertToZone, msPingZoneInterval, aPingRequestsCount));
                if (endPoint.Attribute("fakeping") != null)
                    this.ZoneChannelsList.Last().msFakeDebugPing = endPoint.Attribute("fakeping").getValue(0);
            }
        }

        /// <summary>
        /// Экземпляр доступа к WebHostCache
        /// </summary>
        public static WebHostCache Current { get; private set; }
        /// <summary>
        /// Инициализация класса.
        /// </summary>
        /// <param name="WebApiZoneBaseAddress">Список каналов, используемых для соединения с WebHostCache в XML формате</param>
        /// <param name="CertificateToZoneThumbprint">Отпечаток сертификата из хранилища личных сертификатов данного пользователя, отправляемый в WebHostCache</param>
        /// <param name="msPingZoneInterval">Интервал между отправкой запросов ping в WebHostCache (в мс)</param>
        /// <param name="PingRequestsCount">Количество запросов ping, отправляемых в WebHostCache для вычисления среднего значения</param>
        public static void Create(XmlDocument WebApiZoneBaseAddress, string CertificateToZoneThumbprint, long msPingZoneInterval, int PingRequestsCount, double msNoPingNotifyInterval)
        { Current = new WebHostCache(WebApiZoneBaseAddress, CertificateToZoneThumbprint, msPingZoneInterval, PingRequestsCount, msNoPingNotifyInterval); }
        #endregion

        public enum ContentTypes
        {
            xml = 1,
            json = 2,
            binary = 3,
        }
                
        /// <summary>
        /// "Helper" - получение типизированного результата.
        /// </summary>
        /// <typeparam name="TResult">Тип</typeparam>
        /// <param name="tpe">Запрашиваемый заголовок запроса</param>
        /// <param name="uri">uri запроса</param>
        /// <returns>Возвращаемое значение</returns>
        public TResult GetResponse<TResult>(string uri, XElement PostBody = null, ContentTypes tpe = ContentTypes.xml)
        {

       //     WebHostCacheChannel channel = this.ZoneChannelsList[0];//GetLessLoadedEndPoint();
            //    WebHostCacheChannel channel = GetLessLoadedEndPoint();
            // Проверяем наличие в запросе ключа сессии
            string sessionKey = ExtractSessionKey(uri, PostBody);
            if (String.IsNullOrEmpty(sessionKey))
                throw new ArgumentException("Направляемый запрос не содержит ключ сессии");

            // Обеспечиваем привязку клиенту SignalR
            ResponseHubClient client = ResponseHubClientRepository.GetClient(sessionKey);
            var waitHandler = new System.Threading.AutoResetEvent(false);
            var requestId = RequestID; // Получаем очередной ID для запроса в локальную копию
            client.WaitHandlers.Add(requestId, waitHandler);
            AttachIdToRequest(ref uri, PostBody, requestId);

            // Формируем запрос
            WebHostCacheChannel channel = GetLessLoadedEndPoint();
            List<WebHostCacheChannel> UsedChannels = new List<WebHostCacheChannel>() { channel };
            WebHostCacheRequest request = new WebHostCacheRequest(channel, uri, UsedChannels, PostBody, tpe);

            try
            {   
                // Отправляем запрос
                HttpResponseMessage response = GetResult(channel, request, tpe);

                // Получаем результаты 
                waitHandler.WaitOne();
                var retValue = client.RequestResults[requestId];

                return (TResult)retValue;
            }
            finally
            {
                // Завершаем обработку запроса
                client.WaitHandlers.Remove(requestId);
                client.RequestResults.Remove(requestId);
            }            
        }

        // Хендлер для критических секций
        private static object _lockHandler = new object();

        // Счетчик идентификаторов запроса
        private static long _requestId = 1;

        /// <summary>
        /// Возвращает ID запроса потокобезопасным образом
        /// </summary>
        private static long RequestID {
            get {
                lock (_lockHandler) {
                    return _requestId++;
                }
            }
        }

        /// <summary>
        /// Дополнить запрос уникальным идентификатором для привязки ответа от SignalR
        /// </summary>
        /// <param name="uri">URI запроса</param>
        /// <param name="postBody">Тело POST-запроса</param>
        /// <param name="postBody">Добавляемый ID</param>
        private void AttachIdToRequest(ref string uri, XElement postBody, long requestId)
        {
            if (postBody != null)
                postBody.Add(new XElement("requestId", requestId));
            else
                uri += "&requestId=" + requestId;            
        }

        /// <summary>
        /// Извлекает ключ сессии из запроса
        /// </summary>
        /// <param name="uri">URI запроса</param>
        /// <param name="postBody">Тело запроса в формате XML</param>
        /// <returns>Ключ сессии</returns>
        private string ExtractSessionKey(string uri, XElement postBody)
        {
            string key = null;

            if (postBody != null)
            {
                if (postBody.Element("sessionKey") != null)
                    key = postBody.Element("sessionKey").Value;
            }
            else
            {
                var queryParams = ExtractQueryParams(uri);
                if (queryParams.Keys.Contains("sessionKey"))
                    key = queryParams["sessionKey"];
            }

            return key;
        }

        /// <summary>
        /// Распарсить относительный URI на пары "имя переменной - значение"
        /// </summary>
        /// <param name="uri">Исходный URI</param>
        /// <returns>Словарь пар "имя переменной - значение"</returns>
        private Dictionary<string, string> ExtractQueryParams(string uri)
        {
            var retval = new Dictionary<string, string>();
            var pairs = uri.Split('?', '&');
            foreach (var pair in pairs)
            {
                var tokens = pair.Split('=');
                if (tokens.Length == 2)
                    retval.Add(tokens[0], tokens[1]);
            }
            return retval;
        }

        public byte[] GetResponseBinary(string uri, XElement PostBody = null)
        {
            WebHostCacheChannel channel = GetLessLoadedEndPoint();
            List<WebHostCacheChannel> UsedChannels = new List<WebHostCacheChannel>() { channel };
            WebHostCacheRequest request = new WebHostCacheRequest(channel, uri, UsedChannels, PostBody, ContentTypes.binary);
            HttpResponseMessage response = GetResult(channel, request, ContentTypes.binary);
            return response.Content.ReadAsByteArrayAsync().Result;
        }
        /// <summary>
        /// Отправка запроса на канал. Если канал "упал", выбирается следующий доступный
        /// </summary>
        /// <param name="currentChannel">Канал, по которому надо отправить запрос</param>
        /// <param name="request">Экземпляр заполненного запроса</param>
        /// <param name="tpe">Запрашиваемый заголовок запроса (нужен, если канал упал)</param>
        /// <returns>Экземпляр HttpResponseMessage</returns>
        private HttpResponseMessage GetResult(WebHostCacheChannel currentChannel, WebHostCacheRequest request, ContentTypes tpe = ContentTypes.xml)
        {
            try
            {
                // Добавляем в список запрос и обновляем значение коэффициента нагрузки
                currentChannel.RequestsList.Add(request);
                Task<HttpResponseMessage> response = currentChannel.Client.SendAsync(request);
                HttpResponseMessage result = response.Result;
                result.EnsureSuccessStatusCode();
                currentChannel.ReqSended++;
                return result;
            }
            catch (Exception ee)
            {
                // Канал не доступен, выбираем другой менее нагруженный, но не этот
                WebHostCacheChannel nextCannel = GetLessLoadedEndPoint(request.UsedChannels);
                request.UsedChannels.Add(nextCannel);
                WebHostCacheRequest newRequest = new WebHostCacheRequest(nextCannel, request.RequestUri.AbsolutePath,
                    request.UsedChannels, request.Content == null ? null : request.Content.ReadAsAsync<XElement>().Result, tpe);
                return GetResult(nextCannel, newRequest);
            }
            finally
            {
                // Пришёл ответ или была ошибка, удаляем запрос из списка и обновляем значение коэффициента нагрузки
                currentChannel.RequestsList.Remove(request);
            }
        }
        /// <summary>
        /// Поиск сертификата в хранилище личных сертификатов данного пользователя по отпечатку
        /// </summary>
        /// <param name="Thumbprint">Отпечаток сертификата (без пробелов, проверить на невидимый символ в начале)</param>
        /// <returns>Экземпляр X509Certificate, представляющий сертификат. Если не найден, то исключение ArgumentOutOfRangeException</returns>
        private X509Certificate GetStoreCertificateByThumbprint(string Thumbprint)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection collection = store.Certificates.Find(X509FindType.FindByThumbprint, Thumbprint, false);
            store.Close();
            if (collection.Count > 0)
            {
                // Получаем через энумератор, т.к. если здесь будет collection[0], то выкинется исключение OutOfRange(index)
                // даже если не будет входа в эту часть кода(т.е. сертификата нет). Отложенное действие?
                X509Certificate2Enumerator enumerator = collection.GetEnumerator();
                enumerator.MoveNext();
                return enumerator.Current;
            }
            else throw new ArgumentOutOfRangeException(Thumbprint, "Не найден сертификат с данным отпечатком для связи с WebHostCache");
        }
        /// <summary>
        /// Выбирает самый менее нагруженный доступный канал (с минимальным коэффициентом нагрузки) 
        /// в случае возникновения ошибки по каналу, куда отправлено сообщение
        /// </summary>
        /// <param name="exceptChannels">Список каналов, которые не нужно учитывать (ранее были использованы для запроса)</param>
        /// <returns>Экземпляр ZoneChannel</returns>
        private WebHostCacheChannel GetLessLoadedEndPoint(List<WebHostCacheChannel> exceptChannels)
        {
            foreach (WebHostCacheChannel channel in ZoneChannelsList)
                channel.RefreshLoadCoef();
            if (exceptChannels == null) return GetLessLoadedEndPoint();
            if (exceptChannels.Count == this.ZoneChannelsList.Count)
                throw new ArgumentException("Не удалось отправить запрос по каждому из доступных каналов");
            return this.ZoneChannelsList.Except(exceptChannels).Where(x => x.HasConnection).Aggregate((i1, i2) => i1.LoadCoef < i2.LoadCoef ? i1 : i2);
        }
        private WebHostCacheChannel GetLessLoadedEndPoint()
        {
            foreach (WebHostCacheChannel channel in ZoneChannelsList)
                channel.RefreshLoadCoef();
            return this.ZoneChannelsList.Where(x => x.HasConnection).Aggregate((i1, i2) => i1.LoadCoef < i2.LoadCoef ? i1 : i2);
        }

        /// <summary>
        /// Класс запросов HttpRequestMessage, имеющий список использованных каналов для отправки запроса
        /// </summary>
        public class WebHostCacheRequest : HttpRequestMessage
        {
            /// <summary>
            /// Список использованных каналов для отправки данного запроса
            /// </summary>
            public List<WebHostCacheChannel> UsedChannels;
            /// <summary>
            /// Генерация экземпляра запроса на основе параметров
            /// </summary>
            /// <param name="channel">Канал, используемый для отправки запроса</param>
            /// <param name="uri">Целевой путь для отправки запроса</param>
            /// <param name="UsedChannels">Список использованных каналов для отправки данного запроса</param>
            /// <param name="PostBody">Тело Post-запроса (если есть)</param>
            /// <param name="tpe">Запрашиваемый заголовок запроса</param>
            public WebHostCacheRequest(WebHostCacheChannel channel, string uri, List<WebHostCacheChannel> UsedChannels, XElement PostBody = null, ContentTypes tpe = ContentTypes.xml) : base()
            {
                if (uri[0] != '/')
                    uri = '/' + uri;
                this.RequestUri = new Uri(channel.Uri.OriginalString + uri);
                this.UsedChannels = UsedChannels;
                switch (tpe)
                {
                    case ContentTypes.xml:
                        this.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                        break;
                    case ContentTypes.json:
                        this.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        break;
                    default: break;
                }
                if (PostBody == null) this.Method = HttpMethod.Get;
                else
                {
                    this.Method = HttpMethod.Post;
                    this.Content = new ObjectContent<XElement>(PostBody, new XmlMediaTypeFormatter());
                }
            }
        }


        /// <summary>
        /// Класс, описывающий канал для соединения с WebHostCache
        /// </summary>
        public class WebHostCacheChannel : IDisposable
        {
            /// <summary>
            /// Сертификат, используемый для аутентификации портала в WebHostCache.
            /// </summary>
            public X509Certificate CertToZone { get; private set; }
            /// <summary>
            /// Список запросов на канале, ожидающий ответа
            /// </summary>
            public SynchronizedCollection<WebHostCacheRequest> RequestsList { get; private set; }
            /// <summary>
            /// URI-адрес конечной точки
            /// </summary>
            public Uri Uri { get; private set; }
            /// <summary>
            /// Индивидуальный клиент для отправки запросов по определённому каналу
            /// </summary>
            public HttpClient Client { get; private set; }
            /// <summary>
            /// Среднее время отклика от WebHostCache по каналу
            /// </summary>
            public long msZonePing { get; private set; }
            /// <summary>
            /// Количество отправляемых запросов Ping в WebHostCache для вычисления среднего значения
            /// </summary>
            public int PingRequestsCount { get; private set; }
            /// <summary>
            /// Приоритет канала
            /// </summary>
            public int Priority { get; private set; }
            public int ReqSended { get; set; }
            /// <summary>
            /// Пропускная способность канала (в мегабит\сек)
            /// </summary>
            public float Bandwith { get; private set; }
            /// <summary>
            /// Коэффициент загруженности
            /// </summary>
            public double LoadCoef { get; private set; }
            /// <summary>
            /// Значение отсутсвтия соединения (false-если соединение по каналу есть)
            /// </summary>
            public bool HasConnection { get; private set; }
            public long msFakeDebugPing { get; set; }
            /// <summary>
            /// Конструктор с инициализацией URI-адреса, приоритета, пропускной способности и сертификатом авторизации
            /// </summary>
            /// <param name="aUri">URI-адрес конечной точки</param>
            /// <param name="aPriority">Приоритет конечной точки (чем выше, тем больше шанс выбора)</param>
            /// <param name="aBandwith">Пропускная способность (в мегабит\сек)</param>
            /// <param name="aCert">Сертификат, отправляемый для авторизации у WebHostCache по SSL. Если null, то сертификат к запросам не будет добавлен</param>
            /// <param name="aPingZoneInterval">Интервал таймера (в мс.), по истечению которого определяется пинг от WebHostCache</param>
            /// <param name="aPingRequestsCount">Количество запросов ping, отправляемых в тех WebHostCache для вычисления среднего значения</param>
            public WebHostCacheChannel(string aUri, int aPriority, float aBandwith, X509Certificate aCert, long aPingZoneInterval, int aPingRequestsCount)
            {
                this.InitializeClient(aUri, aCert);
                this.RequestsList = new SynchronizedCollection<WebHostCacheRequest>();
                this.Priority = aPriority;
                this.Bandwith = aBandwith;
                if (aPingRequestsCount > 0)
                    this.PingRequestsCount = aPingRequestsCount;
                else this.PingRequestsCount = 4;
                this.HasConnection = true;
                this.pingTimer = new Timer(aPingZoneInterval);
                this.pingTimer.Elapsed += pingTimer_Elapsed;
                this.pingTimer.Start();
                this.RefreshLoadCoef();
                this.ReqSended = 0;
            }

            private void InitializeClient(string aUri, X509Certificate aCert)
            {
                this.Uri = new Uri(aUri);
                WebRequestHandler handler = new WebRequestHandler();
                // Связываемся с WebHostCache, даже если у него плохой сертификат (самоподписанный)
                handler.ServerCertificateValidationCallback = delegate { return true; };
                if (aCert != null)
                {
                    // Добавляем сертификат в запрос, если не запущена отладка
                    this.CertToZone = aCert;
                    handler.ClientCertificates.Add(aCert);
                }
                this.Client = new HttpClient(handler);
                this.Client.BaseAddress = new Uri(aUri);
            }

            #region Ping Work
            /// <summary>
            /// Таймер проверки времени отклика от WebHostCache (в мс.)
            /// </summary>
            private Timer pingTimer;
            private void pingTimer_Elapsed(object sender, ElapsedEventArgs e) { this.PingZone(); }

            private int NoResponseCounter = 0;

            private Timer NotifyTimer;

            /// <summary>
            /// Определенее среднего времени отклика (в мс.) от WebHostCache по каналу. Запись значения в ZonePing.
            /// </summary>
            private void PingZone()
            {
                try
                {
                    if (this.msFakeDebugPing != 0)
                    {
                        this.msZonePing = this.msFakeDebugPing;
                        return;
                    }
                    Ping ping = new Ping();
                    long totalPing = 0;
                    // Посылаем пинг несколько раз, чтобы выявить среднюю величину пинга
                    for (uint i = 0; i < this.PingRequestsCount; ++i)
                    {
                        PingReply reply = ping.Send(this.Uri.DnsSafeHost, 3000);
                        if (reply.Status == IPStatus.Success)
                            totalPing += reply.RoundtripTime;
                        else throw new PingException(reply.Status.ToString());  // Таймаут или др. ошибка
                    }
                    this.msZonePing = totalPing / this.PingRequestsCount;
                    this.HasConnection = true;
                    this.NoResponseCounter = 0;
                }
                catch (PingException e)
                {
                    // Ловим сами это исключение, если какой-то из каналов будет недоступен. Устанавливаем флаг отсутствия соединения
                    // чтобы алгоритм не выбирал этот канал. Т.к. пинг проводится в отдельном треде без контекста запроса, экзепшн не 
                    // будет отлавливаться в Application_Error => сами логируем
                    this.HasConnection = false;
                    this.NoResponseCounter++;
                    // Пингуем WebHostCache ещё раз, чтобы не ждать следующей попытки пинга через время
                    if (this.NoResponseCounter < this.PingRequestsCount)
                        PingZone();
                    else
                    {
                        Exception exception = new Exception("Нет PING-ответа от " + this.Uri.ToString(), e);
                        // Чтобы не отправлять уведомления об отсутствии пинга, используем второй таймер для предотвращения
                        if (this.NotifyTimer == null)
                        {
                            this.NotifyTimer = new Timer(WebHostCache.Current.msNoPingNotifyInterval);
                            this.NotifyTimer.Elapsed += NotifyTimer_Elapsed;
                            this.NotifyTimer.Start();
                            NoPingNotify(exception);
                        }
                    }
                }
            }
            private void NotifyTimer_Elapsed(object sender, ElapsedEventArgs e)
            {
                this.NotifyTimer = null;
            }
            /// <summary>
            /// Запись в БД и отправка СМС сообщения отсутствия соединения с WebHostCache.
            /// </summary>
            /// <param name="e">Экзепшн с сообщением</param>
            private void NoPingNotify(Exception e)
            {
                // Аудит мог быть не проинициализирован в главном модуле => проверяем перед использованием
                //if (AuditController.isInitialized)
                //    AuditController.RegisterStartRequest((HttpRequestMessage)null, 0, 0).RegisterException(e);
            }
            #endregion
            /// <summary>
            /// Вычисляет и перезаписывает значение коэффициента нагрузки данного канала
            /// </summary>
            public void RefreshLoadCoef()
            {
                // Свойство Uri используем как объект блокировки
                lock (this.Uri)
                {
                    this.LoadCoef = (this.RequestsList.Count + (this.msZonePing / 10) - this.Bandwith);
                    // Если коэффицент = 0, то прибавляем 1, т.к. у каналов может быть разный приоритет (а при делении нуля всё равно будет ноль) 
                    if (this.LoadCoef == 0) this.LoadCoef = 1;
                    if (this.LoadCoef > 0)
                        this.LoadCoef /= (this.Priority);
                    else this.LoadCoef *= this.Priority;
                }
            }
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    pingTimer.Dispose();
                    NotifyTimer.Dispose();
                }

            }
        }
    }
}