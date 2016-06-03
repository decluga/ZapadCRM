using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using zapad.crm.WebApiSync.Helpers;

namespace zapad.crm.WebApiSync.Models
{
    static class LogStore
    {
        private static Dictionary<Guid, LogModel> logs = new Dictionary<Guid, LogModel>();
        public static Dictionary<Guid, LogModel> Logs
        {
            get { return logs; }
        }

        private static Timer timer;

        static LogStore()
        {
            var config = SelfHostConfigRetriever.GetHostConfig();
            long flushTimerInterval = Convert.ToInt64(config.GetElementByName("logInterval").Value);

            timer = new Timer(flushTimerInterval);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        /// <summary>
        /// Writes content to output console.
        /// </summary>
        public static void WriteContentToOutput()
        {
            foreach (var log in logs)
            {
                var logObj = log.Value;
                string debugInfo = string.Format("{0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} ",
                    logObj.WebAccountId, logObj.RequestInfo, logObj.RequestStart, logObj.RequestFinished, logObj.AuthorizationCause, logObj.IsException, logObj.Answer, logObj.AuditInfo);
                Debug.Print(debugInfo);
            }
        }

        /// <summary>
        /// Writes log data to database. 
        /// </summary>
        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<LogModel> logsToDatabase = new List<LogModel>();
            lock (logs)
            {
                logsToDatabase = logs.Where(log => log.Value.ReadyToDB).Select(log => log.Value).ToList();

                logs.ToList().Where(pair => pair.Value.ReadyToDB).ToList().ForEach(pair => logs.Remove(pair.Key));
            }

            using (SqlConnection connection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["LocalDBConnectionString"].ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                foreach (var logToDatabase in logsToDatabase)
                {
                    string query = string.Format("INSERT INTO Audit (WebAccountId, RequestInfo,RequestStart,RequestFinished," +
                        "AuthorizationCause, IsException, Answer, AuditInfo)" +
                        "VALUES ({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}');",
                        logToDatabase.WebAccountId, logToDatabase.RequestInfo, logToDatabase.RequestStart, logToDatabase.RequestFinished,
                        logToDatabase.AuthorizationCause, logToDatabase.IsException, logToDatabase.Answer, logToDatabase.AuditInfo);
                    command.CommandText = query;

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        query = string.Format("INSERT INTO Audit (WebAccountId, RequestInfo,RequestStart,RequestFinished," +
                        "AuthorizationCause, IsException, Answer, AuditInfo)" +
                        "VALUES ({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}');",
                        0, " ", DateTime.Now.ToString(Constants.DateFormat), DateTime.Now.ToString(Constants.DateFormat),
                        0, 1, "database error: " + ex.Message, "");
                        command.CommandText = query;
                        //command.ExecuteNonQuery();
                    }
                }
                //PrintData();
            }
        }

        private static void PrintData()
        {
            using (SqlConnection connection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["LocalDBConnectionString"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT WebAccountId, RequestInfo,RequestStart,RequestFinished," +
                        "AuthorizationCause, IsException, Answer, AuditInfo FROM Audit", connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string debugInfo = string.Format("{0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} ",
                                    reader.GetValue(reader.GetOrdinal("WebAccountId")).ToString(), reader.GetValue(reader.GetOrdinal("RequestInfo")).ToString(),
                                    reader.GetValue(reader.GetOrdinal("RequestStart")).ToString(), reader.GetValue(reader.GetOrdinal("RequestFinished")).ToString(),
                                    reader.GetValue(reader.GetOrdinal("AuthorizationCause")).ToString(), reader.GetValue(reader.GetOrdinal("IsException")).ToString(),
                                    reader.GetValue(reader.GetOrdinal("Answer")).ToString(), reader.GetValue(reader.GetOrdinal("AuditInfo")).ToString());
                            Debug.Print(debugInfo);
                        }
                    }
                }
            }
        }
    }
}
