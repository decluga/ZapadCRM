using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zapad.Model.Main
{
    /// <summary>
    /// Запись о видах состояния задач
    /// </summary>
    public class main_TaskStatus
    {
        /// <summary>
        /// PK, первичный ключ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Вид состояния (статус) задачи
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Примечание (для административных функций)
        /// </summary>
        public string Remark { get; set; }
    }
}
