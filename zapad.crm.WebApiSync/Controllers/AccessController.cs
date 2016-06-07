using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using zapad.crm.WebApiSync.Properties;
using zapad.Model.API;
using zapad.Model.Security;

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

            ObjectAccessResult access = new ObjectAccessResult()
            {
                Id = pageId,
                Access = new CheckAccessResult()
                {
                    Read = true,
                    Update = true,
                    InsertChildren = true,
                    Delete = true
                }
            };

            hubProxy.Invoke("OperationCallback", sessionKey, requestId,
                ReturnCodes.BuildRcAnswer(0, "Успешно", ObjectAccessResult.ToXElement(access)));
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

            ObjectAccessResult access = new ObjectAccessResult()
            {
                Id = pageId,
                Access = new CheckAccessResult()
                {
                    Read = true,
                    Update = true,
                    InsertChildren = true,
                    Delete = true
                }
            };

            return ReturnCodes.BuildRcAnswer(0, "Успешно", ObjectAccessResult.ToXElement(access));
            #endregion
        }
    }
}
