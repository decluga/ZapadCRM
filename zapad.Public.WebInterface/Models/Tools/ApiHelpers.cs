using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using zapad.Public.WebInterface.Models.Authorization;

namespace zapad.Public.WebInterface.Models.Tools
{
    /// <summary>
    /// Класс хелперов для работы с API
    /// </summary>
    static public class ApiHelpers
    {
        /// <summary>
        /// Строит запрос к API
        /// </summary>
        /// <param name="sessionkey">Ключ текущей сессии</param>
        /// <param name="contents">Контент запроса</param>
        /// <returns>Построенный запрос</returns>
        static public XElement BuildRequest(string sessionkey, params XElement[] contents)
        {
            var xml = new XElement("request");
            xml.Add(new XElement("sessionKey", sessionkey));
            if (contents != null && contents.Length > 0)
                foreach (var c in contents) xml.Add(c);
            return xml;
        }

        /// <summary>
        /// Извлекает массив UserInfo из ответа сервера
        /// </summary>
        /// <param name="result">Ответ сервера</param>
        /// <returns>Массив данных о пользователях</returns>
        static public UserInfo[] ExtractUserArray(XElement result)
        {
            var userNodes = result.Element("Users").Elements("UserInfo");
            var users = new List<UserInfo>(userNodes.Count());
            foreach (var xml in userNodes)
            {
                var u = new UserInfo();
                u.FromXElement(xml);
                users.Add(u);
            }

            return users.ToArray();
        }
    }
}