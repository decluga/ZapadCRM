using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace zapad.Public.WebInterface.Models.Authorization
{
    /// <summary>
    /// Модель для связывания данных во View страницы входа в систему
    /// </summary>
    public class AuthRegFormModel
    {
        #region Форма входа
        /// <summary>
        /// Логин
        /// </summary>
        [Required(ErrorMessage = "Введите Вашу электронную почту")]
        public string login_Email { get; set; }
        
        /// <summary>
        /// Пароль
        /// </summary>
        [Required(ErrorMessage = "Введите sms-пароль")]
        public string login_SmsPwd { get; set; }
        #endregion

        #region Форма регистрации
        /// <summary>
        /// Логин
        /// </summary>
        [Required(ErrorMessage = "Введите Вашу электронную почту")]
        public string reg_Email { get; set; }
        
        /// <summary>
        /// Телефон
        /// </summary>
        [Required(ErrorMessage = "Введите Ваш телефон")]
        public string reg_Phone { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required(ErrorMessage = "Введите значение поля 'Фамилия'")]
        public string reg_F { get; set; }
        
        /// <summary>
        /// Имя
        /// </summary>
        [Required(ErrorMessage = "Введите значение поля 'Имя'")]
        public string reg_I { get; set; }
        
        /// <summary>
        /// Отчество
        /// </summary>
        [Required(ErrorMessage = "Введите значение поля 'Отчество'")]
        public string reg_O { get; set; }
        
        /// <summary>
        /// Должность
        /// </summary>
        [Required(ErrorMessage = "Введите значение поля 'Должность'")]
        public string reg_Pos { get; set; }
        
        /// <summary>
        /// Организация
        /// </summary>
        [Required(ErrorMessage = "Введите значение поля 'Организация'")]
        public string reg_Org { get; set; }
        
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? reg_BDate { get; set; }
        #endregion

        /// <summary>
        /// Одноразовый токен (индивидуальный для каждого запроса пользователя)
        /// </summary>
        public int tkn { get; set; }

        /// <summary>
        /// Капча
        /// </summary>
        public string captchaInputText { get; set; }
        
        /// <summary>
        /// Идентификатор капчи
        /// </summary>
        public int captchaId { get; set; }
    }
}