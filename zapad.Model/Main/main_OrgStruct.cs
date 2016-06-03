using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zapad.Model.Main
{
    /// <summary>
    /// Запись о пользователе или подразделении в системе
    /// </summary>
    public class main_OrgStruct
    {
        /// <summary>
        /// PK, первичный ключ
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Ключ предка (контейнера-подразделения, к которому относится данное подразделение или пользователь)
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// Индикатор того, что эта запись относится к пользователю (значение - true) или представляет подразделение (значение false)
        /// </summary>
        public bool IsUser { get; set; }
        /// <summary>
        /// Active Directory Name (поле БД)
        /// </summary>
        public string ADName { get; set; }
        /// <summary>
        /// Примечание (для административных функций) (поле БД, никак не синхронизируемое с AD)
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Имя пользователя (загружается из AD, синхронизируется в БД)
        /// </summary>
        public string I { get; set; }
        /// <summary>
        /// Фамилия пользователя (загружается из AD, синхронизируется в БД)
        /// </summary>
        public string F { get; set; }
        /// <summary>
        /// Выводимое имя (загружается из AD, синхронизируется в БД)
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// EMail пользователя (загружается из AD, синхронизируется в БД)
        /// </summary>
        public string EMail { get; set; }
        /// <summary>
        /// Номер телефона пользователя (загружается из AD, синхронизируется в БД)
        /// </summary>
        public long NumberFull { get; set; }
        /// <summary>
        /// Мобильный номер пользователя (загружается из AD, синхронизируется в БД)
        /// </summary>
        public long MobileNumber { get; set; }
        /// <summary>
        /// Наименование должности (загружается из AD, синхронизируется в БД)
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// Индикатор присутствия пользователя в AD
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
