using System;
using System.Collections;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

using zapad.crm.WebHostCache.Helpers;
using zapad.crm.WebHostCache.Models.Tools;
using System.Xml.Linq;
using zapad.crm.WebHostCache.Properties;

namespace zapad.crm.WebHostCache
{
    static class Program
    {
        static public RequestBuffer WebApiSyncRequestBuffer;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main(string[] args)
        {
            IDictionary saved = new Hashtable();
            if (args.Length > 0)
            {
                try
                {
                    AssemblyInstaller Installer = new AssemblyInstaller(System.IO.Path.GetFileName(Assembly.GetExecutingAssembly().Location), new string[] { });
                    switch (args[0].ToLower())
                    {
                        case "-i":
                        case "/i":
                        case "i":
                            {
                                Installer.UseNewContext = true;
                                Installer.Install(saved);
                                Installer.Commit(saved);
                                return;
                            }
                        case "-u":
                        case "/u":
                        case "u":
                            {
                                Installer.Uninstall(saved);
                                return;
                            }
                        case "debug":
                            {
                                WinService service = new WinService();
                                WinService.IsDebug = true;
                                service.Debug_Start();
                                WebApiSyncRequestBuffer = new RequestBuffer(WebApiSync.Current.GetResponse<XElement>, Settings.Default.msRequestResendInterval);
                                Thread.Sleep(Timeout.Infinite);
                                return;
                            }
                        default: return;
                    }
                }
                catch (Exception ee)
                {
                    if (WinService.IsDebug == false)
                    {
                        EventLog myLog = new EventLog();
                        myLog.Source = "zapad.crm.WebHostCache";
                        myLog.WriteEntry(ee.ExceptionToXElement().ToString(), EventLogEntryType.Error);
                    }
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new WinService() };
                WebApiSyncRequestBuffer = new RequestBuffer(WebApiSync.Current.GetResponse<XElement>, Settings.Default.msRequestResendInterval);
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
