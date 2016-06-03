using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml.Linq;
using zapad.Public.WebInterface.Properties;

namespace zapad.Public.WebInterface.Models.ServiceInteraction
{
    /// <summary>
    /// Клиент для хаба ResponseHub
    /// </summary>
    public class ResponseHubClient
    {
        private HubConnection _hubConnection = null;
        private IHubProxy _hubProxy = null;
                
        public ResponseHubClient(string sessionKey)
        {
            _hubConnection = new HubConnection(Settings.Default.ResponseHubUrl);
            _hubProxy = _hubConnection.CreateHubProxy("ResponseHub");
            RegisterHandlers();
            _hubConnection.Start().Wait();
            _hubProxy.Invoke("RegisterClient", sessionKey).Wait();
        }

        /// <summary>
        /// Регистрирует обработчики сообщений хаба, которые может обработать клиент
        /// </summary>
        private void RegisterHandlers()
        {
            //_hubProxy.On<string, long, XElement>("UpdateAnonymousSessionCallback", UpdateAnonymousSessionCallback);
            _hubProxy.On<string, long, XElement>("OperationCallback", OperationCallback);
        }

        /// <summary>
        /// Словарь хэнлеров для ожидания результатов запроса
        /// </summary>
        public Dictionary<long, AutoResetEvent> WaitHandlers { get; private set; } = new Dictionary<long, AutoResetEvent>();

        /// <summary>
        /// Словарь результатов запроса
        /// </summary>
        public Dictionary<long, object> RequestResults { get; private set; } = new Dictionary<long, object>();

        /// <summary>
        /// Обработчик сообщения "OperationCallback" хаба
        /// </summary>
        /// <param name="sessionKey">Ключ сессии</param>
        /// <param name="requestId">ID исходного запроса</param>
        /// <param name="result">Результат API-запроса</param>
        public void OperationCallback(string sessionKey, long requestId, XElement result)
        {
            RequestResults.Add(requestId, result);
            WaitHandlers[requestId].Set();
        }
    }
}