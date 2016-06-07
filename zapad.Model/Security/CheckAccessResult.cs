using System.Xml.Linq;
using zapad.Model.Tools;

namespace zapad.Model.Security
{
    /// <summary>
    /// Объект, инкапсулирующий права на отдельные операции над объектом
    /// </summary>
    public struct CheckAccessResult
    {
        /// <summary>
        /// Возможность удаления
        /// </summary>
        public bool Delete;

        /// <summary>
        /// Возможность добавления дочерних элементов
        /// </summary>
        public bool InsertChildren;

        /// <summary>
        /// Возможность чтения
        /// </summary>
        public bool Read;

        /// <summary>
        /// Возможность редактирования (обновления)
        /// </summary>
        public bool Update;

        /// <summary>
        /// Преобразует экземпляр CheckAccessResult в XElement
        /// </summary>
        /// <param name="obj">Исходный CheckAccessResult</param>
        /// <returns>Результирующий XElement</returns>
        static public XElement ToXElement(CheckAccessResult obj)
        {
            return new XElement("CheckAccessResult",
                new XElement("Delete", obj.Delete),
                new XElement("InsertChildren", obj.InsertChildren),
                new XElement("Read", obj.Read),
                new XElement("Update", obj.Update));
        }

        /// <summary>
        /// Преобразует XElement в экземпляр CheckAccessResult
        /// </summary>
        /// <param name="src">Исходный XElement</param>
        /// <returns>Экземпляр CheckAccessResult</returns>
        static public CheckAccessResult FromXElement(XElement src)
        {
            CheckAccessResult obj = new CheckAccessResult();

            if (src != null)
            {
                if (src.Element("Delete") != null)
                    obj.Delete = src.Element("Delete").getValue(false);
                if (src.Element("InsertChildren") != null)
                    obj.InsertChildren = src.Element("InsertChildren").getValue(false);
                if (src.Element("Read") != null)
                    obj.Read = src.Element("Read").getValue(false);
                if (src.Element("Update") != null)
                    obj.Update = src.Element("Update").getValue(false);
            }

            return obj;
        }
    }
}