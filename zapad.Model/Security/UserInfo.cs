using System;
using System.Collections.Generic;
using System.Xml.Linq;
using zapad.Model.Tools;

namespace zapad.Model.Security
{
    /// <summary>
    /// Модель профиля пользователя
    /// </summary>
    public partial class UserInfo
    {
        public int UserId { get; set; }
        public string F { get; set; }
        public string I { get; set; }
        public string O { get; set; }
        public bool IsActivatedEMail { get; set; }
        public bool IsActivatedPhone { get; set; }
        public bool IsAcceptAdmin { get; set; }
        public bool IsDisabled { get; set; }
        public string DisabledCause { get; set; }
        public string EMail { get; set; }
        public string Phone { get; set; }
        public System.DateTime LastActivity { get; set; }
        public byte[] SmsCodeHash { get; set; }
        public string ADName { get; set; }
        public string XmlInfo { get; set; }
        public string EMailActivateGuid { get; set; }

        /// <summary>
        /// Преобразует экземпляр UserInfo в XElement для передачи в запросах
        /// </summary>
        /// <param name="obj">Экземпляр UserInfo</param>
        /// <returns>XElement с данными пользователя</returns>
        static public XElement ToXElement(UserInfo obj)
        {
            var xml = new XElement("UserInfo");
            xml.Add(new XElement("UserId", obj.UserId));
            xml.Add(new XElement("F", obj.F));
            xml.Add(new XElement("I", obj.I));
            xml.Add(new XElement("O", obj.O));
            xml.Add(new XElement("IsActivatedEMail", obj.IsActivatedEMail));
            xml.Add(new XElement("IsActivatedPhone", obj.IsActivatedPhone));
            xml.Add(new XElement("IsAcceptAdmin", obj.IsAcceptAdmin));
            xml.Add(new XElement("IsDisabled", obj.IsDisabled));
            xml.Add(new XElement("DisabledCause", obj.DisabledCause));
            xml.Add(new XElement("EMail", obj.EMail));
            xml.Add(new XElement("Phone", obj.Phone));
            xml.Add(new XElement("LastActivity", obj.LastActivity));
            xml.Add(new XElement("SmsCodeHash", obj.SmsCodeHash));
            xml.Add(new XElement("ADName", obj.ADName));
            xml.Add(new XElement("XmlInfo", obj.XmlInfo));
            xml.Add(new XElement("EMailActivateGuid", obj.EMailActivateGuid));
            return xml;
        }

        /// <summary>
        /// Восстанавливает объект UserInfo из XML
        /// </summary>
        /// <param name="xml">XML с данными</param>
        /// <returns>Восстановленный объект UserInfo</returns>
        static public UserInfo FromXElement(XElement xml)
        {
            UserInfo obj = new UserInfo();

            obj.UserId = xml.Element("UserId").getValue(0);
            obj.F = xml.Element("F").getValue(string.Empty);
            obj.I = xml.Element("I").getValue(string.Empty);
            obj.O = xml.Element("O").getValue(string.Empty);
            obj.IsActivatedEMail = xml.Element("IsActivatedEMail").getValue(false);
            obj.IsActivatedPhone = xml.Element("IsActivatedPhone").getValue(false);
            obj.IsAcceptAdmin = xml.Element("IsAcceptAdmin").getValue(false);
            obj.IsDisabled = xml.Element("IsDisabled").getValue(false);
            obj.DisabledCause = xml.Element("IsDisabled").getValue(string.Empty);
            obj.EMail = xml.Element("EMail").getValue(string.Empty);
            obj.Phone = xml.Element("Phone").getValue(string.Empty);
            obj.LastActivity = xml.Element("LastActivity").getValue(DateTime.MinValue);
            obj.SmsCodeHash = xml.Element("LastActivity").getValue(new byte[32]);
            obj.ADName = xml.Element("ADName").getValue(string.Empty);
            obj.XmlInfo = xml.Element("XmlInfo").getValue(string.Empty);
            obj.EMailActivateGuid = xml.Element("EMailActivateGuid").getValue(string.Empty);

            return obj;
        }

        /// <summary>
        /// Преобразует массив экземпляров UserInfo в XElement
        /// </summary>
        /// <param name="users">Исходный массив экземпляров UserInfo</param>
        /// <returns>Результирующий XElement</returns>
        static public XElement ArrayToXElement(UserInfo[] users)
        {
            XElement xml = new XElement("Users");

            foreach (var u in users)
                xml.Add(UserInfo.ToXElement(u));

            return xml;
        }

        /// <summary>
        /// Преобразует XElement в массив экземпляров UserInfo
        /// </summary>
        /// <param name="xml">Исходный XElement</param>
        /// <returns>Результирующий массив экземпляров UserInfo</returns>
        static public UserInfo[] ArrayFromXElement(XElement xml)
        {
            List<UserInfo> lst = new List<UserInfo>();

            foreach (var e in xml.Elements("UserInfo"))
                lst.Add(UserInfo.FromXElement(e));

            return lst.ToArray();
        }
    }
}