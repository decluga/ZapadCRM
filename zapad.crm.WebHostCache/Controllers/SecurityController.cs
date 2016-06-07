using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using zapad.crm.WebHostCache.Models.Tools;
using zapad.Model.API;

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
        public XElement ActivateUserEmail([FromBody] XElement request)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"api\Anonymous\activate_email", request);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }

        /// <summary>
        /// Активировать номер телефона пользователя
        /// </summary>
        /// <param name="request">XML с данными пользователя, ключом текущей сессии и SMS-паролем</param>
        /// <returns></returns>
        [HttpPost]
        public XElement ActivateUserPhone([FromBody] XElement request)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"api\Anonymous\activate_phone", request);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }

        /// <summary>
        /// Добавить в систему нового пользователя
        /// </summary>
        /// <param name="user">XML с данными добавляемого пользователя</param>
        /// <returns>Добавленный пользователь</returns>
        [HttpPost]
        public XElement AddUser([FromBody] XElement user)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"api\Anonymous\AddUser", user);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }

        /// <summary>
        /// Проверить СМС-пароль
        /// </summary>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="smsPassword">Проверяемый пароль</param>
        /// <param name="requestId">ID запроса</param>
        /// <returns>Результат проверки</returns>
        [HttpGet]
        public XElement CheckSmsPassword(string sessionKey, string smsPassword, long requestId)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"/api/Anonymous/login?sessionKey=" + sessionKey + "&smsPassword=" + smsPassword + "&requestId=" + requestId);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }

        /// <summary>
        /// Получить пользователей, зарегистрированных под указанным email
        /// </summary>
        /// <param name="email">email для поиска</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="requestId">Id запроса</param>
        /// <returns>Массив записей пользователей</returns>
        [HttpGet]
        public XElement GetUsersByEmail(string email, string sessionKey, long requestId)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"/api/Anonymous/GetUsersByEmail?email=" + email + "&sessionKey=" + sessionKey + "&requestId=" + requestId);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }

        /// <summary>
        /// Получить пользователей по ID
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="requestId">Id запроса</param>
        /// <returns>Массив записей пользователей</returns>
        [HttpGet]
        public XElement GetUsersById(int id, string sessionKey, long requestId)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"/api/Anonymous/GetUsersById?id=" + id + "&sessionKey=" + sessionKey + "&requestId=" + requestId);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }

        /// <summary>
        /// Запросить восстановление забытого/утерянного пароля
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="requestId">Id запроса</param>
        /// <returns>Результат запроса</returns>
        [HttpGet]
        public XElement RequestLostPasswordRestore(int userId, string sessionKey, long requestId)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"/api/Anonymous/lostpwd?userId=" + userId + "&sessionKey=" + sessionKey + "&requestId=" + requestId);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }

        /// <summary>
        /// Запросить отправку СМС-пароля
        /// </summary>
        /// <param name="userId">ID пользователя, для которого генерируется пароль</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="requestId">Id запроса</param>
        /// <returns>Результат запроса</returns>
        [HttpGet]
        public XElement RequestSmsPassword(int userId, string sessionKey, long requestId)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"/api/Anonymous/take_pwd?userId=" + userId + "&sessionKey=" + sessionKey + "&requestId=" + requestId.ToString());
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }

        /// <summary>
        /// Обновить сессию анонимного пользователя
        /// </summary>
        /// <param name="sessionKey">Ключ сессии</param>
        [HttpPost]
        public XElement UpdateAnonymousSession([FromBody] XElement request)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"/api/Anonymous/UpdateAnonymousSession", request);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }

        /// <summary>
        /// Обновить поле IsAcceptAdmin пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        [HttpPost]
        public XElement UpdateUserAcceptAdmin([FromBody] XElement request)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"/api/Anonymous/UpdateUserAcceptAdmin", request);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }

        /// <summary>
        /// Обновить метку последней активности пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        [HttpPost]
        public XElement UpdateUserLastActivity([FromBody] XElement request)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"/api/Anonymous/UpdateUserLastActivity", request);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }

        /// <summary>
        /// Запросить права доступа к странице
        /// </summary>
        /// <param name="pageId">ID страницы</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="requestId">Id запроса</param>
        /// <returns>права доступа</returns>
        [HttpGet]
        public XElement GetPageAccessRules(long pageId, string sessionKey, long requestId)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"api/Access/Page?pageId=" + pageId + "&sessionKey=" + sessionKey + "&requestId=" + requestId);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }

        /// <summary>
        /// Выполнить выход пользователя
        /// </summary>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="requestId">Id запроса</param>
        /// <returns>Результат запроса</returns>
        [HttpGet]
        public XElement logout(string sessionKey, long requestId)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"/api/Anonymous/logout?sessionKey=" + sessionKey + "&requestId=" + requestId.ToString());
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }
    }
}