using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using zapad.Model.Security;
using zapad.Public.WebInterface.Models.ServiceInteraction;

namespace zapad.Public.WebInterface.Models.Authorization
{
    /// <summary>
    /// Хранилище пользовательских секций
    /// </summary>
    public class UserSessionSet : SessionSetBase<UserSessionSet, UserSessionSet.UserSession>
    {
        public UserSessionSet(long msTimerInverval) : base(msTimerInverval) { }

        /// <summary>
        /// Пользовательская сессия с привязанным пользователем
        /// </summary>
        public class UserSession : SessionBase
        {
            IServiceAccess service;

            public UserSession() : base(new TimeSpan(Properties.Settings.Default.msSessionLifeTime), null)
            {
                service = new WebHostCacheWrapper();
            }

            /// <summary>
            /// Модель пользователя, работающего в сессии
            /// </summary>
            public UserInfo User { get; set; }

            /// <summary>
            /// ID района пользователя
            /// </summary>
            public long DistrictId { get; set; } = -1;

            /// <summary>
            /// Фильтр районов
            /// </summary>
            public List<long> FilterDistricts { get; set; } = new List<long>();

            /// <summary>
            /// Связывание записи профиля пользователя с данным экземпляром сессии по заданному id
            /// </summary>
            /// <param name="id">id пользователя</param>
            /// <param name="sessionKey">Ключ текущей сессии</param>
            public void UserLink(int id, string sessionKey)
            {
                UserInfo[] rows = service.GetUsersById(id, sessionKey);
                if (rows == null || rows.Length != 1) throw new HttpResponseException(HttpStatusCode.Forbidden);
                this.User = rows[0];
            }

            /// <summary>
            /// Индикатор того, что в рамках данной сессии уже была выслана sms-ка
            /// </summary>
            public bool LostPwdActivated { get; set; }
        }
    }
}