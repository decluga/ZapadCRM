using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using zapad.crm.WebHostCache.Filters;
using zapad.crm.WebHostCache.Models.Tools;
using zapad.Public.WebInterface.Models.Authorization;

namespace zapad.crm.WebHostCache.Controllers
{
    [AuthorizationFilter]
    public class CallRegistryController : ApiController
    {
        #region Расширенные методы для реестра звонков
        #region /GetAll: Получение списка звонков для реестра
        /// <param name="page">Текущая страница</param>
        /// <param name="count">Количество записей на странице</param>
        /// <param name="filter">Фильтр</param>
        /// <param name="searchTerm">Поиск</param>
        [Route("GetAll"), HttpPost, PageID(102)]
        public async Task<XElement> GetAll([FromBody]XElement requestParameters)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"api\CallRegistry\GetAll", requestParameters, WebApiSync.ContentTypes.xml);
        }
        #endregion

        //TODO vryabov: возможно объединить запросы с GetAll?
        #region /GetCallRegistryPeoples: Получение списка пользователей для фильтра "Кто принял"
        [Route("GetCallRegistryPeoples"), HttpGet, PageID(103)]
        public async Task<XElement> GetCallRegistryPeoples(string sessionKey, long requestId)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"api\CallRegistry\GetCallRegistryPeoples?sessionKey=" + sessionKey + "&requestId=" + requestId);
        }
        #endregion
        #endregion

        #region /Get: Получение данных по идентификатору звонка
        /// <param name="req">XML с идентификатором звонка CallId</param>
        [Route("Get"), HttpGet]
        public async Task<XElement> Get(Guid callid, string sessionKey, long requestId)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"api\CallRegistry\Get?callid=" + callid + "&sessionKey=" + sessionKey + "&requestId=" + requestId);
        }
        #endregion

        #region CRUD-методы для реестра звонков
        #region /Create: Создание звонка
        /// <param name="req">XML c заполненными данными по звонку</param>
        [Route("Create"), HttpPost]
        public async Task<XElement> Create([FromBody] XElement req)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"api\CallRegistry\Create", req);
        }
        #endregion

        #region /Update: Изменение данных по идентификатору звонка
        /// <param name="req">XML с измененными полями и идентификатором звонка CallId</param>
        [Route("Update"), HttpPost]
        public async Task<XElement> Update([FromBody] XElement req)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"api\CallRegistry\Update", req);
        }
        #endregion
        #endregion
    }
}
