using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using zapad.Public.WebInterface.Models.Authorization;

namespace zapad.Public.WebInterface.Models.ServiceInteraction
{
    /// <summary>
    /// Интерфейс для доступа к данным через сервисы
    /// </summary>
    interface IServiceAccess
    {
        /// <summary>
        /// Обновить сессию анонимного пользователя
        /// </summary>
        /// <param name="sessionKey">Ключ сессии</param>
        void UpdateAnonymousSession(string sessionKey);

        /// <summary>
        /// Получить пользователей, зарегистрированных под указанным email
        /// </summary>
        /// <param name="email">email для поиска</param>
        /// <returns>Массив записей пользователей</returns>
        UserInfo[] GetUsersByEmail(string email);

        /// <summary>
        /// Получить пользователей по ID
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <returns>Массив записей пользователей</returns>
        UserInfo[] GetUsersById(int id);

        /// <summary>
        /// Активировать email пользователя
        /// </summary>
        /// <param name="user">Пользователь, email которого нужно активировать</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>true - если пользователь активирован успешно, иначе - false</returns>
        bool ActivateUserEmail(UserInfo user, string sessionKey);

        /// <summary>
        /// Активировать номер телефона пользователя
        /// </summary>
        /// <param name="user">Пользователь, номер которого нужно активировать</param>
        /// <param name="sessionKey">ключ текущей сессии</param>
        /// <param name="smsPassword">СМС-пароль</param>
        /// <returns></returns>
        XElement ActivateUserPhone(UserInfo user, string sessionKey, string smsPassword);

        /// <summary>
        /// Запросить отправку СМС-пароля
        /// </summary>
        /// <param name="userId">ID пользователя, для которого генерируется пароль</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Результат запроса</returns>
        XElement RequestSmsPassword(int userId, string sessionKey);

        /// <summary>
        /// Проверить СМС-пароль
        /// </summary>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="smsPassword">Проверяемый пароль</param>
        /// <returns>Результат проверки</returns>
        XElement CheckSmsPassword(string sessionKey, string smsPassword);

        /// <summary>
        /// Обновить поле IsAcceptAdmin пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        void UpdateUserAcceptAdmin(int userId);

        /// <summary>
        /// Запросить восстановление забытого/утерянного пароля
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Результат запроса</returns>
        XElement RequestLostPasswordRestore(int userId, string sessionKey);

        /// <summary>
        /// Обновить метку последней активности пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        void UpdateUserLastActivity(int userId);

        /// <summary>
        /// Запросить повторную отправку пароля
        /// </summary>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Результат запроса</returns>
        XElement RequestResendPassword(string sessionKey);

        /// <summary>
        /// Добавить в систему нового пользователя
        /// </summary>
        /// <param name="user">Добавляемый пользователь</param>
        /// <returns>Добавленный пользователь</returns>
        UserInfo AddUser(UserInfo user);

        /// <summary>
        /// Запросить права доступа к странице
        /// </summary>
        /// <param name="pageId">ID страницы</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns></returns>
        XElement GetPageAccessRules(long pageId, string sessionKey);
    }
}
