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
using System.Xml;
using System.Xml.Linq;
using System.ServiceModel;

namespace zapad.crm.WebHostCache.Models.Tools
{
    public class WebApiSync
    {
        /// <summary>
        /// Сертификат, используемый для отправки к WebApiSync для авторизации портала.
        /// </summary>
        public X509Certificate CertToZone { get; private set; }
        /// <summary>
        /// Список доступных каналов до WebApiSyncChannel
        /// </summary>
        public List<WebApiSyncChannel> ZoneChannelsList { get; private set; }
        /// <summary>
        /// Интервал между отправками уведомлений об отсутствии соединения (ответа ping) с WebApiSync (в мс)
        /// </summary>
        public double msNoPingNotifyInterval { get; private set; }
        #region Инициализация
        /// <summary>
        /// Приватный конструктор инициализации
        /// </summary>
        /// <param name="aZoneChannelsXml">Список каналов, используемых для соединения с WebApiSync в XML формате</param>
        /// <param name="CertificateToZoneThumbprint">Отпечаток сертификата из хранилища личных сертификатов данного пользователя, отправляемый в WebApiSync</param>
        /// <param name="msPingZoneInterval">Интервал между отправкой запросов ping в WebApiSync (в мс)</param>
        /// <param name="aPingRequestsCount">Количество запросов ping, отправляемых в WebApiSync для вычисления среднего значения</param>
        private WebApiSync(XmlDocument aWebApiSyncChannelsXml, string CertificateToZoneThumbprint, long msPingZoneInterval, int aPingRequestsCount, double aNoPingNotifyInterval)
        {
            this.msNoPingNotifyInterval = aNoPingNotifyInterval;
            this.ZoneChannelsList = new List<WebApiSyncChannel>();
            XElement endPoints = XElement.Load(new XmlNodeReader(aWebApiSyncChannelsXml));
            // Заполняем список каналов к WebApiSync с ненулевыми приоритетами
            foreach (XElement endPoint in endPoints.Elements("channel").Where(x => x.Attribute("priority").Value != "0"))
            {
                this.ZoneChannelsList.Add(new WebApiSyncChannel(endPoint.Value, endPoint.Attribute("priority").getValue(0),
                   endPoint.Attribute("bandwith").getValue(0f), this.CertToZone, msPingZoneInterval, aPingRequestsCount));
                if (endPoint.Attribute("fakeping") != null)
                    this.ZoneChannelsList.Last().msFakeDebugPing = endPoint.Attribute("fakeping").getValue(0);
            }
        }

        /// <summary>
        /// Экземпляр доступа к WebApiSync
        /// </summary>
        public static WebApiSync Current { get; private set; }
        /// <summary>
        /// Инициализация класса.
        /// </summary>
        /// <param name="WebApiZoneBaseAddress">Список каналов, используемых для соединения с WebApiSync в XML формате</param>
        /// <param name="CertificateToZoneThumbprint">Отпечаток сертификата из хранилища личных сертификатов данного пользователя, отправляемый в WebApiSync</param>
        /// <param name="msPingZoneInterval">Интервал между отправкой запросов ping в WebApiSync (в мс)</param>
        /// <param name="PingRequestsCount">Количество запросов ping, отправляемых в WebApiSync для вычисления среднего значения</param>
        public static void Create(XmlDocument WebApiZoneBaseAddress, string CertificateToZoneThumbprint, long msPingZoneInterval, int PingRequestsCount, double msNoPingNotifyInterval)
        { Current = new WebApiSync(WebApiZoneBaseAddress, CertificateToZoneThumbprint, msPingZoneInterval, PingRequestsCount, msNoPingNotifyInterval); }
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
            WebApiSyncChannel channel = this.ZoneChannelsList[0];//GetLessLoadedEndPoint();
            List<WebApiSyncChannel> UsedChannels = new List<WebApiSyncChannel>() { channel };
            WebApiSyncRequest request = new WebApiSyncRequest(channel, uri, UsedChannels, PostBody, tpe);
            HttpResponseMessage response = GetResult(channel, request, tpe);
            return response.Content.ReadAsAsync<TResult>().Result;
        }
        public byte[] GetResponseBinary(string uri, XElement PostBody = null)
        {
            WebApiSyncChannel channel = GetLessLoadedEndPoint();
            List<WebApiSyncChannel> UsedChannels = new List<WebApiSyncChannel>() { channel };
            WebApiSyncRequest request = new WebApiSyncRequest(channel, uri, UsedChannels, PostBody, ContentTypes.binary);
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
        private HttpResponseMessage GetResult(WebApiSyncChannel currentChannel, WebApiSyncRequest request, ContentTypes tpe = ContentTypes.xml)
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
                WebApiSyncChannel nextCannel = GetLessLoadedEndPoint(request.UsedChannels);
                request.UsedChannels.Add(nextCannel);
                WebApiSyncRequest newRequest = new WebApiSyncRequest(nextCannel, request.RequestUri.AbsolutePath,
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
            else throw new ArgumentOutOfRangeException(Thumbprint, "Не найден сертификат с данным отпечатком для связи с WebApiSync");
        }
        /// <summary>
        /// Выбирает самый менее нагруженный доступный канал (с минимальным коэффициентом нагрузки) 
        /// в случае возникновения ошибки по каналу, куда отправлено сообщение
        /// </summary>
        /// <param name="exceptChannels">Список каналов, которые не нужно учитывать (ранее были использованы для запроса)</param>
        /// <returns>Экземпляр ZoneChannel</returns>
        private WebApiSyncChannel GetLessLoadedEndPoint(List<WebApiSyncChannel> exceptChannels)
        {
            foreach (WebApiSyncChannel channel in ZoneChannelsList)
                channel.RefreshLoadCoef();
            if (exceptChannels == null) return GetLessLoadedEndPoint();
            if (exceptChannels.Count == this.ZoneChannelsList.Count)
                throw new ArgumentException("Не удалось отправить запрос по каждому из доступных каналов");
            return this.ZoneChannelsList.Except(exceptChannels).Where(x => x.HasConnection).Aggregate((i1, i2) => i1.LoadCoef < i2.LoadCoef ? i1 : i2);
        }
        private WebApiSyncChannel GetLessLoadedEndPoint()
        {
            foreach (WebApiSyncChannel channel in ZoneChannelsList)
                channel.RefreshLoadCoef();
            return this.ZoneChannelsList.Where(x => x.HasConnection).Aggregate((i1, i2) => i1.LoadCoef < i2.LoadCoef ? i1 : i2);
        }

        /// <summary>
        /// Класс запросов HttpRequestMessage, имеющий список использованных каналов для отправки запроса
        /// </summary>
        public class WebApiSyncRequest : HttpRequestMessage
        {
            /// <summary>
            /// Список использованных каналов для отправки данного запроса
            /// </summary>
            public List<WebApiSyncChannel> UsedChannels;
            /// <summary>
            /// Генерация экземпляра запроса на основе параметров
            /// </summary>
            /// <param name="channel">Канал, используемый для отправки запроса</param>
            /// <param name="uri">Целевой путь для отправки запроса</param>
            /// <param name="UsedChannels">Список использованных каналов для отправки данного запроса</param>
            /// <param name="PostBody">Тело Post-запроса (если есть)</param>
            /// <param name="tpe">Запрашиваемый заголовок запроса</param>
            public WebApiSyncRequest(WebApiSyncChannel channel, string uri, List<WebApiSyncChannel> UsedChannels, XElement PostBody = null, ContentTypes tpe = ContentTypes.xml) : base()
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
        /// Класс, описывающий канал для соединения с WebApiSync
        /// </summary>
        public class WebApiSyncChannel : IDisposable
        {
            /// <summary>
            /// Сертификат, используемый для аутентификации портала в WebApiSync.
            /// </summary>
            public X509Certificate CertToZone { get; private set; }
            /// <summary>
            /// Список запросов на канале, ожидающий ответа
            /// </summary>
            public SynchronizedCollection<WebApiSyncRequest> RequestsList { get; private set; }
            /// <summary>
            /// URI-адрес конечной точки
            /// </summary>
            public Uri Uri { get; private set; }
            /// <summary>
            /// Индивидуальный клиент для отправки запросов по определённому каналу
            /// </summary>
            public HttpClient Client { get; private set; }
            /// <summary>
            /// Среднее время отклика от WebApiSync по каналу
            /// </summary>
            public long msZonePing { get; private set; }
            /// <summary>
            /// Количество отправляемых запросов Ping в WebApiSync для вычисления среднего значения
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
            /// <param name="aCert">Сертификат, отправляемый для авторизации у WebApiSync по SSL. Если null, то сертификат к запросам не будет добавлен</param>
            /// <param name="aPingZoneInterval">Интервал таймера (в мс.), по истечению которого определяется пинг от WebApiSync</param>
            /// <param name="aPingRequestsCount">Количество запросов ping, отправляемых в тех WebApiSync для вычисления среднего значения</param>
            public WebApiSyncChannel(string aUri, int aPriority, float aBandwith, X509Certificate aCert, long aPingZoneInterval, int aPingRequestsCount)
            {
                this.InitializeClient(aUri, aCert);
                this.RequestsList = new SynchronizedCollection<WebApiSyncRequest>();
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
                // Связываемся с WebApiSync, даже если у него плохой сертификат (самоподписанный)
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
            /// Таймер проверки времени отклика от WebApiSync (в мс.)
            /// </summary>
            private Timer pingTimer;
            private void pingTimer_Elapsed(object sender, ElapsedEventArgs e) { this.PingZone(); }

            private int NoResponseCounter = 0;

            private Timer NotifyTimer;

            /// <summary>
            /// Определенее среднего времени отклика (в мс.) от WebApiSync по каналу. Запись значения в ZonePing.
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
                    // Пингуем WebApiSync ещё раз, чтобы не ждать следующей попытки пинга через время
                    if (this.NoResponseCounter < this.PingRequestsCount)
                        PingZone();
                    else
                    {
                        Exception exception = new Exception("Нет PING-ответа от " + this.Uri.ToString(), e);
                        // Чтобы не отправлять уведомления об отсутствии пинга, используем второй таймер для предотвращения
                        if (this.NotifyTimer == null)
                        {
                            this.NotifyTimer = new Timer(WebApiSync.Current.msNoPingNotifyInterval);
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
            /// Запись в БД и отправка СМС сообщения отсутствия соединения с WebApiSync.
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