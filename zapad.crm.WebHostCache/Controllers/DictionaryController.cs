using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;

namespace zapad.crm.WebHostCache.Controllers
{
    public class DictionaryController : ApiController
    {
        #region Справочник "Периоды звоноков"
        #region /GetCallPeriods: Получение справочника периодов звонков
        /// <summary>
        /// Сущность периода звонков:
        /// Id, Name, Timespan
        /// </summary>
        /// <returns></returns>
        [Route("GetCallPeriods"), HttpGet]
        public async Task<XElement> GetCallPeriods()
        {
            throw new NotImplementedException();
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
        [Route("GetCallStatuses"), HttpGet]
        public async Task<XElement> GetCallStatuses()
        {
            throw new NotImplementedException();
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
        [Route("GetCallResultTypes"), HttpGet]
        public async Task<XElement> GetCallResultTypes()
        {
            throw new NotImplementedException();
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
        [Route("GetDepartments"), HttpGet]
        public async Task<XElement> GetDepartments()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
