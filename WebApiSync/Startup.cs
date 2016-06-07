
using Owin;
using System.Net;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace WebApiSync
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration selfHostConfig = new HttpConfiguration();

            selfHostConfig.Routes.MapHttpRoute(name: "DefaultApiRoute", routeTemplate: "api/{controller}", defaults: null);
            selfHostConfig.Routes.MapHttpRoute(name: "DefaultApiWithId", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });
            selfHostConfig.Routes.MapHttpRoute(name: "DefaultApiWithAction", routeTemplate: "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });

            var config = SelfHostConfigRetriever.GetHostConfig();
            if (config.GetElementByName("authentication").Value == "yes")
            {
                HttpListener listener = (HttpListener)appBuilder.Properties["System.Net.HttpListener"];
                listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication;
            }

            selfHostConfig.MessageHandlers.Add(new MessageLoggingHandler());
            selfHostConfig.Services.Add(typeof(IExceptionLogger), new TraceExceptionLogger());
                        
            appBuilder.UseWebApi(selfHostConfig);
        }
    }
}
