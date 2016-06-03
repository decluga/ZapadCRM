using System;
using System.Xml.Linq;

namespace zapad.crm.WebHostCache.Models.Tools
{
    public static class XAttribute_EXT
    {
        /// <summary>
        /// Возвращает число, или значение по умолчанию из XElement
        /// </summary>
        /// <param name="elem">XElement</param>
        /// <param name="def">Значение по умолчанию</param>
        /// <returns>Результат</returns>
        public static Int32 getValue(this XAttribute elem, Int32 def)
        {
            Int32 res = 0;
            try
            {
                if (Int32.TryParse(elem.Value, out res))
                    return res;
                else
                    return def;
            }
            catch
            {
                return def;
            }
        }
        /// <summary>
        /// Возвращает число с плавающей запятой, или значение по умолчанию из XElement
        /// </summary>
        /// <param name="elem">XElement</param>
        /// <param name="def">Значение по умолчанию</param>
        /// <returns>Результат</returns>
        public static float getValue(this XAttribute elem, float def)
        {
            float res = 0;
            try
            {
                if (float.TryParse(elem.Value, out res))
                    return res;
                else
                    return def;
            }
            catch
            {
                return def;
            }
        }

        /// <summary>
        /// Возвращает число, или значение по умолчанию из XElement
        /// </summary>
        /// <param name="elem">XElement</param>
        /// <param name="def">Значение по умолчанию</param>
        /// <returns>Результат</returns>
        public static long getValue(this XAttribute elem, long def)
        {
            long res = 0;
            try
            {
                if (long.TryParse(elem.Value, out res))
                    return res;
                else
                    return def;
            }
            catch
            {
                return def;
            }
        }
        /// <summary>
        /// Возвращает строку, или значение по умолчанию из XElement
        /// </summary>
        /// <param name="elem">XElement</param>
        /// <param name="def">Зачение по умолчанию</param>
        /// <returns>Результат</returns>
        public static string getValue(this XAttribute elem, string def)
        {
            try
            {
                if (elem.Value != null)
                    return elem.Value;
                else
                    return def;
            }
            catch
            {
                return def;
            }
        }
        /// <summary>
        /// Возвращает число Byte, или значение по умолчанию из XElement
        /// </summary>
        /// <param name="elem">XElement</param>
        /// <param name="def">Зачение по умолчанию</param>
        /// <returns>Результат</returns>
        public static byte getValue(this XAttribute elem, byte def)
        {
            byte res = 0;
            try
            {
                if (Byte.TryParse(elem.Value, out res))
                    return res;
                else
                    return def;
            }
            catch
            {
                return def;
            }
        }
        /// <summary>
        /// Возвращает дату, или значение по умолчанию из XElement
        /// </summary>
        /// <param name="elem">XElement</param>
        /// <param name="def">Зачение по умолчанию</param>
        /// <returns>Результат</returns>
        public static DateTime getValue(this XAttribute elem, DateTime def)
        {
            DateTime res = DateTime.Now;
            try
            {
                if (DateTime.TryParse(elem.Value, out res))
                    return res;
                else
                    return def;
            }
            catch
            {
                return def;
            }
        }
    }
}