using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using zapad.crm.WebApiSync.Properties;
using zapad.Model.API;
using zapad.Model.Security;

namespace zapad.crm.WebApiSync.Controllers
{
    public class AnonymousController : ApiController
    {
        #region Заглушка
        static private List<UserInfo> users = new List<UserInfo>();
        static AnonymousController()
        {
            UserInfo u = new UserInfo()
            {
                UserId = 23,
                EMail = "1@1.com",
                IsActivatedEMail = true,
                IsActivatedPhone = true,
                F = "Одинов",
                I = "Раз",
                O = "Разович"
            };

            users.Add(u);
        }
        #endregion

        // Обновляет метку последней активности анонимного пользователя. Соответствует операции
        //       WebApiZone.Current.GetResponse<XElement>("/anonymous/updateSession/whguid=" + session.Key.ToString());
        // Структуру запроса см. в zapad.Public.WebInterface.Models.ServiceInteraction.WebHostCacheWrapper.UpdateAnonymousSession()
        // Ожидаемый ответ - обычный RC
        [HttpPost]
        public async Task<XElement> UpdateAnonymousSession([FromBody] XElement request)
        {
            // Выполнить необходимые операции и передать ответ хабу. 
            // Долгие операции выполнять в отдельном потоке
            // Статус код отправителю вернуть через ответ на запрос немедленно после передачи запроса дальше по цепочке или начала обработки данным сервисом

            // Параметры запроса
            string sessionKey = request.Element("sessionKey").Value;
            long requestId = long.Parse(request.Element("requestId").Value);

            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId, ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        // Соответствует вызову
        // UserInfo[] rows = db.Database.SqlQuery<UserInfo>("SELECT * FROM dbo.UserInfo w WHERE UPPER(w.EMail)=UPPER(@email)", new SqlParameter("@email", request.reg_Email)).ToArray();
        // return rows;
        // Ожидаемую структуру ответа см. в методе zapad.Public.WebInterface.Models.Tools.ApiHelpers.ExtractUserArray()
        [HttpGet]
        public async Task<XElement> GetUsersByEmail(string email, string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId, 
                ReturnCodes.BuildRcAnswer(0, "Успешно", UserInfo.ArrayToXElement(users.Where(u => u.EMail == email).ToArray())));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        // Соответствует операциям
        //    UserInfo[] rows = db.Database.SqlQuery<UserInfo>("SELECT * FROM dbo.UserInfo w WHERE w.UserId=@UserId", new SqlParameter("@UserId", id)).ToArray();
        //    return rows;
        // Ожидаемая структура ответа совпадает с GetUsersByEmail()
        [HttpGet]
        public async Task<XElement> GetUsersById(int id, string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId,
                ReturnCodes.BuildRcAnswer(0, "Успешно", UserInfo.ArrayToXElement(users.Where(u => u.UserId == id).ToArray())));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        // Соответствует
        // db.UserInfo.Add(user);
        // db.savechanges();
        // return user;
        // Структуру запроса см. в zapad.Public.WebInterface.Models.ServiceInteraction.WebHostCacheWrapper.AddUser()
        // Важно при возврате установить правильный UserId
        // Структуру данных пользователя в запросе и ответе см. в методе zapad.Public.WebInterface.Models.Authorization.UserInfo.FromXElement()
        [HttpPost]
        public async Task<XElement> AddUser([FromBody] XElement request)
        {
            // Параметры запроса
            string sessionKey = request.Element("sessionKey").Value;
            long requestId = long.Parse(request.Element("requestId").Value);
            UserInfo user = UserInfo.FromXElement(request.Element("UserInfo"));

            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            users.Add(user);

            hubProxy.Invoke("OperationCallback", sessionKey, requestId, 
                ReturnCodes.BuildRcAnswer(0, "Успешно", UserInfo.ToXElement(user)));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        // Сответствует операциям
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
        // Структуру запроса см. в zapad.Public.WebInterface.Models.ServiceInteraction.WebHostCacheWrapper.ActivateUserEmail()
        // Ожидаемый ответ - обычный RC
        [HttpPost]
        public async Task<XElement> activate_email([FromBody] XElement request)
        {
            // Параметры запроса
            string sessionKey = request.Element("sessionKey").Value;
            long requestId = long.Parse(request.Element("requestId").Value);
            UserInfo user = UserInfo.FromXElement(request.Element("UserInfo"));

            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");

            var lu = users.Where(u => u.UserId == user.UserId).FirstOrDefault();
            lu.IsActivatedEMail = true;

            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId, ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        // Соответствует 
        //XElement xanswer = WebApiZone.Current.GetResponse<XElement>("/anonymous/activate_phone/",
        //    new XElement("request",
        //        new XElement("id", session.User.UserId),
        //        new XElement("pwd", smspwd),
        //        new XElement("whguid", session.Key.ToString())),
        //    WebApiZone.ContentTypes.xml);
        //if (int.Parse(xanswer.Element("rc").Value) == 0)
        //    db.Database.ExecuteSqlCommand("UPDATE dbo.UserInfo SET IsActivatedPhone=1, LastActivity=GETDATE() WHERE UserId=@Id", new SqlParameter("@Id", session.User.UserId));
        //return xanswer
        // Структуру запроса см. в zapad.Public.WebInterface.Models.ServiceInteraction.WebHostCacheWrapper.ActivateUserPhone()
        // Ожидаемый ответ - обычный RC
        [HttpPost]
        public async Task<XElement> activate_phone([FromBody] XElement request)
        {
            // Параметры запроса
            string sessionKey = request.Element("sessionKey").Value;
            long requestId = long.Parse(request.Element("requestId").Value);
            UserInfo user = UserInfo.FromXElement(request.Element("UserInfo"));
            string smsPassword = request.Element("smsPassword").Value;

            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");

            var lu = users.Where(u => u.UserId == user.UserId).FirstOrDefault();
            lu.IsActivatedPhone = true;

            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId, ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        // Соответствует 
        // return WebApiZone.Current.GetResponse<XElement>("/anonymous/take_pwd/id=" + session.User.UserId.ToString() + "&whguid=" + session.Key.ToString());
        // Ожидаемый ответ - обычный RC + отправленная пользователю СМСка 
        [HttpGet]
        public async Task<XElement> take_pwd(int userId, string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId, ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        // Соответствует 
        // db.Database.ExecuteSqlCommand("UPDATE dbo.UserInfo SET LastActivity=GETDATE() WHERE UserId=@UserId", new SqlParameter("@UserId", session.User.UserId));
        // Структуру запроса см. zapad.Public.WebInterface.Models.ServiceInteraction.WebHostCacheWrapper.UpdateUserLastActivity()
        // Ожидаемый ответ - обычный RC
        [HttpPost]
        public async Task<XElement> UpdateUserLastActivity([FromBody] XElement request)
        {
            // Параметры запроса
            string sessionKey = request.Element("sessionKey").Value;
            long requestId = long.Parse(request.Element("requestId").Value);
            int userId = int.Parse(request.Element("userId").Value);

            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId, ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        // Соответствует вызову 
        //return WebApiZone.Current.GetResponse<XElement>("/anonymous/login/whguid=" + session.Key.ToString() + "&pwd=" + sms);
        // Ожидаемый ответ - обычный RC
        [HttpGet]
        public async Task<XElement> login(string sessionKey, string smsPassword, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId, ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        // Соответствует
        //db.Database.ExecuteSqlCommand("UPDATE dbo.UserInfo SET LastActivity=GETDATE(), IsAcceptAdmin=1 WHERE UserId=@UserId", new SqlParameter("@UserId", session.User.UserId));
        // Структуру запроса см. в zapad.Public.WebInterface.Models.ServiceInteraction.WebHostCacheWrapper.UpdateUserAcceptAdmin()
        // Ожидаемый ответ - обычный RC
        [HttpPost]
        public async Task<XElement> UpdateUserAcceptAdmin([FromBody] XElement request)
        {
            // Параметры запроса
            string sessionKey = request.Element("sessionKey").Value;
            long requestId = long.Parse(request.Element("requestId").Value);
            int userId = int.Parse(request.Element("userId").Value);

            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");

            var lu = users.Where(u => u.UserId == userId).Single();
            lu.IsAcceptAdmin = true;

            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId, ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        // Соответствует вызову 
        //return WebApiZone.Current.GetResponse<XElement>("/anonymous/lostpwd/id=" + session.User.UserId + "&whguid=" + session.Key.ToString());
        // Ожидаемый ответ - обычный RC
        [HttpGet]
        public async Task<XElement> lostpwd(int userId, string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId, ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        // Соответствует 
        //    XElement xanswer = WebApiZone.Current.GetResponse<XElement>("/mainarea/logout/" + this.session.Key.ToString());
        // Ожидаемый ответ - обычный RC
        [HttpGet]
        public async Task<XElement> logout(string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId, ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        // Проверяет, аутентифицирован ли пользователь
        // Ожидаемый ответ - обычный RC
        [HttpGet]
        public async Task<XElement> IsAuthentificated(string sessionKey)
        {
            #region Заглушка
            // SignalR не используем, т.к. запрос идет только до WebHostCache
            return ReturnCodes.BuildRcAnswer(0, "Аутентифицирован"); // Не-ноль - иначе
            #endregion
        }
    }
}
