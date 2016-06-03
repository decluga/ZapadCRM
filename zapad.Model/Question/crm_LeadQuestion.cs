using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

namespace zapad.Model.Question
{
    /// <summary>
    /// Запись из набора ответов на вопросы по конкретному лиду в системе.
    /// Множество записей, связанных с конкретным лидом составляют содержимое анкет.
    /// 1:M c crm_Lead
    /// </summary>
    public class crm_LeadQuestion
    {
        /// <summary>
        /// PK, первичный ключ
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Ключ - связь с лидом
        /// </summary>
        public long LeadId { get; set; }
        /// <summary>
        /// Ключ - связь с вопросом
        /// </summary>
        public int QuestionId { get; set; }
        /// <summary>
        /// Ключ - связь с ответом
        /// </summary>
        public int QuestionAnswerId { get; set; }
        /// <summary>
        /// Текст ответа, здесь возможен свой вариант ответа (для случаев "другого варианта" ответа, а так же других, вводимых пользователем вручную)
        /// </summary>
        public string QuestionAnswerTxt { get; set; }
        /// <summary>
        /// Дата и время создания записи
        /// </summary>
        public DateTime TimeInput { get; set; }
        /// <summary>
        ///  Ключ - связь с сотрудником, создавшим запись
        /// </summary>
        public long UserId_Input { get; set; }
        /// <summary>
        /// Дополнительная информация (зарезервированное поле, для хранения информации аудита)
        /// </summary>
        public XElement XmlInfo { get; set; }
    }
}
