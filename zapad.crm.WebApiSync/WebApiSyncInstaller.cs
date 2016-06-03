
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace zapad.crm.WebApiSync
{
    [RunInstaller(true)]
    public sealed class WebApiSyncInstallerProcess : ServiceProcessInstaller
    {
        public WebApiSyncInstallerProcess()
        {
            this.Account = ServiceAccount.NetworkService;
        }
    }

    [RunInstaller(true)]
    public sealed class WebApiSyncInstaller : ServiceInstaller
    {
        private readonly string _serviceName = "zapad.crm.WebApiSync";

        public WebApiSyncInstaller()
        {
            this.Description = "Ulzapad WebHostCache service";
            this.DisplayName = this._serviceName;
            this.ServiceName = this._serviceName;
            this.StartType = ServiceStartMode.Automatic;
            this.AfterInstall += new InstallEventHandler(WebApiSyncInstaller_AfterInstall);
        }

        private void WebApiSyncInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (ServiceController sc = new ServiceController(this._serviceName))
            {
                sc.Start();
            }
        }
    }
}
