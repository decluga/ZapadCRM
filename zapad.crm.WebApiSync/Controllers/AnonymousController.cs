using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using zapad.crm.WebApiSync.Properties;
using zapad.crm.WebHostCache.Models.DTO;

namespace zapad.crm.WebApiSync.Controllers
{
    public class AnonymousController : ApiController
    {
        static private List<XElement> users = new List<XElement>();

        static AnonymousController()
        {
            users.Add(new XElement("UserInfo",
                new XElement("UserId", 23),
                new XElement("EMail", "1@1.com"),
                new XElement("IsActivatedEMail", true),
                new XElement("IsActivatedPhone", true),
                new XElement("F", "Одинов"),
                new XElement("I", "Раз"),
                new XElement("O", "Разович")));
        }

        [HttpPost]
        public async Task<XElement> UpdateAnonymousSession([FromBody] XElement request)
        {
            // Выполнить необходимые операции и передать ответ хабу. 
            // Долгие операции выполнять в отдельном потоке. 
            // Статус код отправителю вернуть немедленно после передачи запроса дальше по цепочке или начала обработки данным сервисом

            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", request.Element("sessionKey").Value, long.Parse(request.Element("requestId").Value), ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        [HttpGet]
        public async Task<XElement> GetUsersByEmail(string email, string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId, 
                ReturnCodes.BuildRcAnswer(0, "Успешно", 
                    new XElement("Users", users.Where(u => u.Element("EMail").Value == email))));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        [HttpGet]
        public async Task<XElement> GetUsersById(int id, string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId,
                ReturnCodes.BuildRcAnswer(0, "Успешно",
                    new XElement("Users", users.Where(u => u.Element("UserId").Value == id.ToString()))));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        [HttpPost]
        public async Task<XElement> AddUser([FromBody] XElement request)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            users.Add(request.Element("UserInfo"));
            hubProxy.Invoke("OperationCallback", request.Element("sessionKey").Value, long.Parse(request.Element("requestId").Value), ReturnCodes.BuildRcAnswer(0, "Успешно", request.Element("UserInfo")));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        [HttpPost]
        public async Task<XElement> activate_email([FromBody] XElement request)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", request.Element("sessionKey").Value, long.Parse(request.Element("requestId").Value), ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        [HttpPost]
        public async Task<XElement> activate_phone([FromBody] XElement request)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", request.Element("sessionKey").Value, long.Parse(request.Element("requestId").Value), ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

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

        [HttpPost]
        public async Task<XElement> UpdateUserLastActivity([FromBody] XElement request)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", request.Element("sessionKey").Value, long.Parse(request.Element("requestId").Value), ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

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

        [HttpPost]
        public async Task<XElement> UpdateUserAcceptAdmin([FromBody] XElement request)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", request.Element("sessionKey").Value, long.Parse(request.Element("requestId").Value), ReturnCodes.BuildRcAnswer(0, "Успешно"));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

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
