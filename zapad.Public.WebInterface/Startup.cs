using Microsoft.Owin;
using Owin;
using zapad.Public.WebInterface;

[assembly: OwinStartup(typeof(Startup))]
namespace zapad.Public.WebInterface
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}