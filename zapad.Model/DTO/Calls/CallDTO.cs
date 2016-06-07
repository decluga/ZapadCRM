using System;
using System.Collections.Generic;
using System.Xml.Linq;
using zapad.Model.Tools;

namespace zapad.Model.Calls.DTO
{
    public class CallDTO
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Client { get; set; }
        public DateTime DateTime { get; set; }
        public string CallReceiver { get; set; }
        public bool IsRepeatCall { get; set; }
        public string EventStatus { get; set; }
        public string EventResultType { get; set; }
        public string DepartmentUserDepartment { get; set; }
        public string DepartmentUser { get; set; }

        /// <summary>
        /// Преобразует экземпляр CallDTO в XElement для передачи в запросах
        /// </summary>
        /// <returns>XElement с данными</returns>
        public static XElement ToXElement(CallDTO callDTO)
        {
            var xml = new XElement("Call");
            xml.Add(new XElement("Id", callDTO.Id));
            xml.Add(new XElement("Phone", callDTO.Phone));
            xml.Add(new XElement("Client", callDTO.Client));
            xml.Add(new XElement("DateTime", callDTO.DateTime));
            xml.Add(new XElement("CallReceiver", callDTO.CallReceiver));
            xml.Add(new XElement("IsRepeatCall", callDTO.IsRepeatCall));
            xml.Add(new XElement("EventStatus", callDTO.EventStatus));
            xml.Add(new XElement("EventResultType", callDTO.EventResultType));
            xml.Add(new XElement("DepartmentUserDepartment", callDTO.DepartmentUserDepartment));
            xml.Add(new XElement("DepartmentUser", callDTO.DepartmentUser));
            return xml;
        }

        /// <summary>
        /// Заполняет поля объекта данными из XML
        /// </summary>
        /// <param name="xml">XML с данными</param>
        public static CallDTO FromXElement(XElement xml)
        {
            var callDTO = new CallDTO();
            callDTO.Id = xml.Element("Id").getValue(Guid.Empty);
            callDTO.Phone = xml.Element("Phone").getValue(string.Empty);
            callDTO.Client = xml.Element("Client").getValue(string.Empty);
            callDTO.DateTime = xml.Element("DateTime").getValue(System.DateTime.MinValue);
            callDTO.CallReceiver = xml.Element("CallReceiver").getValue(string.Empty);
            callDTO.IsRepeatCall = xml.Element("IsRepeatCall").getValue(true);
            callDTO.EventStatus = xml.Element("EventStatus").getValue(string.Empty);
            callDTO.EventResultType = xml.Element("EventResultType").getValue(string.Empty);
            callDTO.DepartmentUserDepartment = xml.Element("DepartmentUserDepartment").getValue(string.Empty);
            callDTO.DepartmentUser = xml.Element("DepartmentUser").getValue(string.Empty);
            return callDTO;
        }

        /// <summary>
        /// Преобразует массив экземпляров CallDTO в XElement
        /// </summary>
        /// <param name="calls">Исходный массив экземпляров CallDTO</param>
        /// <returns>Результирующий XElement</returns>
        static public XElement ArrayToXElement(CallDTO[] calls)
        {
            XElement xml = new XElement("Calls");

            foreach (var u in calls)
                xml.Add(CallDTO.ToXElement(u));

            return xml;
        }

        /// <summary>
        /// Преобразует XElement в массив экземпляров CallDTO
        /// </summary>
        /// <param name="xml">Исходный XElement</param>
        /// <returns>Результирующий массив экземпляров CallDTO</returns>
        static public CallDTO[] ArrayFromXElement(XElement xml)
        {
            List<CallDTO> lst = new List<CallDTO>();

            foreach (var e in xml.Elements("Call"))
                lst.Add(CallDTO.FromXElement(e));

            return lst.ToArray();
        }
    }
}