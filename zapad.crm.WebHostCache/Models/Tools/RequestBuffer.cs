using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Xml.Linq;
using static zapad.Public.WebInterface.Models.Tools.WebHostCache;

namespace zapad.crm.WebHostCache.Models.Tools
{
    ///// <summary>
    ///// Делегат для методов отправки запросов
    ///// </summary>
    ///// <param name="uri"></param>
    ///// <param name="PostBody"></param>
    ///// <param name="tpe"></param>
    ///// <returns></returns>
    //public delegate XElement RequestSender(string uri, XElement PostBody = null, ContentTypes tpe = ContentTypes.xml);

    /// <summary>
    /// Буфер для отправки запросов
    /// </summary>
    class RequestBuffer
    {
        /// <summary>
        /// Внутренний список запросов
        /// </summary>
        private List<RequestParameters> _requests = new List<RequestParameters>();

        /// <summary>
        /// Объект для блокировки
        /// </summary>
        private object _locker = new object();

        /// <summary>
        /// Метод для отправки запросов
        /// </summary>
        private Func<string, XElement, WebApiSync.ContentTypes, XElement> _senderMethod;

        /// <summary>
        /// Список запросов, которые можно удалить
        /// </summary>
        private List<RequestParameters> _requestsToDelete = new List<RequestParameters>();

        private Timer _resendTimer;

        /// <summary>
        /// Инициализировать буфер
        /// </summary>
        /// <param name="sender">Метод для отправки запросов</param>
        /// <param name="msResendInterval">Интервал повторной отправки запросов</param>
        public RequestBuffer(Func<string, XElement, WebApiSync.ContentTypes, XElement> sender, double msResendInterval)
        {
            _senderMethod = sender;
            _resendTimer = new Timer(msResendInterval);
            _resendTimer.Elapsed += _resendTimer_Elapsed;
            _resendTimer.Start();
        }

        /// <summary>
        /// Обработчик срабатывания таймера повторной отправки
        /// </summary>
        /// <param name="sender">Отправитель события</param>
        /// <param name="e">Параметры события</param>
        private void _resendTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SendAllRequests();
        }

        /// <summary>
        /// Добавить новый запрос в буфер
        /// </summary>
        /// <param name="uri">URL запроса</param>
        /// <param name="PostBody">Тело POST-запроса</param>
        /// <param name="tpe">Тип принимаемого контента</param>
        public void AddRequest(string uri, XElement PostBody = null, WebApiSync.ContentTypes tpe = WebApiSync.ContentTypes.xml)
        {
            var req = new RequestParameters(uri, PostBody, tpe);

            // Если не удается отправить запрос сходу, добавляем его в буфер
            if (!SendRequest(req))
                _requests.Add(req);
        }

        /// <summary>
        /// Отправить все запросы получателю
        /// </summary>
        private void SendAllRequests()
        {
            lock (_locker)
            {
                _requestsToDelete.Clear();
                Parallel.ForEach(_requests, (req) => SendRequest(req));
                _requests.RemoveAll(r => _requestsToDelete.Contains(r));
                _requestsToDelete.Clear();
            }
        }

        /// <summary>
        /// Отправить запрос получателю
        /// </summary>
        /// <param name="req">Параметры отправляемого запроса</param>
        private bool SendRequest(RequestParameters req)
        {
            XElement result = null;
            bool retVal = false;

            try
            {
                result = _senderMethod(req.URL, req.PostBody, req.ContentType);
            }
            catch
            {
                // TODO: Добавить логирование возникшей ошибки
            }

            if (result != null && int.Parse(result.Element("Rc").Value) == 0)
            {
                _requestsToDelete.Add(req);
                retVal = true;
                // TODO: добавить логирование возникшей ошибки
            }

            return retVal;
        }

        /// <summary>
        /// Хранит параметры отправляемого запроса
        /// </summary>
        private class RequestParameters
        {
            /// <summary>
            /// URL запроса
            /// </summary>
            public string URL { get; private set; }

            /// <summary>
            /// Тело POST-запроса (null для GET-запросов)
            /// </summary>
            public XElement PostBody { get; private set; }

            /// <summary>
            /// Тип принимаемого контента
            /// </summary>
            public WebApiSync.ContentTypes ContentType { get; private set; }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="url">URL запроса</param>
            /// <param name="postBody">Тело POST-запроса</param>
            /// <param name="contentType">Тип принимаемого контента</param>
            public RequestParameters(string url, XElement postBody, WebApiSync.ContentTypes contentType)
            {
                URL = url;
                PostBody = postBody;
                ContentType = contentType;
            }
        }
    }
}
