using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zapad.Public.WebInterface.Models.ServiceInteraction
{
    /// <summary>
    /// Репозиторий SignalR слиентов для хаба ResponseHub
    /// </summary>
    static public class ResponseHubClientRepository
    {
        // Словарь клиентов, привязанных к сессии пользователя
        static private Dictionary<string, ResponseHubClient> _repos = new Dictionary<string, ResponseHubClient>();

        /// <summary>
        /// Получить клиента по ключу сессии
        /// </summary>
        /// <param name="sessionKey">Ключ сессии</param>
        /// <returns>Клиент для хаба ResponseHub</returns>
        static public ResponseHubClient GetClient(string sessionKey)
        {
            if (_repos.ContainsKey(sessionKey) == false)
                AddNewClient(sessionKey);
            return _repos[sessionKey];
        }

        /// <summary>
        /// Добавить нового клиента в репозиторий
        /// </summary>
        /// <param name="sessionKey">Ключ сессии</param>
        static private void AddNewClient(string sessionKey)
        {
            _repos.Add(sessionKey, new ResponseHubClient(sessionKey));
        }
    }
}