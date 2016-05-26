using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zapad.Public.WebInterface.Models.Authorization
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
    }
}