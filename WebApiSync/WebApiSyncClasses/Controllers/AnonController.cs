
using System.Web.Http;

namespace WebApiSync
{
    [AllowAnonymous]
    public class AnonController : ApiController
    {
        [AllowAnonymous]
        public string Get()
        {
            return "succesful";
        }

        [AllowAnonymous]
        public string Post()
        {
            return "Post succesful";
        }

        [AllowAnonymous]
        public string Put()
        {
            return "Post succesful";
        }
    }
}
