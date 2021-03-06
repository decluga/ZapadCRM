﻿using System.Xml.Linq;
using zapad.Model.Security;

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
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Массив записей пользователей</returns>
        UserInfo[] GetUsersByEmail(string email, string sessionKey);

        /// <summary>
        /// Получить пользователей по ID
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Массив записей пользователей</returns>
        UserInfo[] GetUsersById(int id, string sessionKey);

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
        /// <param name="sessionKey">Ключ текущей сессии</param>
        void UpdateUserAcceptAdmin(int userId, string sessionKey);

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
        /// <param name="sessionKey">Ключ текущей сессии</param>
        void UpdateUserLastActivity(int userId, string sessionKey);

        /// <summary>
        /// Добавить в систему нового пользователя
        /// </summary>
        /// <param name="user">Добавляемый пользователь</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Добавленный пользователь</returns>
        UserInfo AddUser(UserInfo user, string sessionKey);

        /// <summary>
        /// Запросить права доступа к странице
        /// </summary>
        /// <param name="pageId">ID страницы</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Статус операции</returns>
        XElement GetPageAccessRules(long pageId, string sessionKey);

        /// <summary>
        /// Выполнить выход пользователя
        /// </summary>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        void Logout(string sessionKey);
    }
}
