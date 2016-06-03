using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Xml.Linq;
using zapad.crm.WebHostCache.Filters;
using zapad.crm.WebHostCache.Models.Tools;
using zapad.Public.WebInterface.Models.Authorization;

namespace zapad.crm.WebHostCache.Controllers
{
    [AuthorizationFilter]
    public class DictionaryController : ApiController
    {
        #region Справочник "Периоды звоноков"
        #region /GetCallPeriods: Получение справочника периодов звонков
        /// <summary>
        /// Сущность периода звонков:
        /// Id, Name, Timespan
        /// </summary>
        /// <returns></returns>
        [Route("GetCallPeriods"), HttpGet, PageID(201)]
        public async Task<XElement> GetCallPeriods(string sessionKey, long requestId)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"api\Dictionary\GetCallPeriods?sessionKey=" + sessionKey + "&requestId=" + requestId);
        }
        #endregion
        #endregion

        #region Справочник "Статусы звонка"
        #region /GetCallStatuses: Получение справочника статусов звонка
        /// <summary>
        /// Сущность статусов звонка:
        /// Id, Name
        /// </summary>
        /// <returns></returns>
        [Route("GetCallStatuses"), HttpGet, PageID(202)]
        public async Task<XElement> GetCallStatuses(string sessionKey, long requestId)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"api\Dictionary\GetCallStatuses?sessionKey=" + sessionKey + "&requestId=" + requestId);
        }
        #endregion
        #endregion

        #region Справочник "Тип результата звонка"
        #region /GetCallResultTypes: Получение справочника типов результата звонка
        /// <summary>
        /// Сущность типа результатов звонка:
        /// Id, Name
        /// </summary>
        /// <returns></returns>
        [Route("GetCallResultTypes"), HttpGet, PageID(203)]
        public async Task<XElement> GetCallResultTypes(string sessionKey, long requestId)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"api\Dictionary\GetCallResultTypes?sessionKey=" + sessionKey + "&requestId=" + requestId);
        }
        #endregion
        #endregion

        #region Справочник "Отделы"
        #region /GetDepartments: Получение справочника отделов
        /// <summary>
        /// Сущность отдел:
        /// Id, Name
        /// </summary>
        /// <returns></returns>
        [Route("GetDepartments"), HttpGet, PageID(204)]
        public async Task<XElement> GetDepartments(string sessionKey, long requestId)
        {
            return WebApiSync.Current.GetResponse<XElement>(@"api\Dictionary\GetDepartments?sessionKey=" + sessionKey + "&requestId=" + requestId);
        }
        #endregion
        #endregion
    }
}
