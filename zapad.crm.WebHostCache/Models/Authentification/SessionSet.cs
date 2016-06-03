using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using zapad.crm.WebHostCache.Models.Tools;
using zapad.crm.WebHostCache.Properties;
using zapad.Public.WebInterface.Models.Authorization;

namespace zapad.crm.WebHostCache.Models.Authentification
{
    public class SessionSet : TransactionListBase<Guid>
    {
        /// <summary>
        /// Приватный конструктор инициализации
        /// </summary>
        /// <param name="msTimerInverval">Интервал срабатывания таймера очистки сессий</param>
        private SessionSet(long msTimerInverval) : base(msTimerInverval) { }

        /// <summary>
        /// Экземпляр класса-хранилища сессий
        /// </summary>
        public static SessionSet Current { get; private set; }

        /// <summary>
        /// Создать хранилище сессий
        /// </summary>
        /// <param name="msTimerInverval"></param>
        public static void Create(long msTimerInverval)
        {
            Current = new SessionSet(msTimerInverval);
        }

        /// <summary>
        /// Найти пользователя по его GUID
        /// </summary>
        /// <param name="sessionKey">GUID пользователя</param>
        /// <returns></returns>
        public Session FindByWebUserId(Guid sessionKey)
        {
            return this.Find<Session>(sessionKey);
        }

        /// <summary>
        /// Создать для пользователя новую сессию по его GUID
        /// </summary>
        /// <param name="sessionKey">GUID пользователя</param>
        /// <returns>Экземпляр созданной сессии</returns>
        public Session CreateSessionByWebUserId(Guid sessionKey)
        {
            return this.Add<Session>(sessionKey, new Session(TimeSpan.FromMilliseconds(Settings.Default.msSessionLifiTime), SessionSet.Current, new Guid().ToString()));
        }

        /// <summary>
        /// Сессия пользователя
        /// </summary>
        public class Session : TransactionBase
        {
            /// <summary>
            /// Создать новую сессию
            /// </summary>
            /// <param name="lifeTime">Время жизни</param>
            /// <param name="Parent">Родительский контейнер</param>
            /// <param name="guid">GUID пользователя</param>
            public Session(TimeSpan lifeTime, SessionSet Parent, string guid) : base(lifeTime, Parent)
            {
                this.Guid = guid;
            }

            /// <summary>
            /// GUID пользователя
            /// </summary>
            public string Guid { get; private set; }

            /// <summary>
            /// Залогировался ли пользователь
            /// </summary>
            public bool IsAuthentificated {
                get {
                    // Запросить статус аутентификации у WebApiSync
                    XElement res = WebApiSync.Current.GetResponse<XElement>(@"api/Anonymous/IsAuthentificated?sessionKey=" + Guid.ToString());
                    return int.Parse(res.Element("Rc").Value) == 0;
                }
            }
        }


    }
}
