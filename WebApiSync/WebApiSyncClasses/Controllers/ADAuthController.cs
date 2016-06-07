
using System.Web.Http;

namespace WebApiSync
{
    public class ADAuthController : ApiController
    {
        [AuthorizeAD(Groups = "Высокий обязательный уровен,Вс")]
        public string Get()
        {
            return "success";
        }
    }

}
