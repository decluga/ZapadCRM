using System.ComponentModel;
using System.ServiceProcess;

namespace zapad.crm.WebHostCache
{
    [RunInstaller(true)]
    public sealed class WebHostCacheInstallerProcess : ServiceProcessInstaller
    {
        public WebHostCacheInstallerProcess()
        {
            this.Account = ServiceAccount.NetworkService;
        }
    }

    [RunInstaller(true)]
    public sealed class WebHostCacheInstaller : ServiceInstaller
    {
        public WebHostCacheInstaller()
        {
            this.Description = "Ulzapad WebHostCache service";
            this.DisplayName = "zapad.crm.WebHostCache";
            this.ServiceName = "zapad.crm.WebHostCache";
            this.StartType = ServiceStartMode.Automatic;
        }
    }
}
