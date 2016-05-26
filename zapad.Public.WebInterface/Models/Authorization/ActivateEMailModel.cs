using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zapad.Public.WebInterface.Models.Authorization
{
    /// <summary>
    /// Модель для формы активации email пользователя
    /// </summary>
    public class ActivateEMailModel
    {
        public UserInfo User { get; set; }

        public string SmsPwd { get; set; }

        public int tkn { get; set; }
    }
}