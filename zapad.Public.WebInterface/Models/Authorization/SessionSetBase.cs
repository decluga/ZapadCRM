using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace zapad.Public.WebInterface.Models.Authorization
{
    /// <summary>
    /// Хранилище пользовательских секций
    /// </summary>
    /// <typeparam name="T">Тип ключа для поиска хранимой сессии</typeparam>
    /// <typeparam name="TSession">Тип хранимой сессии</typeparam>
    public class SessionSetBase<T, TSession> : TransactionListBase<Guid>
        where TSession : SessionSetBase<T, TSession>.SessionBase, new()
    {
        private long msTimerInvervalSessionReview;
        
        /// <summary>
        /// Приватный конструктор инициализации
        /// </summary>
        /// <param name="msTimerInverval">Интервал срабатывания таймера очистки сессий</param>
        protected SessionSetBase(long msTimerInverval)
            : base(msTimerInverval)
        {
            this.msTimerInvervalSessionReview = msTimerInverval;
        }
        
        /// <summary>
        /// Экземпляр класса-хранилища сессий
        /// </summary>
        public static SessionSetBase<T, TSession> Current { get; set; }

        /// <summary>
        /// Создает хранилище сессий
        /// </summary>
        /// <param name="msTimerInverval">Интервал срабатывания таймера очистки сессий</param>
        public static void Create(long msTimerInverval)
        {
            Current = new SessionSetBase<T, TSession>(msTimerInverval);
        }

        /// <summary>
        /// Состояния пользовательской сессии
        /// </summary>
        public enum SessionStates
        {
            /// <summary>
            /// Не аутентифицирован - анонимный пользователь
            /// </summary>
            Anonymous = 0,
            /// <summary>
            /// Аутентифицирован (произведен вход в систему)
            /// </summary>
            Authenticated = 1,
            /// <summary>
            /// Капча верифицирована
            /// </summary>
            CaptchaVerified = 2,
        }

        /// <summary>
        /// Имя устанавливаемой 
        /// </summary>
        private const string AUTH_COOKIE_NAME = "sid";

        /// <summary>
        /// Путь, передаваемый вместе с кукой
        /// </summary>
        private const string AUTH_COOKIE_PATH = "/";

        #region public Session GetOrCreateSessionByCookie(HttpRequestBase currentRequest, HttpResponseBase currentResponse)
        /// <summary>
        /// Обеспечиваем простую вещь: каждому браузеру[пользователю] - свою правильную куку и сессию.
        /// Никаких проверок и авторизаций - просто обеспечиваем наличие куки и сессии
        /// </summary>
        /// <param name="currentRequest">Текущий запрос</param>
        /// <param name="currentResponse">Текущий ответ</param>
        /// <returns>Пользовательская сессия</returns>
        public virtual TSession GetOrCreateSessionByCookie(HttpRequestBase currentRequest, HttpResponseBase currentResponse)
        {
            // если куки нет - новую сессию и куку, кука битая - новую сессию и куку, сессии нет - новую сессию и куку

            HttpCookie cookie = currentRequest.Cookies[AUTH_COOKIE_NAME];
            TSession session = null;
            if (cookie != null)
                session = this.Find<TSession>(Guid.Parse(cookie.Value));

            if (session == null)
            {
                session = this.Add<TSession>(Guid.NewGuid(), new TSession());
                if (currentResponse.Cookies.AllKeys.Contains(AUTH_COOKIE_NAME))
                {
                    currentResponse.Cookies[AUTH_COOKIE_NAME].Value = session.Key.ToString();
                }
                else
                {
                    currentResponse.SetCookie(new HttpCookie(AUTH_COOKIE_NAME, session.Key.ToString()) { Path = AUTH_COOKIE_PATH, Expires = DateTime.MaxValue });
                }
            }

            return session;
        }
        
        /// <summary>
        /// Удаляет сессию и инвалидирует куку
        /// </summary>
        /// <param name="currentRequest">Текущий запрос</param>
        /// <param name="Response">Текущий ответ</param>
        /// <returns>true - если сессия успешно удалена, false - в другом случае</returns>
        public virtual bool RemoveSessionAndCookie(HttpRequestBase currentRequest, HttpResponseBase Response)
        {
            try
            {
                Response.SetCookie(new HttpCookie(AUTH_COOKIE_NAME, currentRequest.Cookies[AUTH_COOKIE_NAME].Value) { Path = AUTH_COOKIE_PATH, Expires = DateTime.Now.AddDays(-1) });
                HttpCookie cookie = currentRequest.Cookies[AUTH_COOKIE_NAME];
                this.Remove(Guid.Parse(currentRequest.Cookies[AUTH_COOKIE_PATH].Value));
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region public Session GetSessionByCookie(HttpRequestMessage httpRequestMessage)

        /// <summary>
        /// Проверка наличия сессии по куке. Если что-то не так - исключение: HttpResponseException(HttpStatusCode.Forbidden)
        /// </summary>
        /// <param name="httpRequestMessage">Проверяемое сообщение</param>
        /// <param name="tkn">Уникальный токен, размещенный на странице в момент ее генерации (если 0 - проверка не осуществляется)</param>
        /// <returns>Найденная сессия</returns>
        public virtual TSession GetSessionByCookie(HttpRequestMessage httpRequestMessage, int tkn)
        {
            try
            {
                CookieHeaderValue cookie = httpRequestMessage.Headers.GetCookies(AUTH_COOKIE_NAME).FirstOrDefault();
                TSession session = this.Find<TSession>(Guid.Parse(cookie.Cookies.Last(x => x.Name == AUTH_COOKIE_NAME).Value));
                if (session.CurrentSingleUseToken != tkn && tkn != 0)
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                return session;
            }
            catch { throw new HttpResponseException(HttpStatusCode.Forbidden); }
        }

        /// <summary>
        /// Проверка наличия сессии по куке. Если что-то не так - исключение: HttpResponseException(HttpStatusCode.Forbidden)
        /// </summary>
        /// <param name="httpRequestMessage">Проверяемое сообщение</param>
        /// <param name="tkn">Уникальный токен, размещенный на странице в момент ее генерации (если 0 - проверка не осуществляется)</param>
        /// <param name="isNeedTestPersonal">Индикатор необходимости обязательный проверок на ошибки и безопасность привязки к личному кабинету</param>
        /// <returns>Найденная сессия</returns>
        public virtual TSession GetSessionByCookie(HttpRequestBase httpRequestBase, int tkn, bool isNeedTestPersonal = false)
        {
            try
            {
                TSession session = this.Find<TSession>(Guid.Parse(httpRequestBase.Cookies["sid"].Value));
                if (session.CurrentSingleUseToken != tkn && tkn != 0)
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                return session;
            }
            catch { throw new HttpResponseException(HttpStatusCode.Forbidden); }
        }

        #endregion

        /// <summary>
        /// Пользовательская сессия
        /// </summary>
        public abstract class SessionBase : TransactionBase
        {
            public SessionBase(TimeSpan lifeTime, SessionSetBase<T, TSession> Parent)
                : base(lifeTime, Parent)
            {
                this.State = SessionStates.Anonymous;
            }
            
            /// <summary>
            /// Текущее состояние сессии пользователя
            /// </summary>
            public SessionStates State { get; set; }

            #region Работа с токенами

            /// <summary>
            /// Текущий одноразовый токен (аналог AntiForgeryToken'а)
            /// </summary>
            public int CurrentSingleUseToken { get; protected set; }
            
            /// <summary>
            /// Генератор одноразового токена (аналог AntiForgeryToken'а)
            /// </summary>
            /// <returns>Одноразовый токен (аналог AntiForgeryToken'а)</returns>
            public int NewSingleUseToken()
            {
                Random rnd = new Random();
                return this.CurrentSingleUseToken = rnd.Next();
            }
            #endregion

            #region Капча: bool VerifyCaptcha(string captcha, int id)
            
            /// <summary>
            /// Проверка капчи (и удаление ее, если проверка успешна)
            /// </summary>
            /// <param name="captchaId">Идентификатор капчи</param>
            /// <param name="captchaInputText">Введенная пользователем строка капчи</param>
            /// <returns>Результат проверки</returns>
            public bool VerifyCaptcha(int captchaId, string captchaInputText)
            {
                if (captchaInputText == "" || captchaId.VerifyCaptcha(captchaInputText) == false)
                    return false;
                this.State = SessionSetBase<T, TSession>.SessionStates.CaptchaVerified;
                return true;
            }
            #endregion

            /// <summary>
            /// Генерация sms-пароля
            /// </summary>
            /// <returns>Вновь сгенерированный пароль</returns>
            public string GenSmsPassword(int smsPasswordLength)
            {
                int Max = (int)Math.Pow(10, smsPasswordLength - 1) * 9;
                for (int i = Max / 10; i > 0; i /= 10)
                    Max += i;
                return new Random().Next((int)Math.Pow(10, smsPasswordLength - 1), Max).ToString();
            }
        }
    }
}