using System;
using System.Xml.Linq;
using zapad.Model.Security;
using zapad.Model.Tools;
using zapad.Public.WebInterface.Models.Tools;

namespace zapad.Public.WebInterface.Models.ServiceInteraction
{
    /// <summary>
    /// Реализует запросы к сервису WebHostCache
    /// </summary>
    public class WebHostCacheWrapper : IServiceAccess
    {
        /// <summary>
        /// Значение кода результата для вызовов API по умолчанию
        /// </summary>
        private const int DEFAULT_RC = Int32.MaxValue;

        /// <summary>
        /// Активировать email пользователя
        /// </summary>
        /// <param name="user">Пользователь, email которого нужно активировать</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>true - если пользователь активирован успешно, иначе - false</returns>
        public bool ActivateUserEmail(UserInfo user, string sessionKey)
        {
            var request = ApiHelpers.BuildRequest(sessionKey, UserInfo.ToXElement(user));
            var result = WebHostCache.Current.GetResponse<XElement>(@"api\Security\ActivateUserEmail", request);
            return (result.Element("Rc").getValue(DEFAULT_RC) == 0);
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
            var request = ApiHelpers.BuildRequest(sessionKey, UserInfo.ToXElement(user), new XElement("smsPassword", smsPassword));
            var result = WebHostCache.Current.GetResponse<XElement>(@"api\security\ActivateUserPhone", request);
            return result;
        }

        /// <summary>
        /// Добавить в систему нового пользователя
        /// </summary>
        /// <param name="user">Добавляемый пользователь</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Добавленный пользователь</returns>
        public UserInfo AddUser(UserInfo user, string sessionKey)
        {
            var request = ApiHelpers.BuildRequest(sessionKey, UserInfo.ToXElement(user));
            var result = WebHostCache.Current.GetResponse<XElement>(@"api\security\AddUser", request);
            user = UserInfo.FromXElement(result.Element("UserInfo"));

            return user;
        }

        /// <summary>
        /// Проверить СМС-пароль
        /// </summary>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="smsPassword">Проверяемый пароль</param>
        /// <returns>Результат проверки</returns>
        public XElement CheckSmsPassword(string sessionKey, string smsPassword)
        {
            var result = WebHostCache.Current.GetResponse<XElement>(@"api\security\CheckSmsPassword?sessionKey=" + sessionKey + "&smsPassword=" + smsPassword);
            return result;
        }

        /// <summary>
        /// Получить пользователей, зарегистрированных под указанным email
        /// </summary>
        /// <param name="email">email для поиска</param>
        /// <param name="email">Ключ текущей сессии</param>
        /// <returns>Массив записей пользователей</returns>
        public UserInfo[] GetUsersByEmail(string email, string sessionKey)
        {
            var result = WebHostCache.Current.GetResponse<XElement>(@"api\security\GetUsersByEmail?email=" + email + "&sessionKey=" + sessionKey);
            var retval = UserInfo.ArrayFromXElement(result.Element("Users"));
            return retval;
        }

        /// <summary>
        /// Получить пользователей по ID
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Массив записей пользователей</returns>
        public UserInfo[] GetUsersById(int id, string sessionKey)
        {
            var result = WebHostCache.Current.GetResponse<XElement>(@"api\security\GetUsersById?id=" + id + "&sessionKey=" + sessionKey);
            var retval = UserInfo.ArrayFromXElement(result.Element("Users"));
            return retval;
        }

        /// <summary>
        /// Запросить восстановление забытого/утерянного пароля
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Результат запроса</returns>
        public XElement RequestLostPasswordRestore(int userId, string sessionKey)
        {
            var result = WebHostCache.Current.GetResponse<XElement>(@"api\security\RequestLostPasswordRestore?userId=" + userId + "&sessionKey=" + sessionKey);
            return result;
        }

        /// <summary>
        /// Запросить отправку СМС-пароля
        /// </summary>
        /// <param name="userId">ID пользователя, для которого генерируется пароль</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Результат запроса</returns>
        public XElement RequestSmsPassword(int userId, string sessionKey)
        {
            var result = WebHostCache.Current.GetResponse<XElement>(@"api\security\RequestSmsPassword?userId=" + userId + "&sessionKey=" + sessionKey);
            return result;
        }

        /// <summary>
        /// Обновить сессию анонимного пользователя
        /// </summary>
        /// <param name="sessionKey">Ключ сессии</param>
        public void UpdateAnonymousSession(string sessionKey)
        {
            var request = ApiHelpers.BuildRequest(sessionKey);
            var result = WebHostCache.Current.GetResponse<XElement>(@"api/security/UpdateAnonymousSession", request);
        }

        /// <summary>
        /// Обновить поле IsAcceptAdmin пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        public void UpdateUserAcceptAdmin(int userId, string sessionKey)
        {
            var request = ApiHelpers.BuildRequest(sessionKey, new XElement("userId", userId));
            var result = WebHostCache.Current.GetResponse<XElement>(@"api\security\UpdateUserAcceptAdmin", request);
        }

        /// <summary>
        /// Обновить метку последней активности пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        public void UpdateUserLastActivity(int userId, string sessionKey)
        {
            var request = ApiHelpers.BuildRequest(sessionKey, new XElement("userId", userId));
            var result = WebHostCache.Current.GetResponse<XElement>(@"api\security\UpdateUserLastActivity", request);
        }

        /// <summary>
        /// Запросить права доступа к странице
        /// </summary>
        /// <param name="pageId">ID страницы</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>права доступа</returns>
        public XElement GetPageAccessRules(long pageId, string sessionKey)
        {
            var result = WebHostCache.Current.GetResponse<XElement>(@"api\security\GetPageAccessRules?pageId=" + pageId + "&sessionKey=" + sessionKey);
            return result;            
        }

        /// <summary>
        /// Выполнить выход пользователя
        /// </summary>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        public void Logout(string sessionKey)
        {
            var result = WebHostCache.Current.GetResponse<XElement>("api/security/logout?sessionKey=" + sessionKey);
        }
    }
}