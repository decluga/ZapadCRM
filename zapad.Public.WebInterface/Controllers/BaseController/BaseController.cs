using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zapad.Public.WebInterface.Models.Authorization;

namespace zapad.Public.WebInterface.Controllers
{
    /// <summary>
    /// Базовый класс для всех контроллеров, реализующий необходимый всем контроллерам общий функционал:
    /// - авторизация и аутентификация
    /// - получение сессии текущего пользователя
    /// ВАЖНО: контроллеры бизнес-логики должны наследоваться от этого контроллера
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Сессия текущего пользователя
        /// </summary>
        protected UserSessionSet.UserSession session;

        /// <summary>
        /// Словарь для указания соответствий между методами контроллера и ID соответствующих страниц
        /// Должен заполняться в каждом контроллере отдельно
        /// </summary>
        protected Dictionary<string, long> pageIDs = new Dictionary<string, long>();

        /// <summary>
        /// Проверка доступа для текущего пользователя: должен быть аутентифицирован, должны быть права на данное действие
        /// </summary>
        /// <param name="filterContext">Контекст авторизации</param>
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            // Проверяем аутентификацию пользователя
            if (!Authentificate.checkAuthentificate(out this.session, this))
            {
                filterContext.Result = RedirectToAction("Index", "Account");
                return;
            }

            // Проверяем права доступа к данному объекту
            string actionName = filterContext.RouteData.GetRequiredString("action");
            bool readAccess = false;
            if (pageIDs.ContainsKey(actionName))
            {
                ObjectAccessResult AccessInfo = Authentificate.checkAuthorization(this.session, pageIDs[actionName]);
                readAccess = AccessInfo.Access.Read;                
            }
            if (!readAccess)
            {
                filterContext.Result = View("Error/Error403");
                return;
            }
        }
    }
}