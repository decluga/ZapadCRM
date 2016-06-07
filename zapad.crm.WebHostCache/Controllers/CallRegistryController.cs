using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using zapad.crm.WebHostCache.Filters;
using zapad.crm.WebHostCache.Models.Tools;
using zapad.Model.API;
using zapad.Public.WebInterface.Models.Authorization;

namespace zapad.crm.WebHostCache.Controllers
{
    [AuthorizationFilter]
    public class CallRegistryController : ApiController
    {
        #region Расширенные методы для реестра звонков
        #region /GetCalls: Получение списка звонков для реестра
        /// <param name="page">Текущая страница</param>
        /// <param name="count">Количество записей на странице</param>
        /// <param name="filter">Фильтр</param>
        /// <param name="searchTerm">Поиск</param>
        [Route("GetCalls"), HttpPost, PageID(102)]
        public async Task<XElement> GetCalls([FromBody]XElement requestParameters)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"api\CallRegistry\GetCalls", requestParameters, WebApiSync.ContentTypes.xml);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }
        #endregion
        #endregion

        #region /Get: Получение данных по идентификатору звонка
        /// <param name="req">XML с идентификатором звонка CallId</param>
        [Route("Get"), HttpGet]
        public async Task<XElement> Get(Guid callid, string sessionKey, long requestId)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"api\CallRegistry\Get?callid=" + callid + "&sessionKey=" + sessionKey + "&requestId=" + requestId);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }
        #endregion

        #region CRUD-методы для реестра звонков
        #region /Create: Создание звонка
        /// <param name="req">XML c заполненными данными по звонку</param>
        [Route("Create"), HttpPost]
        public async Task<XElement> Create([FromBody] XElement req)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"api\CallRegistry\Create", req);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }
        #endregion

        #region /Update: Изменение данных по идентификатору звонка
        /// <param name="req">XML с измененными полями и идентификатором звонка CallId</param>
        [Route("Update"), HttpPost]
        public async Task<XElement> Update([FromBody] XElement req)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"api\CallRegistry\Update", req);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }
        #endregion
        #endregion
    }
}
