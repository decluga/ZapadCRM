using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zapad.Public.WebInterface.Models.Authorization
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
    }
}