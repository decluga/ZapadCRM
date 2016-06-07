using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Xml.Linq;
using zapad.crm.WebHostCache.Models.Authentification;
using zapad.crm.WebHostCache.Models.Tools;
using zapad.Model.Security;
using zapad.Public.WebInterface.Models.Authorization;

namespace zapad.crm.WebHostCache.Filters
{
    /// <summary>
    /// Фильтр авторизации для контроллеров
    /// </summary>
    class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Разрешено ли применять фильтр несколько раз
        /// </summary>
        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }

        private const string GUID_KEY = "sessionKey";

        /// <summary>
        /// Проверяется 
        /// </summary>
        /// <param name="actionContext">Контекст запроса</param>
        /// <param name="cancellationToken">Токен для прерывания</param>
        /// <param name="continuation">Метод, вызываемый при успешной авторизации</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            Guid sessionKey = GetSessionKey(actionContext);

            if (!Authenticate.registerGuid(sessionKey) || !CheckPageAccess(actionContext, sessionKey))
            {
                XElement answer = new XElement("answer", new XElement("rc", 7), new XElement("msg", "Доступ запрещен"));
                return Task.FromResult<HttpResponseMessage>(actionContext.Request.CreateResponse(answer));
            }

            return continuation();
        }

        /// <summary>
        /// Получить ключ сессии из запроса
        /// </summary>
        /// <param name="actionContext">Контекст запроса</param>
        /// <returns>Ключ сессии в формате GUID</returns>
        private Guid GetSessionKey(HttpActionContext actionContext)
        {
            Guid sessionKey = Guid.Empty;

            if (actionContext.Request.Method == HttpMethod.Get)
            {
                sessionKey = Guid.Parse(actionContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value)[GUID_KEY]);
            }
            else if (actionContext.Request.Method == HttpMethod.Post)
            {
                string requestBody = actionContext.Request.Content.ReadAsStringAsync().Result;
                XElement req = XElement.Parse(requestBody);
                sessionKey = Guid.Parse(req.Element(GUID_KEY).Value);
            }

            return sessionKey;
        }

        /// <summary>
        /// Проверяет права доступа к запрашиваемому действию
        /// </summary>
        /// <param name="actionContext">Контекст запроса</param>
        /// <param name="sessionKey">Ключ сессии</param>
        /// <returns>true - доступ разрешен, false - иначе</returns>
        private bool CheckPageAccess(HttpActionContext actionContext, Guid sessionKey)
        {
            var actionName = actionContext.ActionDescriptor.ActionName;

            var methodInfo = actionContext.ControllerContext.Controller.GetType().GetMethod(actionName);
            var attr = Attribute.GetCustomAttribute(methodInfo, typeof(PageIDAttribute)) as PageIDAttribute;

            long pageId = -1;
            if (attr != null)
                pageId = attr.PageID;
            var resp = WebApiSync.Current.GetResponse<XElement>(@"api/Access/PageTransfer?pageId=" + pageId + "&sessionKey=" + sessionKey);

            var access = ObjectAccessResult.FromXElement(resp.Element("ObjectAccessResult"));

            return access.Access.Read;
        }
    }
}
