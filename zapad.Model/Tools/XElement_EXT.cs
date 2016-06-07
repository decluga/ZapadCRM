using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace zapad.Model.Tools
{
    /// <summary>
    /// Библиотека расширений XElement для упрощения работы с API сервисов
    /// </summary>
    public static class XElement_EXT
    {
        
        /// <summary>
        /// Получение значения xml-узла, закопанного в иерархии, задаваемой как путь вида: Node1\\Node2\\....\\NodeN.
        /// Рекурсия не используется.
        /// </summary>
        /// <param name="xe">Исходный XElement</param>
        /// <param name="TagPath">Путь вида "Node1\\Node2\\....\\NodeN" (извлекается значение узла NodeN), имя XElement'а не учитывается. Путь может быть пустым, в этомм случае значение берется из элемента xe</param>
        /// <param name="ValueIsNotFound">Значение, если такового узла не существует</param>
        /// <returns>Найденное значение</returns>
        public static T GetValueByPath<T>(this XElement xe, T ValueIsNotFound, string TagPath = "")
        {
            if (xe == null) return ValueIsNotFound;
            
            string[] a = TagPath.Split('\\');
            foreach (string scurr in a.Where(x => x != string.Empty))
            {
                xe = xe.Element(scurr);
                if (xe == null) return ValueIsNotFound;
            }
            if (typeof(T) == typeof(string)) return (T)(object)xe.Value;

            T val = (T)Activator.CreateInstance(typeof(T));
            MethodInfo m = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });   // вытаскиваем нужный метод
            if (m == null)
                throw new NotSupportedException("В конечном типе не оказалось метода Parse");

            return (T)m.Invoke(val, new Object[] { xe.Value });
        }

        /// <summary>
        /// Получение перечисления XElement'ов по заданному пути.
        /// Рекурсия не используется
        /// </summary>
        /// <param name="xe">Исходный XElement</param>
        /// <param name="TagPath">Путь вида "Node1\\Node2\\....\\NodeN" (извлекается значение узла NodeN), имя XElement'а не учитывается. 
        /// Путь может быть пустым, в этомм случае значения берутся непосредственно из xe</param>
        /// <param name="TagName">Если не пустая строка - задает фильтр выборки из конечного XElement'а</param>
        /// <returns>Выборка найденных XElement'ов</returns>
        public static IEnumerable<XElement> GetXElementsByPath(this XElement xe, string TagPath = "", string TagName = "")
        {
            if (xe == null) return new XElement[] { };

            string[] a = TagPath.Split('\\');
            foreach (string scurr in a.Where(x => x != string.Empty))
            {
                xe = xe.Element(scurr);
                if (xe == null) return new XElement[] { };
            }
            return TagName == "" ? xe.Elements() : xe.Elements(TagName);
        }

        /// <summary>
        /// Возвращает число, или значение по умолчанию из XElement
        /// </summary>
        /// <param name="elem">XElement</param>
        /// <param name="def">Значение по умолчанию</param>
        /// <returns>Результат</returns>
        public static Int32 getValue(this XElement elem, Int32 def)
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
        /// Возвращает массив или значение по умолчанию из XElement
        /// </summary>
        /// <param name="elem">XElement</param>
        /// <param name="def">Значение по умолчанию</param>
        /// <returns>Результат</returns>
        public static byte[] getValue(this XElement elem, byte[] def)
        {
            byte[] res = new byte[0];
            try
            {
                res = Convert.FromBase64String(elem.Value);
                return res;
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
        public static long getValue(this XElement elem, long def)
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
        public static string getValue(this XElement elem, string def)
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
        public static byte getValue(this XElement elem, byte def)
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
        public static DateTime getValue(this XElement elem, DateTime def)
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

        /// <summary>
        /// Возвращает Decimal, или значение по умолчанию из XElement
        /// </summary>
        /// <param name="elem">XElement</param>
        /// <param name="def">Зачение по умолчанию</param>
        /// <returns>Результат</returns>
        public static decimal getValue(this XElement elem, decimal def)
        {
            decimal res = default(decimal);
            try
            {
                if (decimal.TryParse(elem.Value.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingSign, System.Globalization.CultureInfo.InvariantCulture, out res))
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
        /// Возвращает логическое значение или значение по умолчанию из XElement
        /// </summary>
        /// <param name="elem">XElement</param>
        /// <param name="def">Зачение по умолчанию</param>
        /// <returns>Результат</returns>
        public static bool getValue(this XElement elem, bool def)
        {
            bool res = false;
            try
            {
                if (Boolean.TryParse(elem.Value, out res))
                    return res;
                else
                    return def;
            }
            catch
            {
                return def;
            }
        }

        public static decimal getValueDecimal(this XElement elem, decimal def)
        {
            try
            {
                decimal res = decimal.Parse(elem.Value.Split('.')[0]);
                if (elem.Value.Split('.').Count() > 1)
                {
                    res += (decimal.Parse(elem.Value.Split('.')[1]) / (decimal)Math.Pow(10.0, (double)elem.Value.Split('.')[1].Length));
                }
                return res;
            }
            catch
            {
                return def;
            }
        }

        /// <summary>
        /// Возвращает Guid, или значение по умолчанию из XElement
        /// </summary>
        /// <param name="elem">XElement</param>
        /// <param name="def">Значение по умолчанию</param>
        /// <returns>Результат</returns>
        public static Guid getValue(this XElement elem, Guid def)
        {
            Guid res = Guid.Empty;
            try
            {
                if (Guid.TryParse(elem.Value, out res))
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