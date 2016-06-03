using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using zapad.crm.WebHostCache.Models.Tools;

namespace zapad.crm.WebHostCache.Controllers
{
    public class SecurityController : ApiController
    {
        /// <summary>
        /// Активировать email пользователя
        /// </summary>
        /// <param name="request">XML с данными пользователя и ключом текущей сессии</param>
        /// <returns>true - если пользователь активирован успешно, иначе - false</returns>
        [HttpPost]
        public async Task<XElement> ActivateUserEmail([FromBody] XElement request)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"api\Anonymous\activate_email", request);
        }

        /// <summary>
        /// Активировать номер телефона пользователя
        /// </summary>
        /// <param name="request">XML с данными пользователя, ключом текущей сессии и SMS-паролем</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<XElement> ActivateUserPhone([FromBody] XElement request)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"api\Anonymous\activate_phone", request);
        }

        /// <summary>
        /// Добавить в систему нового пользователя
        /// </summary>
        /// <param name="user">XML с данными добавляемого пользователя</param>
        /// <returns>Добавленный пользователь</returns>
        [HttpPost]
        public async Task<XElement> AddUser([FromBody] XElement user)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"api\Anonymous\AddUser", user);
        }

        /// <summary>
        /// Проверить СМС-пароль
        /// </summary>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="smsPassword">Проверяемый пароль</param>
        /// <param name="requestId">ID запроса</param>
        /// <returns>Результат проверки</returns>
        [HttpGet]
        public async Task<XElement> CheckSmsPassword(string sessionKey, string smsPassword, long requestId)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"/api/Anonymous/login?sessionKey=" + sessionKey + "&smsPassword=" + smsPassword + "&requestId=" + requestId);
        }

        /// <summary>
        /// Получить пользователей, зарегистрированных под указанным email
        /// </summary>
        /// <param name="email">email для поиска</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="requestId">Id запроса</param>
        /// <returns>Массив записей пользователей</returns>
        [HttpGet]
        public async Task<XElement> GetUsersByEmail(string email, string sessionKey, long requestId)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"/api/Anonymous/GetUsersByEmail?email=" + email + "&sessionKey=" + sessionKey + "&requestId=" + requestId);
        }

        /// <summary>
        /// Получить пользователей по ID
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="requestId">Id запроса</param>
        /// <returns>Массив записей пользователей</returns>
        [HttpGet]
        public async Task<XElement> GetUsersById(int id, string sessionKey, long requestId)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"/api/Anonymous/GetUsersById?id=" + id + "&sessionKey=" + sessionKey + "&requestId=" + requestId);
        }

        /// <summary>
        /// Запросить восстановление забытого/утерянного пароля
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="requestId">Id запроса</param>
        /// <returns>Результат запроса</returns>
        [HttpGet]
        public async Task<XElement> RequestLostPasswordRestore(int userId, string sessionKey, long requestId)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"/api/Anonymous/lostpwd?userId=" + userId + "&sessionKey=" + sessionKey + "&requestId=" + requestId);
        }

        /// <summary>
        /// Запросить отправку СМС-пароля
        /// </summary>
        /// <param name="userId">ID пользователя, для которого генерируется пароль</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="requestId">Id запроса</param>
        /// <returns>Результат запроса</returns>
        [HttpGet]
        public async Task<XElement> RequestSmsPassword(int userId, string sessionKey, long requestId)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"/api/Anonymous/take_pwd?userId=" + userId + "&sessionKey=" + sessionKey + "&requestId=" + requestId.ToString());
        }

        /// <summary>
        /// Обновить сессию анонимного пользователя
        /// </summary>
        /// <param name="sessionKey">Ключ сессии</param>
        [HttpPost]
        public async Task<XElement> UpdateAnonymousSession([FromBody] XElement request)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"/api/Anonymous/UpdateAnonymousSession", request);
        }

        /// <summary>
        /// Обновить поле IsAcceptAdmin пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        [HttpPost]
        public async Task<XElement> UpdateUserAcceptAdmin([FromBody] XElement request)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"/api/Anonymous/UpdateUserAcceptAdmin", request);
        }

        /// <summary>
        /// Обновить метку последней активности пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        [HttpPost]
        public async Task<XElement> UpdateUserLastActivity([FromBody] XElement request)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"/api/Anonymous/UpdateUserLastActivity", request);
        }

        /// <summary>
        /// Запросить права доступа к странице
        /// </summary>
        /// <param name="pageId">ID страницы</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="requestId">Id запроса</param>
        /// <returns>права доступа</returns>
        [HttpGet]
        public async Task<XElement> GetPageAccessRules(long pageId, string sessionKey, long requestId)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"api/Access/Page?pageId=" + pageId + "&sessionKey=" + sessionKey + "&requestId=" + requestId);
        }

        /// <summary>
        /// Выполнить выход пользователя
        /// </summary>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="requestId">Id запроса</param>
        /// <returns>Результат запроса</returns>
        [HttpGet]
        public async Task<XElement> logout(string sessionKey, long requestId)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"/api/Anonymous/logout?sessionKey=" + sessionKey + "&requestId=" + requestId.ToString());
        }
    }
}
