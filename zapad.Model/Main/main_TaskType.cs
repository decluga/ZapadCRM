using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zapad.Model.Main
{
    /// <summary>
    /// Запись о типе задачи (или сущности системы)
    /// </summary>
    public class main_TaskType
    {
        /// <summary>
        /// PK, первичный ключ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Тип задачи
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Примечание (для административных функций)
        /// </summary>
        public string Remark { get; set; }
    }
}
