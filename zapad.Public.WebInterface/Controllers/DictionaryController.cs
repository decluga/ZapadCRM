using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using zapad.Public.WebInterface.Models.Authorization;
using zapad.Public.WebInterface.Models.Tools;
using zapad.Public.WebInterface.Models.ViewModels;

namespace zapad.Public.WebInterface.Controllers
{
    public class DictionaryController : BaseController
    {
        #region Справочник "Периоды звоноков"
        #region /GetCallPeriods: Получение справочника периодов звонков
        /// <summary>
        /// Сущность периода звонков:
        /// Id, Name, Timespan
        /// </summary>
        /// <returns></returns>
        [Route("GetCallPeriods"), HttpGet, PageID(201)]
        public JsonResult GetCallPeriods()
        {
            var response = WebHostCache.Current.GetResponse<XElement>(@"api\Dictionary\GetCallPeriods&sessionKey=" + session.Key.ToString());
            var result = response.Elements("CallPeriod").Select(x => new DictionaryDTO<Int32>()
            {
                Id = x.Element("Id").getValue(0),
                Name = x.Element("Name").getValue("")
            }).ToList();
            return Json(new { rc = 0, Items = result }, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetCallStatuses()
        {
            var response = WebHostCache.Current.GetResponse<XElement>(@"api\Dictionary\GetCallStatuses?sessionKey=" + session.Key.ToString());
            var result = response.Elements("CallStatus").Select(x => new DictionaryDTO<Int32>()
            {
                Id = x.Element("Id").getValue(0),
                Name = x.Element("Name").getValue("")
            }).ToList();
            return Json(new { rc = 0, Items = result }, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetCallResultTypes()
        {
            var response = WebHostCache.Current.GetResponse<XElement>(@"api\Dictionary\GetCallResultTypes?sessionKey=" + session.Key.ToString());
            var result = response.Elements("CallResultType").Select(x => new DictionaryDTO<Int32>()
            {
                Id = x.Element("Id").getValue(0),
                Name = x.Element("Name").getValue("")
            }).ToList();
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
            var result = response.Elements("Department").Select(x => new DictionaryDTO<Guid>()
            {
                Id = new Guid(x.Element("Id").getValue("")),
                Name = x.Element("Name").getValue("")
            }).ToList();
            return Json(new { rc = 0, Items = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
    }
}