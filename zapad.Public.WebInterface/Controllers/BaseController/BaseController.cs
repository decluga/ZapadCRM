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
            bool access = CheckAccess(filterContext);
            if (!access)
            {
                filterContext.Result = View("Error/Error403");
                return;
            }

            // Обеспечиваем корректный вывод имени текущего пользователя в меню системы
            this.ViewBag.userinfo = this.session.User;
        }

        /// <summary>
        /// Проверяет права доступа к запрашиваемому действию
        /// </summary>
        /// <param name="filterContext">Контекст запроса</param>
        /// <returns>true - доступ разрешен, false - иначе</returns>
        private bool CheckAccess(AuthorizationContext filterContext)
        {
            var actionName = filterContext.ActionDescriptor.ActionName;
            var methodInfo = filterContext.Controller.GetType().GetMethod(actionName);
            var attr = Attribute.GetCustomAttribute(methodInfo, typeof(PageIDAttribute)) as PageIDAttribute;

            long pageId = -1;
            if (attr != null)
                pageId = attr.PageID;
            
            ObjectAccessResult AccessInfo = Authentificate.checkAuthorization(this.session, pageId);
            bool readAccess = AccessInfo.Access.Read;

            return readAccess;
        }
    }
}