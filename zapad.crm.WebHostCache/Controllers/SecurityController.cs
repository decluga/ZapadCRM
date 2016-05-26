using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using zapad.Public.WebInterface.Models.Authorization;

namespace zapad.crm.WebHostCache.Controllers
{
    class SecurityController : ApiController
    {
        /// <summary>
        /// Активировать email пользователя
        /// </summary>
        /// <param name="user">Пользователь, email которого нужно активировать</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>true - если пользователь активирован успешно, иначе - false</returns>
        [HttpPost]
        public async Task<XElement> ActivateUserEmail(UserInfo user, string sessionKey)
        {
            //XElement xanswer = WebApiZone.Current.GetResponse<XElement>("/anonymous/activate_email", new XElement("request",
            //    new XElement("UserId", session.User.UserId),
            //    new XElement("Phone", session.User.Phone),
            //    new XElement("EMail", session.User.EMail),
            //    new XElement("F", session.User.F),
            //    new XElement("I", session.User.I),
            //    new XElement("O", session.User.O),
            //                    new XElement("whguid", session.Key.ToString())
            //    ));
            //using (webhostdbConnection db = new webhostdbConnection())
            //{
            //    db.Database.ExecuteSqlCommand("UPDATE dbo.UserInfo SET IsActivatedEmail=1, LastActivity=GETDATE() WHERE UserId=@UserId", new SqlParameter("@UserId", session.User.UserId));
            //}

            throw new NotImplementedException();
        }

        /// <summary>
        /// Активировать номер телефона пользователя
        /// </summary>
        /// <param name="user">Пользователь, номер которого нужно активировать</param>
        /// <param name="sessionKey">ключ текущей сессии</param>
        /// <param name="smsPassword">СМС-пароль</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<XElement> ActivateUserPhone(UserInfo user, string sessionKey, string smsPassword)
        {
            //XElement xanswer = WebApiZone.Current.GetResponse<XElement>("/anonymous/activate_phone/",
            //    new XElement("request",
            //        new XElement("id", session.User.UserId),
            //        new XElement("pwd", smspwd),
            //        new XElement("whguid", session.Key.ToString())),
            //    WebApiZone.ContentTypes.xml);
            //if (int.Parse(xanswer.Element("rc").Value) == 0)
            //    db.Database.ExecuteSqlCommand("UPDATE dbo.UserInfo SET IsActivatedPhone=1, LastActivity=GETDATE() WHERE UserId=@Id", new SqlParameter("@Id", session.User.UserId));
            //return xanswer

            throw new NotImplementedException();
        }

        /// <summary>
        /// Добавить в систему нового пользователя
        /// </summary>
        /// <param name="user">Добавляемый пользователь</param>
        /// <returns>Добавленный пользователь</returns>
        [HttpPost]
        public async Task<XElement> AddUser(UserInfo user)
        {
            // db.UserInfo.Add(user);
            // db.savechanges();
            // return user;

            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверить СМС-пароль
        /// </summary>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <param name="smsPassword">Проверяемый пароль</param>
        /// <returns>Результат проверки</returns>
        [HttpGet]
        public async Task<XElement> CheckSmsPassword(string sessionKey, string smsPassword)
        {
            //return WebApiZone.Current.GetResponse<XElement>("/anonymous/login/whguid=" + session.Key.ToString() + "&pwd=" + sms);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить пользователей, зарегистрированных под указанным email
        /// </summary>
        /// <param name="email">email для поиска</param>
        /// <returns>Массив записей пользователей</returns>
        [HttpGet]
        public async Task<XElement> GetUsersByEmail(string email)
        {
            //UserInfo[] rows = db.Database.SqlQuery<UserInfo>("SELECT * FROM dbo.UserInfo w WHERE UPPER(w.EMail)=UPPER(@email)", new SqlParameter("@email", request.reg_Email)).ToArray();
            //return rows;

            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить пользователей по ID
        /// </summary>
        /// <param name="id">ID для поиска</param>
        /// <returns>Массив записей пользователей</returns>
        [HttpGet]
        public async Task<XElement> GetUsersById(int id)
        {
            //using (webhostdbConnection db = new webhostdbConnection())
            //{
            //    UserInfo[] rows = db.Database.SqlQuery<UserInfo>("SELECT * FROM dbo.UserInfo w WHERE w.UserId=@UserId", new SqlParameter("@UserId", id)).ToArray();
            //    return rows;
            //}

            throw new NotImplementedException();
        }

        /// <summary>
        /// Запросить восстановление забытого/утерянного пароля
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Результат запроса</returns>
        [HttpGet]
        public async Task<XElement> RequestLostPasswordRestore(int userId, string sessionKey)
        {
            //return WebApiZone.Current.GetResponse<XElement>("/anonymous/lostpwd/id=" + session.User.UserId + "&whguid=" + session.Key.ToString());

            throw new NotImplementedException();
        }

        /// <summary>
        /// Запросить повторную отправку пароля
        /// </summary>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Результат запроса</returns>
        [HttpGet]
        public async Task<XElement> RequestResendPassword(string sessionKey)
        {
            //return WebApiZone.Current.GetResponse<XElement>("/anonymous/not_send_pwd/" + session.Key.ToString());

            throw new NotImplementedException();
        }

        /// <summary>
        /// Запросить отправку СМС-пароля
        /// </summary>
        /// <param name="userId">ID пользователя, для которого генерируется пароль</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>Результат запроса</returns>
        [HttpGet]
        public async Task<XElement> RequestSmsPassword(int userId, string sessionKey)
        {
            //return WebApiZone.Current.GetResponse<XElement>("/anonymous/take_pwd/id=" + session.User.UserId.ToString() + "&whguid=" + session.Key.ToString());

            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновить сессию анонимного пользователя
        /// </summary>
        /// <param name="sessionKey">Ключ сессии</param>
        [HttpPost]
        public async Task<XElement> UpdateAnonymousSession(string sessionKey)
        {
            //WebApiZone.Current.GetResponse<XElement>("/anonymous/updateSession/whguid=" + session.Key.ToString());

            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновить поле IsAcceptAdmin пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        [HttpPost]
        public async Task<XElement> UpdateUserAcceptAdmin(int userId)
        {
            //db.Database.ExecuteSqlCommand("UPDATE dbo.UserInfo SET LastActivity=GETDATE(), IsAcceptAdmin=1 WHERE UserId=@UserId", new SqlParameter("@UserId", session.User.UserId));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновить метку последней активности пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        [HttpPost]
        public async Task<XElement> UpdateUserLastActivity(int userId)
        {
            // db.Database.ExecuteSqlCommand("UPDATE dbo.UserInfo SET LastActivity=GETDATE() WHERE UserId=@UserId", new SqlParameter("@UserId", session.User.UserId));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Запросить права доступа к странице
        /// </summary>
        /// <param name="pageId">ID страницы</param>
        /// <param name="sessionKey">Ключ текущей сессии</param>
        /// <returns>права доступа</returns>
        [HttpGet]
        public async Task<XElement> GetPageAccessRules(long pageId, string sessionKey)
        {
            //XElement request = new XElement("request");
            //request.Add(new XElement("whguid", session.Key.ToString()));
            //request.Add(new XElement("Items", Pages));
            //return WebApiZone.Current.GetResponse<XElement>("/access/Page", request, WebApiZone.ContentTypes.xml);

            throw new NotImplementedException();
        }
    }
}
