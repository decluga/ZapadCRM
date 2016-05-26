using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace zapad.Public.WebInterface.Controllers
{
    //v.ryabov TODO: наследовать BaseController
    public class CallRegistryController : Controller
    {
        // GET: CallRegistry
        public ActionResult Index()
        {
            return View();
        }
    }
}