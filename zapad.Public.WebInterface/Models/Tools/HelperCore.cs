using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Script.Serialization;

namespace zapad.Public.WebInterface.Models.Tools
{
    public class HelperCore
    {
        /// <summary>
        /// Получить JSS serializer
        /// </summary>
        /// <returns></returns>
        public static JavaScriptSerializer getJSS()
        {
            return new JavaScriptSerializer();
        }

        /// <summary>
        /// Конвертировать обобщенный список в JSON 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object">Обобщенный список</param>
        /// <returns></returns>
        public static string toJSON<T>(List<T> Object)
        {
            JavaScriptSerializer JSS = getJSS();
            return JSS.Serialize(Object);
        }

        /// <summary>
        /// Получить рандомное число
        /// </summary>
        /// <returns></returns>
        public static string getRandom()
        {
            return new Random().NextDouble().ToString();
        }

        /// <summary>
        /// Получить имя текущего controller
        /// </summary>
        /// <returns></returns>
        public static string getCurrentControllerName()
        {
            System.Web.Routing.RouteValueDictionary routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("controller"))
            {
                return (string)routeValues["controller"];
            }

            return string.Empty;
        }

        /// <summary>
        /// Получить имя текущего action
        /// </summary>
        /// <returns></returns>
        public static string getCurrentActionName()
        {
            System.Web.Routing.RouteValueDictionary routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("action"))
            {
                return (string)routeValues["action"];
            }

            return string.Empty;
        }

        /// <summary>
        /// Определить, является ли текущая страница главной
        /// </summary>
        /// <returns></returns>
        public static bool isMainPage()
        {
            string currentControllerName = getCurrentControllerName();
            string currentActionName = getCurrentActionName();

            return (currentControllerName == "Main" && currentActionName == "Index") ? true : false;
        }

        /// <summary>
        /// Получить форматированное значение цены
        /// </summary>
        /// <param name="price">Цена</param>
        /// <returns></returns>
        public static string getFormatPrice(decimal price)
        {
            return price.ToString("N2", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Получить форматированное значение строки
        /// </summary>
        /// <param name="str">Строка</param>
        /// <param name="defaultValue">Значение строки по умолчанию</param>
        /// <returns></returns>
        public static string getFormatString(string str, string defaultValue = "-")
        {
            string result = defaultValue;

            if (!String.IsNullOrEmpty(str))
            {
                result = str;
            }

            return result;
        }

        /// <summary>
        /// Получить форматированное значение даты
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="format">Формат даты</param>
        /// <returns></returns>
        public static string getFormatDate(DateTime? date, string format = "dd.MM.yyyy H:mm")
        {
            string result = "";

            if (date.HasValue)
            {
                result = date.Value.ToString(format);
            }

            return result;
        }

        /// <summary>
        /// Получить форматированное значение даты
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="format">Формат даты</param>
        /// <returns></returns>
        public static string getFormatDate(DateTime date, string format = "dd.MM.yyyy H:mm")
        {
            return date.ToString(format);
        }
    }
}