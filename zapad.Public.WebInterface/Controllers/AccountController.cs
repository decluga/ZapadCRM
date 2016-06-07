using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml.Linq;
using zapad.Model.Security;
using zapad.Model.Tools;
using zapad.Public.WebInterface.Models.Authorization;
using zapad.Public.WebInterface.Models.ServiceInteraction;
using zapad.Public.WebInterface.Models.Tools;
using zapad.Public.WebInterface.Properties;

namespace zapad.Public.WebInterface.Controllers
{
    /// <summary>
    /// Точка входа. Здесь только операции, связанные с авторизацией и регистрацией
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Хелпер для доступа к сервису бизнес-данных и операций
        /// </summary>
        private IServiceAccess service = new WebHostCacheWrapper();

        #region ActionResult Index(): Главная страница
        public ActionResult Index()
        {
            UserSessionSet.UserSession session = UserSessionSet.Current.GetOrCreateSessionByCookie(this.Request, this.Response);
            if (session.State == UserSessionSet.SessionStates.Authenticated)
                return RedirectToAction("Index", "CallRegistry");

            service.UpdateAnonymousSession(session.Key.ToString());
            return View(new AuthRegFormModel { tkn = session.NewSingleUseToken(), login_Email = this.Request.Cookies.AllKeys.Contains("l_mail") ? this.Request.Cookies["l_mail"].Value : "" });
        }
        #endregion

        #region JsonResult Registration(HomeIndexModel request): Получение и проверка регистрационных данных
        
        /// <summary>
        /// Первичная отработка результатов вкладки "Регистрация" на главной странице
        /// </summary>
        [System.Web.Mvc.HttpPost]
        public JsonResult Registration(AuthRegFormModel request)
        {
            UserSessionSet.UserSession session = UserSessionSet.Current.GetSessionByCookie(this.Request, request.tkn);
            service.UpdateAnonymousSession(session.Key.ToString());
            
            if (session.VerifyCaptcha(request.captchaId, request.captchaInputText) == false)
                return Json(new { code = 1, message = string.Empty }, JsonRequestBehavior.AllowGet);
            if (request.reg_Phone == null || request.reg_Email == null)
                return Json(new { code = 201, message = string.Empty }, JsonRequestBehavior.AllowGet);
            if (request.reg_F == null || request.reg_I == null || request.reg_O == null)
                return Json(new { code = 202, message = string.Empty }, JsonRequestBehavior.AllowGet);
            if (request.reg_Pos == null || request.reg_Org == null)
                return Json(new { code = 203, message = string.Empty }, JsonRequestBehavior.AllowGet);

            UserInfo[] rows = service.GetUsersByEmail(request.reg_Email, session.Key.ToString());
            if (rows.Any() == true)
                return Json(new { code = 3, message = "Пользователь, с указанной Вами электронной почтой уже зарегистрирован" }, JsonRequestBehavior.AllowGet);

            session.User = service.AddUser(new UserInfo
            {
                UserId = 0,
                F = request.reg_F,
                I = request.reg_I,
                O = request.reg_O,
                DisabledCause = "",
                IsAcceptAdmin = false,
                IsActivatedEMail = false,
                IsActivatedPhone = false,
                IsDisabled = false,
                EMail = request.reg_Email,
                Phone = request.reg_Phone.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", ""),
                LastActivity = DateTime.Now,
                SmsCodeHash = new byte[32],
                EMailActivateGuid = Guid.NewGuid().ToString(),
                XmlInfo = new XElement("data", new XElement("Organization", request.reg_Org)).ToString(),
                ADName = ""
            }, session.Key.ToString());

            new EMailSender
            {
                SmtpServer = Settings.Default.SmtpServer,
                UserName = Settings.Default.SmtpServerUsername,
                FromEMail = Settings.Default.SmtpServerUsername,
                Password = Settings.Default.SmtpServerPassword
            }.Send(request.reg_Email, "Активация регистрационных данных в CRM ЗАПАД", "Здравствуйте!\r\n" +
                "Вы оставили заявку за создание регистрационной информации для работы в ООО Запад на сайте crm.ulzapad.ru\r\n" +
                "Для того, чтобы активировать Ваш кабинет, пройдите по ссылке http://" + Settings.Default.domainName + "/Account/ActivateEMail?id=" + session.User.UserId + "&key=" + session.User.EMailActivateGuid + "\r\n" +
                "Если Вы не оставляли таковую заявку, либо Ваши планы изменились - просто проигнорируйте данное письмо.\r\n" +
                "Отвечать на это письмо не нужно.");
            request.captchaId.RemoveCaptcha();
            return Json(new { code = 0, message = string.Empty }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ActionResult ActivateEMail(long id, Guid key): Активация email пользователя, вывод ему странички для активации телефона
        
        /// <summary>
        /// Активация email пользователя, вывод ему странички для активации телефона
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="key">Guid, высланный ему в письме</param>
        public ActionResult ActivateEMail(int id, Guid key)
        {   
            // либо получаем сессию, либо создаем            
            UserSessionSet.UserSession session = UserSessionSet.Current.GetOrCreateSessionByCookie(this.Request, this.Response);
            if (session.State == UserSessionSet.SessionStates.Authenticated)
                RedirectToAction("Index", "CallRegistry");
            session.UserLink(id, session.Key.ToString());
            if (session.User.IsActivatedEMail == false && session.User.EMailActivateGuid != key.ToString())
                throw new HttpResponseException(HttpStatusCode.Forbidden);

            // регистрация пользователя в тех.зоне (sms-ки отправляет она сама)
            service.ActivateUserEmail(session.User, session.Key.ToString());
            
            ActivateEMailModel model = new ActivateEMailModel
            {
                User = session.User,
                tkn = session.NewSingleUseToken()
            };
            return View(model);
        }
        #endregion

        #region JsonResult ActivatePhone(int tkn, string smspwd, int captchaId, string captcha): Активания номера телефона
        
        /// <summary>
        /// Активация номера телефона
        /// </summary>
        public JsonResult ActivatePhone(int tkn, string smspwd)
        {
            UserSessionSet.UserSession session = UserSessionSet.Current.GetSessionByCookie(this.Request, tkn);

            if (String.IsNullOrEmpty(smspwd))
                return Json(new { code = 2, message = string.Empty }, JsonRequestBehavior.AllowGet);

            // отдаем на проверку sms-пароль
            var xanswer = service.ActivateUserPhone(session.User, session.Key.ToString(), smspwd);
            if (int.Parse(xanswer.Element("Rc").Value) != 0)    // если ошибка
                return Json(new { code = int.Parse(xanswer.Element("Rc").Value), message = xanswer.Element("Msg").Value }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                code = 0,
                message = string.Empty
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region JsonResult LoginStepOne (int tkn, string email): Первый этап авторизации
        
        /// <summary>
        /// Первый этап авторизации: проверяем активирован ли поьлзователь и отправляем СМС с паролем
        /// </summary>
        /// <param name="email">Электронный адрес пользователя</param>
        [System.Web.Mvc.HttpPost]
        public JsonResult LoginStepOne(int tkn, string email)
        {   
            // либо получаем сессию, либо создаем            
            UserSessionSet.UserSession session = UserSessionSet.Current.GetSessionByCookie(this.Request, tkn);
            if (session.State == UserSessionSet.SessionStates.Authenticated)
                RedirectToAction("Index", "CallRegistry");

            UserInfo[] rows = service.GetUsersByEmail(email, session.Key.ToString());
            if (!rows.Any())
                return Json(new { rc = 5, msg = String.Empty }, JsonRequestBehavior.AllowGet);
            session.User = rows[0];
            
            if (!session.User.IsActivatedEMail)
                return Json(new { rc = 4, msg = String.Empty }, JsonRequestBehavior.AllowGet);
            if (!session.User.IsActivatedPhone)
                return Json(new { rc = 3, msg = "У пользователя не активирован телефон." }, JsonRequestBehavior.AllowGet);

            // Отправка смс-ки
            XElement xanswer = service.RequestSmsPassword(session.User.UserId, session.Key.ToString());
            service.UpdateUserLastActivity(session.User.UserId, session.Key.ToString());

            return Json(new { rc = xanswer.Element("Rc").getValue(5), msg = xanswer.Element("Msg").getValue("") }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region JsonResult LoginStepTwo(int tkn, string email, string sms): Вход в систему
        /// <summary>
        /// Второй этап авторизации: проверяем пароль из СМС и пропускаем пользователя в систему
        /// </summary>
	    [System.Web.Mvc.HttpPost]
        public JsonResult LoginStepTwo(int tkn, string email, string sms)
        {
            UserSessionSet.UserSession session = UserSessionSet.Current.GetSessionByCookie(this.Request, tkn);

            UserInfo[] rows = service.GetUsersByEmail(email, session.Key.ToString());                
            if (rows.Any() == false)
            {
                System.Threading.Thread.Sleep(new TimeSpan(0, 0, 20));  // спим 20 секунд для усложнения сканирования
                return Json(new { rc = 2, msg = "Пользователь, с указанной Вами электронной почтой не зарегистрирован в системе" }, JsonRequestBehavior.AllowGet);
            }

            if (rows.Length != 1)
                return Json(new { rc = 3, msg = "Регистрационная информация разрушена, пожалуйства обратитесь в службу технической поддержки" }, JsonRequestBehavior.AllowGet);

            session.User = rows[0];

            // отдаем на проверку sms-пароль
            XElement xanswer = service.CheckSmsPassword(session.Key.ToString(), sms);                
            if (int.Parse(xanswer.Element("Rc").Value) != 0)    // если ошибка
                return Json(new { rc = int.Parse(xanswer.Element("Rc").Value), timeout = xanswer.Element("timeout").GetValueByPath<int>(0), msg = xanswer.Element("Msg").Value }, JsonRequestBehavior.AllowGet);

            service.UpdateUserAcceptAdmin(session.User.UserId, session.Key.ToString());
                
            session.State = UserSessionSet.SessionStates.Authenticated;
            session.DistrictId = xanswer.Element("DistrictId").getValue(-1);
            if (xanswer.Element("Districts").getValue("") != "")
            {
                session.FilterDistricts = xanswer.Element("Districts").getValue("").Split(',').Select(u => long.Parse(u)).ToList();
            }

            if (this.Response.Cookies.AllKeys.Contains("l_mail"))
                this.Response.Cookies["l_mail"].Value = session.User.EMail;
            else
                this.Response.SetCookie(new HttpCookie("l_mail", session.User.EMail) { Path = "/", Expires = DateTime.MaxValue });
            //
            return Json(new { rc = 0 }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region JsonResult LostPwd(int tkn, string email): Восстановление пароля через sms        
        /// <summary>
        /// Восстановление забытого пароля.
        /// Метод взлома: грузим головную страницу, получаем куку, затем вызываем метод, затем убиваем куку и сначала.
        /// Защита: уровень WebApi Zone
        /// </summary>
        [System.Web.Mvc.HttpPost]
        public JsonResult LostPwd(int tkn, string email)
        {
            UserSessionSet.UserSession session = UserSessionSet.Current.GetSessionByCookie(this.Request, tkn);

            if (session.LostPwdActivated == true)   // повторная попытка блокируется
                throw new HttpResponseException(HttpStatusCode.Forbidden);

            UserInfo[] rows = service.GetUsersByEmail(email, session.Key.ToString());
            if (!rows.Any())
            {
                System.Threading.Thread.Sleep(new TimeSpan(0, 1, 0));  // спим 1 минуту для усложнения сканирования
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            if (rows.Length != 1)
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            session.User = rows[0];

            // просим службу прислать sms-пароль
            XElement xanswer = service.RequestLostPasswordRestore(session.User.UserId, session.Key.ToString());
               
            if (int.Parse(xanswer.Element("Rc").Value) != 0)    // если ошибка
                throw new HttpResponseException(HttpStatusCode.Forbidden);

            service.UpdateUserLastActivity(session.User.UserId, session.Key.ToString());
            session.LostPwdActivated = true;
                
            return Json(new { rc = 0 }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Выход пользователя
        public ActionResult Logout()
        {
            UserSessionSet.UserSession session;

            if (!Authentificate.checkAuthentificate(out session, this))
                return RedirectToAction("Index", "Account");
            
            service.Logout(session.Key.ToString());
            session.State = SessionSetBase<UserSessionSet, UserSessionSet.UserSession>.SessionStates.Anonymous;

            return RedirectToAction("Index", "Account");
        }
        #endregion
    }
}