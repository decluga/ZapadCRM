using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using zapad.crm.WebApiSync.Helpers;

namespace zapad.crm.WebApiSync.Models
{
    public static class SessionStore
    {
        public static Dictionary<Guid, Session> sessions = new Dictionary<Guid, Session>();

        private static Timer timer;

        static SessionStore()
        {
            var config = SelfHostConfigRetriever.GetHostConfig();
            long flushTimerInterval = Convert.ToInt64(config.GetElementByName("sessionInterval").Value);

            timer = new Timer(flushTimerInterval);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        /// <summary>
        /// Сброс истёкших сессий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Guid[] keys = getOutdatedSessions();
            lock (sessions)
            {
                foreach (var key in keys)
                {
                    sessions.Remove(key);
                }
            }
        }

        private static Guid[] getOutdatedSessions()
        {
            return sessions.Where(s => s.Value.ExpirationTime > DateTime.Now).Select(x => x.Key).ToArray();
        }

        public static void AddSession(Guid guid, Session session)
        {
            sessions[guid] = session;
        }

        public static void UpdateLastActivity(Guid guid)
        {
            sessions[guid].LastActivity = DateTime.Now;
        }

        /// <summary>
        /// Все тот же ip у одного guid?
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsTheSameIp(Guid guid, string ip)
        {
            if (sessions[guid].CompareIpAdress(ip))
                return true;
            else
            {
                sessions[guid].SetIpAdress(ip);
                return false;
            }
        }

        public static bool ContainsKey(Guid guid)
        {
            return sessions.ContainsKey(guid);
        }

        /// <summary>
        /// Возвращает время, когда истечет период действия сессии
        /// </summary>
        /// <returns></returns>
        public static DateTime GetTimeOutdateOfSession()
        {
            var config = SelfHostConfigRetriever.GetHostConfig();
            return DateTime.Now.AddSeconds(Convert.ToInt64(config.GetElementByName("dateOfOutdateSessions").Value));
        }

        public static void setSmsHash(Guid guid, string hash)
        {
            sessions[guid].hashOfSms = hash;
        }

        public static void AddQueryToLastQueries(Guid guid, string url)
        {
            var config = SelfHostConfigRetriever.GetHostConfig();
            if (sessions[guid].LastQueries.Count == Convert.ToInt32(config.GetElementByName("sizeQueueOfLastQueries").Value))
                sessions[guid].LastQueries.Dequeue();

            sessions[guid].LastQueries.Enqueue(url);
        }
    }
}
