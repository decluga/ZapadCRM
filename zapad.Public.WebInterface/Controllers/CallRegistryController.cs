using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using zapad.Public.WebInterface.Models.Tools;
using System.Xml.Linq;
using Newtonsoft.Json;
using zapad.Public.WebInterface.Models.ViewModels;
using zapad.Public.WebInterface.Models.Authorization;

namespace zapad.Public.WebInterface.Controllers
{
    //v.ryabov TODO: наследовать BaseController
    public class CallRegistryController : BaseController
    {
        // GET: CallRegistry
        [PageID(101)]
        public ActionResult Index()
        {
            return View();
        }

        [PageID(104)]
        public ActionResult CallPage()
        {
            ViewBag.phone = "+7-555-555-55-55";
            ViewBag.FIO = "Иванов Иван Иванович";
            ViewBag.Marketing = new List<Marketing>
            {
                new Marketing
                {
                    element = new List<string> { "TV", "Радио", "Пресса", "Акции" },
                    title = "СМИ"
                },
                new Marketing
                {
                    element = new List<string> {"Буклеты, листовки", "Билборды", "Вывески офиса", "Лифт" },
                    title = "Реклама"
                },
                new Marketing
                {
                    element = new List<string> {"Официальный сайт", "Avito", "Другие сайты", "Социальные сети" },
                    title = "Интернет"
                },
                new Marketing
                {
                    element = new List<string> {"Риэлтер", "Знакомые", "Стройка", "Предприятие" },
                    title = "Контакты"
                }
            };
            ViewBag.Advertising = new List<Marketing>
            {
                new Marketing
                {
                    element = new List<string> {"Реклама1", "Реклама2", "Реклама3", "Реклама4", "Реклама5" },
                    title = "Городской уровень"
                },
                new Marketing
                {
                    element = new List<string> {"Реклама1", "Реклама2", "Реклама3", "Реклама4", "Реклама5" },
                    title = "Федеральны уровень"
                },
                new Marketing
                {
                    element = new List<string> {"Реклама1", "Реклама2", "Реклама3", "Реклама4", "Реклама5", "Реклама6" },
                    title = "Площадка"
                }
            };
            return View();
        }

        #region /GetAll: Получение списка звонков для реестра
        /// <param name="page">Текущая страница</param>
        /// <param name="pageSize">Количество записей на странице</param>
        /// <param name="filter">Фильтр</param>
        /// <param name="searchTerm">Поиск</param>
        [Route("GetAll"), HttpPost, PageID(102)]
        public JsonResult GetAll(long? page, long? pageSize, KendoFilter filter = null, string searchTerm = "")
        {
            var requestParameters = new XElement("Request",
                                                    new XElement("page", page.HasValue ? page.Value : 0),
                                                    new XElement("pageSize", pageSize.HasValue ? pageSize.Value : 0),
                                                    new XElement("searchTerm", searchTerm),
                                                    new XElement("sessionKey", session.Key.ToString())
                                                    );
            FilterHelper.AddRequestFilters(ref requestParameters, filter);
            var response = WebHostCache.Current.GetResponse<XElement>(@"api\CallRegistry\GetAll", requestParameters, WebHostCache.ContentTypes.xml);
            var result = response.Elements("Call").Select(x => new CallDTO()
            {
                Id = new Guid(x.Element("Id").getValue("")),
                Phone = x.Element("Phone").getValue(""),
                Client = x.Element("Client").Element("Name").getValue(""),
                DateTime = x.Element("DateTime").getValue(DateTime.MinValue).ToString("dd.MM.yyyy hh:mm"),
                CallReceiver = x.Element("CallReceiver").Element("Name").getValue(""),
                IsRepeatCall = x.Element("IsRepeatCall").getValue(true) ? "Да" : "Нет",
                Status = x.Element("Status").Element("Name").getValue(""),
                CallResultType = x.Element("CallResultType").Element("Name").getValue(""),
                DepartmentUserDepartment = x.Element("DepartmentUser").Element("Department").Element("Name").getValue(""),
                DepartmentUser = x.Element("DepartmentUser").Element("Name").getValue("")
            }).ToList();
            return Json(new { rc = 0, Items = result, /*Total = response.Element("Total").getValue(Int64.MinValue)*/ }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region /GetCallRegistryPeoples: Получение списка пользователей для фильтра "Кто принял"
        [Route("GetAll"), HttpGet, PageID(103)]
        public JsonResult GetCallRegistryPeoples()
        {
            var response = WebHostCache.Current.GetResponse<XElement>(@"api\CallRegistry\GetCallRegistryPeoples?sessionKey=" + session.Key.ToString());
            var result = response.Elements("People").Select(x => new DictionaryDTO<Guid>()
            {
                Id = new Guid(x.Element("Id").getValue("")),
                Name = x.Element("Name").getValue("")
            }).ToList();
            return Json(new { rc = 0, Items = result}, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}