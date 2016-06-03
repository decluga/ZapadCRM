using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zapad.crm.WebHostCache.Models.Authentification
{
    /// <summary>
    /// Класс для авторизации пользовательских запросов
    /// </summary>
    public static class Authenticate
    {
        /// <summary>
        /// Зарегистрировать GUID сессии пользователя и получить статус его аутентификации
        /// </summary>
        /// <param name="sessionKey">GUID сессии пользователя</param>
        /// <returns>Статус аутентификации</returns>
		public static bool registerGuid(Guid sessionKey)
        {
            if (SessionSet.Current.FindByWebUserId(sessionKey) == null)
            {
                SessionSet.Current.CreateSessionByWebUserId(sessionKey);
            }
            return SessionSet.Current.FindByWebUserId(sessionKey).IsAuthentificated;
        }
    }
}
