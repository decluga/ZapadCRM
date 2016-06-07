
using System;
using System.Xml.Linq;

namespace WebApiSync
{
    public static class Exception_EXT
    {
        /// <summary>
        /// Конвертация исключения в стандартный вид (XElement)
        /// </summary>
        /// <param name="ee">Исключение</param>
        /// <returns>XElement-представление исключения</returns>
        public static XElement ExceptionToXElement(this Exception e)
        {
            XElement xr = new XElement("e",
                new XElement("te", e.GetType().ToString()),
                new XElement("msg", e.Message),
                new XElement("stack", e.StackTrace));
            if (e.InnerException != null)
                xr.Add(new XElement("ie", e.InnerException.ExceptionToXElement()));
            return xr;
        }
    }
}
