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
        // Получить права доступа к странице системы. Запрашивается с WebInterface
        // <param name="pageId">Уникальный номер страницы (атрибут PageID метода контроллера)</param>
        // <param name="sessionKey">Ключ текущей сессии пользователя для аутентификации и авторизации. Возвращать обратно без изменений</param>
        // <param name="requestId">ID запроса для привязки ответа в конечной точке. Возвращать обратно без изменений</param>
        // <returns> В ответе на запрос отправляет "ОК:: запрос получен и принят в обработку.
        // Через SignalR возвращает структуру с правами доступа к объекту.</returns>
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

        // Аналогично Page(long pageId, string sessionKey, long requestId), но предназначен для проверок на WebHostCache
        // Результат должен отправляться в ответе на запрос, а не через SignalR
        // Структура результата аналогична
        [HttpGet]
        public XElement PageTransfer(string sessionKey, long pageId)
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
