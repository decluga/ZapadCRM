using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using zapad.Public.WebInterface.Models.Tools;

namespace zapad.Public.WebInterface.Models.Authorization
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
        /// <param name="userInfo">Экземпляр UserInfo</param>
        /// <returns>XElement с данными пользователя</returns>
        public XElement ToXElement()
        {
            var xml = new XElement("UserInfo");
            xml.Add(new XElement("UserId", this.UserId));
            xml.Add(new XElement("F", this.F));
            xml.Add(new XElement("I", this.I));
            xml.Add(new XElement("O", this.O));
            xml.Add(new XElement("IsActivatedEMail", this.IsActivatedEMail));
            xml.Add(new XElement("IsActivatedPhone", this.IsActivatedPhone));
            xml.Add(new XElement("IsAcceptAdmin", this.IsAcceptAdmin));
            xml.Add(new XElement("IsDisabled", this.IsDisabled));
            xml.Add(new XElement("DisabledCause", this.DisabledCause));
            xml.Add(new XElement("EMail", this.EMail));
            xml.Add(new XElement("Phone", this.Phone));
            xml.Add(new XElement("LastActivity", this.LastActivity));
            xml.Add(new XElement("SmsCodeHash", this.SmsCodeHash));
            xml.Add(new XElement("ADName", this.ADName));
            xml.Add(new XElement("XmlInfo", this.XmlInfo));
            xml.Add(new XElement("EMailActivateGuid", this.EMailActivateGuid));
            return xml;
        }

        /// <summary>
        /// Заполняет поля объекта данными из XML
        /// </summary>
        /// <param name="xml">XML с данными</param>
        public void FromXElement(XElement xml)
        {
            this.UserId = xml.Element("UserId").getValue(0);
            this.F = xml.Element("F").getValue(string.Empty);
            this.I = xml.Element("I").getValue(string.Empty);
            this.O = xml.Element("O").getValue(string.Empty);
            this.IsActivatedEMail = xml.Element("IsActivatedEMail").getValue(false);
            this.IsActivatedPhone = xml.Element("IsActivatedPhone").getValue(false);
            this.IsAcceptAdmin = xml.Element("IsAcceptAdmin").getValue(false);
            this.IsDisabled = xml.Element("IsDisabled").getValue(false);
            this.DisabledCause = xml.Element("IsDisabled").getValue(string.Empty);
            this.EMail = xml.Element("EMail").getValue(string.Empty);
            this.Phone = xml.Element("Phone").getValue(string.Empty);
            this.LastActivity = xml.Element("LastActivity").getValue(DateTime.MinValue);
            this.SmsCodeHash = xml.Element("LastActivity").getValue(new byte[32]);
            this.ADName = xml.Element("ADName").getValue(string.Empty);
            this.XmlInfo = xml.Element("XmlInfo").getValue(string.Empty);
            this.EMailActivateGuid = xml.Element("EMailActivateGuid").getValue(string.Empty);
        }
    }
}