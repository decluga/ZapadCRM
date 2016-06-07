
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace WebApiSync
{
    static class Program
    {
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
                        myLog.Source = "zapad.bbspp";
                        myLog.WriteEntry(ee.ExceptionToXElement().ToString(), EventLogEntryType.Error);
                    }
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new WinService() };
                ServiceBase.Run(ServicesToRun);
                /*/
                WinService service = new WinService();
                WinService.IsDebug = false;
                service.Debug_Start();
                Thread.Sleep(Timeout.Infinite);
                /*/
            }
        }
    }

    [RunInstaller(true)]
    public sealed class MyServiceInstallerProcess : ServiceProcessInstaller
    {
        public MyServiceInstallerProcess()
        {
            this.Account = ServiceAccount.NetworkService;
        }
    }

    [RunInstaller(true)]
    public sealed class MyServiceInstaller : ServiceInstaller
    {
        public MyServiceInstaller()
        {
            this.Description = "My service desc";
            this.DisplayName = "Service1";
            this.ServiceName = "Service1";
            this.StartType = ServiceStartMode.Automatic;
          //... Installer code here
            this.AfterInstall += new InstallEventHandler(MyServiceInstaller_AfterInstall);
        }

        private void MyServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (ServiceController sc = new ServiceController("Service1"))
            {
                sc.Start();
            }
        }

    }
}
