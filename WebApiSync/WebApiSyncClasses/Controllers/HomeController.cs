
using System.Security.Principal;
using System.Web.Http;

namespace WebApiSync
{
    [Authorize]
    public class HomeController : ApiController
    {
        public HomeController()
        {
        }

        [Authorize]
        public string Get()
        {
            WindowsPrincipal user = RequestContext.Principal as WindowsPrincipal;
            if (!ReferenceEquals(null, user))
            {
                return "userName " + user.Identity.Name;
            }

            return "empty Name";
        }
    }
}
