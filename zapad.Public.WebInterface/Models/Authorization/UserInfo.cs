using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}