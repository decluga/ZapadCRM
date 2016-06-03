using Owin;
using System.Net;
using System.Web.Http;
using System.Web.Http.Owin;
using System.Web.Http.ExceptionHandling;
using zapad.crm.WebHostCache.Helpers;
using zapad.crm.WebHostCache.Models;
using zapad.crm.WebHostCache.Models.Tools;
using zapad.crm.WebHostCache.Properties;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using zapad.crm.WebHostCache.Models.Authentification;

[assembly: OwinStartup(typeof(zapad.crm.WebHostCache.Startup))]
namespace zapad.crm.WebHostCache
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Регистрируем и конфигурируем SignalR
            appBuilder.Map("/signalr", map => {
                map.UseCors(CorsOptions.AllowAll);
                map.MapSignalR();
            });

            HttpConfiguration selfHostConfig = new HttpConfiguration();

            selfHostConfig.Routes.MapHttpRoute(name: "DefaultApiRoute", routeTemplate: "api/{controller}", defaults: null);
            //selfHostConfig.Routes.MapHttpRoute(name: "DefaultApiWithId", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });
            selfHostConfig.Routes.MapHttpRoute(name: "DefaultApiWithAction", routeTemplate: "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });

            var config = SelfHostConfigRetriever.GetHostConfig();
            if (config.GetElementByName("authentication").Value == "yes")
            {
                HttpListener listener = (HttpListener)appBuilder.Properties["System.Net.HttpListener"];
                listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication;
            }

            selfHostConfig.MessageHandlers.Add(new MessageLoggingHandler());
            selfHostConfig.Services.Add(typeof(IExceptionLogger), new TraceExceptionLogger());

            SessionSet.Create(Settings.Default.msSessionLifiTime);

            WebApiSync.Create(Settings.Default.WebApiSyncUrls, ""/*/Settings.Default.CertificateToZoneThumbprint/*/, Settings.Default.msPingZoneInterval,
                    Settings.Default.PingRequestsCount, Settings.Default.NoPingNotifyInterval.TotalMilliseconds);
            appBuilder.UseWebApi(selfHostConfig);
        }
    }
}
