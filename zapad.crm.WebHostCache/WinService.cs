using Microsoft.Owin.Hosting;
using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

using zapad.crm.WebHostCache.Helpers;

namespace zapad.crm.WebHostCache
{
    public partial class WinService : ServiceBase
    {
        public WinService()
        {
            InitializeComponent();
            WinServiceName = this.ServiceName;
        }

        /// <summary>
        /// Имя сервиса - (для sql-исключений, когда невозможно записать что-либо в стандартную логу)
        /// </summary>
        public static string WinServiceName { get; private set; }
        /// <summary>
        /// Индикатор запуска отладчика
        /// </summary>
        public static bool IsDebug = false;
        /// <summary>
        /// Экземпляр хостингового класса WebAPI
        /// </summary>
        //HttpSelfHostServer restServer;

        public void Debug_Start() { this.OnStart(null); }

        protected override void OnStart(string[] args)
        {
            EventLog eventLog = new EventLog();
            eventLog.Source = WinServiceName;
            if (IsDebug == false)
            {
                eventLog.WriteEntry("Инициализация началась", EventLogEntryType.Information);
            }
            try
            {
                var config = SelfHostConfigRetriever.GetHostConfig();
                var siteUrl = config.GetElementByName("url").Value;
                using (WebApp.Start<Startup>(url: siteUrl))
                {
                    Thread.Sleep(Timeout.Infinite);
                }

                if (WinService.IsDebug == false)
                    eventLog.WriteEntry("Инициализация завершена", EventLogEntryType.Information);
            }
            catch (Exception e)
            {
                eventLog.WriteEntry("Ошибка при запуске: " + e.ExceptionToXElement().ToString(), EventLogEntryType.Error);
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            EventLog eventLog = new EventLog();
            eventLog.Source = WinServiceName;
            try
            {
                eventLog.WriteEntry("Остановлена успешно", EventLogEntryType.Information);
            }
            catch (Exception ee)
            {
                eventLog.WriteEntry("Остановлена с ошибкой: " + ee.ExceptionToXElement().ToString(), EventLogEntryType.Error);
            }
        }
    }
}
