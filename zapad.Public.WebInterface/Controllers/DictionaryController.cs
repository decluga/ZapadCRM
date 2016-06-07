using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using zapad.Model.DTO.Dictionaries;
using zapad.Model.Tools;
using zapad.Public.WebInterface.Models.Authorization;
using zapad.Public.WebInterface.Models.Tools;

namespace zapad.Public.WebInterface.Controllers
{
    public class DictionaryController : BaseController
    {
        #region Справочник "Периоды событий"
        #region /GetEventPeriods: Получение справочника периодов события
        /// <summary>
        /// Сущность периода:
        /// Id, Name, Timespan
        /// </summary>
        /// <returns></returns>
        [Route("GetEventPeriods"), HttpGet, PageID(201)]
        public JsonResult GetEventPeriods()
        {
            var response = WebHostCache.Current.GetResponse<XElement>(@"api\Dictionary\GetEventPeriods?sessionKey=" + session.Key.ToString());
            var result = EventPeriodDTO.ArrayFromXElement(response).ToList();
            return Json(new { rc = 0, Items = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Справочник "Статусы событий"
        #region /GetEventStatuses: Получение справочника статусов события
        /// <summary>
        /// Сущность статусов события:
        /// Id, Name
        /// </summary>
        /// <returns></returns>
        [Route("GetEventStatuses"), HttpGet, PageID(202)]
        public JsonResult GetEventStatuses()
        {
            var response = WebHostCache.Current.GetResponse<XElement>(@"api\Dictionary\GetEventStatuses?sessionKey=" + session.Key.ToString());
            var result = EventStatusDTO.ArrayFromXElement(response).ToList();
            return Json(new { rc = 0, Items = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Справочник "Тип результата события"
        #region /GetEventResultTypes: Получение справочника типов результата события
        /// <summary>
        /// Сущность типа результатов:
        /// Id, Name
        /// </summary>
        /// <returns></returns>
        [Route("GetEventResultTypes"), HttpGet, PageID(203)]
        public JsonResult GetEventResultTypes()
        {
            var response = WebHostCache.Current.GetResponse<XElement>(@"api\Dictionary\GetEventResultTypes?sessionKey=" + session.Key.ToString());
            var result = EventResultTypeDTO.ArrayFromXElement(response).ToList();
            return Json(new { rc = 0, Items = result }, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetDepartments()
        {
            var response = WebHostCache.Current.GetResponse<XElement>(@"api\Dictionary\GetDepartments?sessionKey=" + session.Key.ToString());
            var result = DepartmentDTO.ArrayFromXElement(response).ToList();
            return Json(new { rc = 0, Items = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Справочник "Сотрудники"
        #region /GetPeoples: Получение списка пользователей
        [Route("GetPeoples"), HttpGet, PageID(103)]
        public JsonResult GetPeoples()
        {
            var response = WebHostCache.Current.GetResponse<XElement>(@"api\Dictionary\GetPeoples?sessionKey=" + session.Key.ToString());
            var result = PeopleDTO.ArrayFromXElement(response).ToList();
            return Json(new { rc = 0, Items = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
    }
}