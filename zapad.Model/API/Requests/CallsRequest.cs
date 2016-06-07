using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using zapad.Model.Tools;
using zapad.Public.WebInterface.Models.Tools;

namespace zapad.Model.API.Requests
{
    public class CallsRequest
    {
        public Guid SessionKey { get; set; }
        public long Page { get; set; }
        public long PageSize { get; set; }
        public string SearchTerm { get; set; }
        public KendoFilter Filters { get; set; }

        /// <summary>
        /// Преобразует экземпляр CallsRequest в XElement для передачи в запросах
        /// </summary>
        /// <param name="obj">Экземпляр CallsRequest</param>
        /// <returns>XElement с данными CallsRequest</returns>
        static public XElement ToXElement(CallsRequest obj)
        {
            var xml = new XElement("Request");
            xml.Add(new XElement("sessionKey", obj.SessionKey));
            xml.Add(new XElement("page", obj.Page));
            xml.Add(new XElement("pageSize", obj.PageSize));
            xml.Add(new XElement("searchTerm", obj.SearchTerm));
            KendoFilter.AddRequestFilters(ref xml, obj.Filters);
            return xml;
        }

        /// <summary>
        /// Восстанавливает объект CallsRequest из XML
        /// </summary>
        /// <param name="xml">XML с данными</param>
        /// <returns>Восстановленный объект CallsRequest</returns>
        static public CallsRequest FromXElement(XElement xml)
        {
            CallsRequest obj = new CallsRequest();

            obj.SessionKey = xml.Element("sessionKey").getValue(Guid.Empty);
            obj.Page = xml.Element("page").getValue(0);
            obj.PageSize = xml.Element("pageSize").getValue(0);
            obj.SearchTerm = xml.Element("searchTerm").getValue(string.Empty);

            return obj;
        }
    }
}
