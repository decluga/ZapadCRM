using System.Xml.Linq;
using zapad.Model.Tools;

namespace zapad.Model.Security
{
    /// <summary>
    /// Результат проверки прав доступа к объекту
    /// </summary>
    public class ObjectAccessResult
    {
        /// <summary>
        /// ID элемента
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Права доступа на отдельные операции
        /// </summary>
        public CheckAccessResult Access { get; set; }

        /// <summary>
        /// Преобразует экземпляр ObjectAccessResult в XElement
        /// </summary>
        /// <param name="obj">Исходный ObjectAccessResult</param>
        /// <returns>Результирующий XElement</returns>
        static public XElement ToXElement(ObjectAccessResult obj)
        {
            return new XElement("ObjectAccessResult",
                new XElement("Id", obj.Id),
                CheckAccessResult.ToXElement(obj.Access));
        }

        static public ObjectAccessResult FromXElement(XElement src)
        {
            ObjectAccessResult obj = new ObjectAccessResult()
            {
                Id = -1,
                Access = new CheckAccessResult()
                {
                    Read = false,
                    Update = false,
                    InsertChildren = false,
                    Delete = false
                }
            };

            if (src != null)
            {
                if (src.Element("Id") != null)
                    obj.Id = src.Element("Id").getValue(-1);
                if (src.Element("CheckAccessResult") != null)
                    obj.Access = CheckAccessResult.FromXElement(src.Element("CheckAccessResult"));
            }

            return obj;
        }
    }
}