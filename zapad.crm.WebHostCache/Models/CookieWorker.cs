using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using zapad.crm.WebHostCache.Helpers;

namespace zapad.crm.WebHostCache.Models
{
    public static class CookieWorker
    {
        public static CookieHeaderValue GetSessionGuidFromCookies(HttpRequestHeaders requestHeaders)
        {
            return requestHeaders.GetCookies(Constants.SessionIdKeyInCookies).FirstOrDefault();
        }

        public static void AddSessionIdToCookies(Guid guid, HttpResponseMessage response)
        {
            CookieHeaderValue newCookie = new CookieHeaderValue(Constants.SessionIdKeyInCookies, guid.ToString());
            newCookie.Expires = GetTimeOutdateOfCookies();
            response.Headers.AddCookies(new CookieHeaderValue[] { newCookie });
        }

        /// <summary>
        /// Возвращает время, когда истечет срок действия cookies
        /// </summary>
        /// <returns></returns>
        public static DateTime GetTimeOutdateOfCookies()
        {
            var config = SelfHostConfigRetriever.GetHostConfig();
            return DateTime.Now.AddSeconds(Convert.ToInt64(config.GetElementByName("dateOfOutdateCookies").Value));
        }
    }
}
