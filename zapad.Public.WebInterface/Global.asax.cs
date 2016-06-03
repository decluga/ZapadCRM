using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using zapad.Public.WebInterface.Models.Authorization;
using zapad.Public.WebInterface.Models.Tools;
using zapad.Public.WebInterface.Properties;

namespace zapad.Public.WebInterface
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UserSessionSet.Create(Settings.Default.msSessionLifeTime);
            WebHostCache.Create(Settings.Default.WebHostCacheUrls, ""/*/Settings.Default.CertificateToZoneThumbprint/*/, Settings.Default.msPingZoneInterval,
                    Settings.Default.PingRequestsCount, Settings.Default.NoPingNotifyInterval.TotalMilliseconds);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
