using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zapad.Model.Question
{
    /// <summary>
    /// Модель-абстрация группы ответов на вопросы, 1:M с crm_QuestionAnswer
    /// </summary>
    public class crm_QuestionAnswerGroup
    {
        /// <summary>
        /// PK, первичный ключ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название группы ответов
        /// </summary>
        public string Title { get; set; }
    }
}
