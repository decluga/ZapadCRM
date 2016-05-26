using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using zapad.Public.WebInterface.Models.Authorization;

namespace zapad.Public.WebInterface.Models.ServiceInteraction
{
    /// <summary>
    /// Реализует запросы к сервису WebHostCache
    /// </summary>
    public class WebHostCacheWrapper : IServiceAccess
    {
        /// <summary>
        /// Активировать email пользователя
        /// </summary>
        /// <param name="user">Пользователь, email которого нужно активировать</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>true - если пользователь активирован успешно, иначе - false</returns>
        public bool ActivateUserEmail(UserInfo user, string sessionKey)
        {
            // POST /security/ActivateUserEmail
            return true;
        }

        /// <summary>
        /// Активировать номер телефона пользователя
        /// </summary>
        /// <param name="user">Пользователь, номер которого нужно активировать</param>
        /// <param name="sessionKey">ключ текущей сессии</param>
        /// <param name="smsPassword">СМС-пароль</param>
        /// <returns></returns>
        public XElement ActivateUserPhone(UserInfo user, string sessionKey, string smsPassword)
        {
            // POST /security/ActivateUserPhone

            return new XElement("rc", 0);
        }

        /// <summary>
        /// Добавить в систему нового пользователя
        /// </summary>
        /// <param name="user">Добавляемый пользователь</param>
        /// <returns>Добавленный пользователь</returns>
        public UserInfo AddUser(UserInfo user)
        {
            // POST /security/AddUser

            return new UserInfo();
        }

        /// <summary>
        /// Проверить СМС-пароль
        /// </summary>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="smsPassword">Проверяемый пароль</param>
        /// <returns>Результат проверки</returns>
        public XElement CheckSmsPassword(string sessionKey, string smsPassword)
        {
            // GET /security/CheckSmsPassword

            return new XElement("rc", 0);
        }

        /// <summary>
        /// Получить пользователей, зарегистрированных под указанным email
        /// </summary>
        /// <param name="email">email для поиска</param>
        /// <returns>Массив записей пользователей</returns>
        public UserInfo[] GetUsersByEmail(string email)
        {
            // GET /security/GetUsersByEmail

            return new UserInfo[] { new UserInfo() };
        }

        /// <summary>
        /// Получить пользователей по ID
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <returns>Массив записей пользователей</returns>
        public UserInfo[] GetUsersById(int id)
        {
            // GET /security/GetUsersById

            return new UserInfo[] { new UserInfo() };
        }

        /// <summary>
        /// Запросить восстановление забытого/утерянного пароля
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Результат запроса</returns>
        public XElement RequestLostPasswordRestore(int userId, string sessionKey)
        {
            // GET /security/RequestLostPasswordRestore

            return new XElement("rc", 0);
        }

        /// <summary>
        /// Запросить повторную отправку пароля
        /// </summary>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Результат запроса</returns>
        public XElement RequestResendPassword(string sessionKey)
        {
            // GET /security/RequestResendPassword

            return new XElement("rc", 0);
        }

        /// <summary>
        /// Запросить отправку СМС-пароля
        /// </summary>
        /// <param name="userId">ID пользователя, для которого генерируется пароль</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Результат запроса</returns>
        public XElement RequestSmsPassword(int userId, string sessionKey)
        {
            // GET /security/RequestSmsPassword

            return new XElement("rc", 0);
        }

        /// <summary>
        /// Обновить сессию анонимного пользователя
        /// </summary>
        /// <param name="sessionKey">Ключ сессии</param>
        public void UpdateAnonymousSession(string sessionKey)
        {
            // POST /security/UpdateAnonymousSession
        }

        /// <summary>
        /// Обновить поле IsAcceptAdmin пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        public void UpdateUserAcceptAdmin(int userId)
        {
            // POST /security/UpdateUserAcceptAdmin
        }

        /// <summary>
        /// Обновить метку последней активности пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        public void UpdateUserLastActivity(int userId)
        {
            // POST /security/UpdateUserLastActivity
        }

        /// <summary>
        /// Запросить права доступа к странице
        /// </summary>
        /// <param name="pageId">ID страницы</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>права доступа</returns>
        public XElement GetPageAccessRules(long pageId, string sessionKey)
        {
            // GET /security/GetPageAccessRules

            return new XElement("rc", 0);
        }
    }
}