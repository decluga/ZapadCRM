using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zapad.crm.WebApiSync.Helpers
{
    static class Constants
    {
        public static readonly string RequestIdKey = "requestId"; //ключ по которому в request хранится его GUID
        public static readonly string DateFormat = "yyyy-MM-dd HH:mm:ss.fff"; //DateTime формат для логов 
        public static readonly string SessionIdKeyInCookies = "zapadSessionId"; //ключ по которому в куках пользователя хранится guid сессии
    }
}
