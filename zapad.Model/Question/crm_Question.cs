using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

namespace zapad.Model.Question
{
    /// <summary>
    /// Модель-абстракция вопроса анкет (справочник вопросов)
    /// </summary>
    public class crm_Question
    {
        /// <summary>
        /// PK, первичный ключ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Формулировка вопроса
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Индикатор возможности пользователя выбора нескольких вариантов ответов
        /// </summary>
        public bool IsMultiplyChoice { get; set; }
        /// <summary>
        /// Индикатор возможности пользователя выбора поля "Другое"
        /// </summary>
        public bool IsCanOtherChoice { get; set; }
        /// <summary>
        /// Примечание (для административных функций)
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// Дата и время создания записи
        /// </summary>
        public DateTime TimeInput { get; set; }
        /// <summary>
        /// Дополнительная информация (зарезервированное поле, для хранения информации аудита)
        /// </summary>
        public XElement XmlInfo { get; set; }
    }
}
