using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zapad.Public.WebInterface.Models.Authorization
{
    /// <summary>
    /// Устанавливает ID страницы для контроля доступа
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PageIDAttribute : Attribute
    {
        /// <summary>
        /// ID страницы
        /// </summary>
        public long PageID { get; set; }

        /// <summary>
        /// Инициализировать атрибут
        /// </summary>
        /// <param name="pageId">Устанавливаемый ID страницы</param>
        public PageIDAttribute(long pageId)
        {
            this.PageID = pageId;
        }
    }
}