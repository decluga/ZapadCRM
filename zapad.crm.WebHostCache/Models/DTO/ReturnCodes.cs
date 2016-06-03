using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace zapad.crm.WebHostCache.Models.DTO
{
    /// <summary>
    /// Хелпер для построения XML-ответов для API
    /// </summary>
    static public class ReturnCodes
    {
        /// <summary>
        /// Строит ответ с кодом запроса
        /// </summary>
        /// <param name="code">Код результата</param>
        /// <param name="message">Сообщение</param>
        /// <param name="content">Содержимое ответа (опционально)</param>
        /// <returns>Сформированный XML-ответ</returns>
        static public XElement BuildRcAnswer(int code, string message, XElement content = null)
        {
            return new XElement("Answer",
                new XElement("Rc", code),
                new XElement("Msg", message),
                content);
        }
    }
}
