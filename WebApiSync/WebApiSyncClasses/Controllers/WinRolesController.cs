
using System;
using System.Web.Http;

namespace WebApiSync
{
    public class WinRolesController : ApiController
    {
        [Authorize(Roles = "Пользователи")]
        public string Get()
        {
            throw new Exception("exmes");
            return "success";
        }
    }
}
