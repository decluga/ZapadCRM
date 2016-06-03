using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

namespace zapad.Model.Question
{
    /// <summary>
    /// Модель-абстрация варианта ответа на вопрос, 1:M c crm_Question
    /// </summary>
    public class crm_QuestionAnswer
    {
        /// <summary>
        /// PK, первичный ключ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ключ - связь с вопросом
        /// </summary>
        public int QuestionId { get; set; }
        /// <summary>
        /// Ключ - связь с группой ответов
        /// </summary>
        public int AnswerGroupId { get; set; }
        /// <summary>
        /// Формулировка варианта ответа на вопрос
        /// </summary>
        public string Title { get; set; }
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
