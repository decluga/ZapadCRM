using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using zapad.Public.WebInterface.Models.Tools;
using System.Xml.Linq;
using Newtonsoft.Json;
using zapad.Public.WebInterface.Models.Authorization;
using zapad.Model.Tools;
using zapad.Model.Calls.DTO;
using zapad.Model.API.Requests;
using zapad.Model.ViewModel;

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

        #region /GetCalls: Получение списка звонков для реестра
        /// <param name="page">Текущая страница</param>
        /// <param name="pageSize">Количество записей на странице</param>
        /// <param name="filter">Фильтр</param>
        /// <param name="searchTerm">Поиск</param>
        [Route("GetCalls"), HttpPost, PageID(102)]
        public JsonResult GetCalls(long? page, long? pageSize, KendoFilter filter = null, string searchTerm = "")
        {
            var requestParameters = new CallsRequest()
            {
                SessionKey = session.Key,
                Page = page.HasValue ? page.Value : 0,
                PageSize = pageSize.HasValue ? pageSize.Value : 0,
                SearchTerm = searchTerm,
                Filters = filter
            };
            var response = WebHostCache.Current.GetResponse<XElement>(@"api\CallRegistry\GetCalls", CallsRequest.ToXElement(requestParameters), WebHostCache.ContentTypes.xml);
            var result = CallDTO.ArrayFromXElement(response).ViewModelList();
            return Json(new { rc = 0, Items = result, Total = response.Element("Total").getValue(Int32.MinValue) }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}