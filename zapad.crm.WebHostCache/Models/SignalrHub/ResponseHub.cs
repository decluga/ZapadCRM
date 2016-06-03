using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace zapad.crm.SignalR
{
    /// <summary>
    /// SignalR Hub для передачи ответов на запросы в цепочке API
    /// </summary>
    public class ResponseHub : Hub<IResponseProcessor>
    {
        // Словарь, сопоставляющий ConnectionId и ключи сессий
        static private Dictionary<string, string> sessionKeys = new Dictionary<string, string>();

        /// <summary>
        /// Регистрирует нового клиента
        /// Обязательная операция, чтобы клиент имел возможность получать сообщения
        /// </summary>
        /// <param name="sessionKey">Ключ сессии клиента</param>
        public void RegisterClient(string sessionKey)
        {
            if (sessionKeys.ContainsKey(sessionKey))
                sessionKeys[sessionKey] = Context.ConnectionId;
            else
                sessionKeys.Add(sessionKey, Context.ConnectionId);
        }

        ///// <summary>
        ///// Колбэк для API-вызова обновления анонимной сессии
        ///// </summary>
        ///// <param name="sessionKey">Ключ сессии</param>
        ///// <param name="requestId">ID исходного запроса</param>
        ///// <param name="result">Результат вызова</param>
        //public void UpdateAnonymousSessionCallback(string sessionKey, long requestId, XElement result)
        //{
        //    Clients.Client(sessionKeys[sessionKey]).UpdateAnonymousSessionCallback(sessionKey, requestId, result);
        //}

        /// <summary>
        /// Колбэк для API-вызова
        /// </summary>
        /// <param name="sessionKey">Ключ сессии</param>
        /// <param name="requestId">ID исходного запроса</param>
        /// <param name="result">Результат вызова</param>
        public void OperationCallback(string sessionKey, long requestId, XElement result)
        {
            Clients.Client(sessionKeys[sessionKey]).OperationCallback(sessionKey, requestId, result);
        }
    }
}
