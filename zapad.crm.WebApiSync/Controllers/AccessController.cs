using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using zapad.crm.WebApiSync.Properties;
using zapad.crm.WebHostCache.Models.DTO;

namespace zapad.crm.WebApiSync.Controllers
{
    public class AccessController : ApiController
    {
        [HttpGet]
        public async Task<XElement> Page(long pageId, string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();
            hubProxy.Invoke("OperationCallback", sessionKey, requestId,
                ReturnCodes.BuildRcAnswer(0, "Успешно",
                    new XElement("Item", 
                        new XElement("Id", 0),
                        new XElement("Read", 1),
                        new XElement("Delete", 1),
                        new XElement("Update", 1),
                        new XElement("InsertChild", 1))));
            return ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }

        [HttpGet]
        public XElement PageTransfer(long pageId)
        {
            #region Заглушка 
            return ReturnCodes.BuildRcAnswer(0, "Успешно",
                    new XElement("Item",
                        new XElement("Id", pageId),
                        new XElement("Read", 1),
                        new XElement("Delete", 1),
                        new XElement("Update", 1),
                        new XElement("InsertChild", 1)));
            #endregion
        }
    }
}
