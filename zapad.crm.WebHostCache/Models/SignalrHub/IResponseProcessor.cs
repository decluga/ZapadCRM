using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace zapad.crm.SignalR
{
    /// <summary>
    /// Интерфейс для хаба для передачи ответов на запросы в цепочке API
    /// </summary>
    public interface IResponseProcessor
    {
        /// <summary>
        /// Получает результат запроса от WebApiSync и передает его в WebInterface
        /// </summary>
        /// <param name="sessionKey">Ключ сессии</param>
        /// <param name="requestId">ID исходного запроса</param>
        /// <param name="result">Результат запроса</param>
        void OperationCallback(string sessionKey, long requestId, XElement result);

        /// <summary>
        /// Получает результат вызова UpdateAnonymousSession от WebApiSync и передает его в WebInterface
        /// </summary>
        /// <param name="sessionKey">Ключ сессии обновляемого пользователя</param>
        /// <param name="requestId">ID исходного запроса</param>
        /// <param name="result">Результат вызова</param>
        //void UpdateAnonymousSessionCallback(string sessionKey, long requestId, XElement result);
    }
}
