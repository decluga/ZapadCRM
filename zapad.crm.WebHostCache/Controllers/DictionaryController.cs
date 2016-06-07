using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Xml.Linq;
using zapad.crm.WebHostCache.Filters;
using zapad.crm.WebHostCache.Models.Tools;
using zapad.Model.API;
using zapad.Public.WebInterface.Models.Authorization;

namespace zapad.crm.WebHostCache.Controllers
{
    [AuthorizationFilter]
    public class DictionaryController : ApiController
    {
        #region Справочник "Периоды звоноков"
        #region /GetEventPeriods: Получение справочника периодов события
        /// <summary>
        /// Сущность периода:
        /// Id, Name, Timespan
        /// </summary>
        /// <returns></returns>
        [Route("GetEventPeriods"), HttpGet, PageID(201)]
        public async Task<XElement> GetEventPeriods(string sessionKey, long requestId)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"api\Dictionary\GetEventPeriods?sessionKey=" + sessionKey + "&requestId=" + requestId);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }
        #endregion
        #endregion

        #region Справочник "Статусы события"
        #region /GetEventStatuses: Получение справочника статусов
        /// <summary>
        /// Сущность статусов:
        /// Id, Name
        /// </summary>
        /// <returns></returns>
        [Route("GetEventStatuses"), HttpGet, PageID(202)]
        public async Task<XElement> GetEventStatuses(string sessionKey, long requestId)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"api\Dictionary\GetEventStatuses?sessionKey=" + sessionKey + "&requestId=" + requestId);
            return ReturnCodes.BuildRcAnswer(0, "OK");
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
        [Route("GetEventResultTypes"), HttpGet, PageID(203)]
        public async Task<XElement> GetEventResultTypes(string sessionKey, long requestId)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"api\Dictionary\GetEventResultTypes?sessionKey=" + sessionKey + "&requestId=" + requestId);
            return ReturnCodes.BuildRcAnswer(0, "OK");
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
            Program.WebApiSyncRequestBuffer.AddRequest(@"api\Dictionary\GetDepartments?sessionKey=" + sessionKey + "&requestId=" + requestId);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }
        #endregion
        #endregion

        #region Справочник "Сотрудники"
        #region /GetPeoples: Получение списка пользователей для фильтра "Кто принял"
        [Route("GetPeoples"), HttpGet, PageID(103)]
        public async Task<XElement> GetPeoples(string sessionKey, long requestId)
        {
            Program.WebApiSyncRequestBuffer.AddRequest(@"api\Dictionary\GetPeoples?sessionKey=" + sessionKey + "&requestId=" + requestId);
            return ReturnCodes.BuildRcAnswer(0, "OK");
        }
        #endregion
        #endregion
    }
}
