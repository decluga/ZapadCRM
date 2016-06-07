using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using zapad.Public.WebInterface.Models.ServiceInteraction;

namespace zapad.Public.WebInterface.Controllers
{
    public class CallQueueController : Controller
    {
        private static List<string> callQueue = new List<string>() {
            "+7-842-223-45-56",
            "+7-325-556-46-73",
            "+7-989-898-98-98",
            "+7-903-333-66-66",
            "+7-932-555-64-64",
            "+7-842-223-45-56",
            "+7-989-898-98-98"
        };

        public void Update()
        {
            GlobalHost.ConnectionManager.GetHubContext<CallQueueHub>().Clients.All.sendQueue(callQueue);
        }
    }
}