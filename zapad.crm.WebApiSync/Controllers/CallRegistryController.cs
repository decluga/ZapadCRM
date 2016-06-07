using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using zapad.crm.WebApiSync.Properties;
using zapad.Model.Calls.DTO;

namespace zapad.crm.WebApiSync.Controllers
{
    public class CallRegistryController : ApiController
    {
        #region Расширенные методы для реестра звонков
        #region /GetCalls: Получение списка звонков для реестра
        /// <param name="page">Текущая страница</param>
        /// <param name="count">Количество записей на странице</param>
        /// <param name="filter">Фильтр</param>
        /// <param name="searchTerm">Поиск</param>
        [Route("GetCalls"), HttpPost]
        public async Task<XElement> GetCalls([FromBody]XElement requestParameters)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            var result = CallDTO.ArrayToXElement(new CallDTO[]
            {
                new CallDTO
                {
                    Id = Guid.NewGuid(),
                    Phone = "+7(123)235-14-11",
                    Client = "Валерий Рябов",
                    DateTime = DateTime.Now,
                    CallReceiver = "Артем Аленин",
                    IsRepeatCall = false,
                    EventStatus = "Принят",
                    EventResultType = "Передан",
                    DepartmentUser = "Денис Стенюшкин",
                    DepartmentUserDepartment = "ОП 3-2"
                },
                new CallDTO
                {
                    Id = Guid.NewGuid(),
                    Phone = "+7(355)221-22-11",
                    Client = "Иванов Георгий",
                    DateTime = DateTime.Now,
                    CallReceiver = "Иванов Александр",
                    IsRepeatCall = true,
                    EventStatus = "ПРОПУЩЕН!",
                    EventResultType = "ПЕРЕЗВОНИТЬ",
                    DepartmentUser = "Бекшин Олег",
                    DepartmentUserDepartment = "ОП 3-1"
                }
            });
            result.Add(new XElement("Total", 50));
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));

            hubProxy.Invoke("OperationCallback", requestParameters.Element("sessionKey").Value, long.Parse(requestParameters.Element("requestId").Value), result);
            return zapad.Model.API.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion
        #endregion

        #region /Get: Получение данных по идентификатору звонка
        /// <param name="req">XML с идентификатором звонка CallId</param>
        [Route("Get"), HttpGet]
        public async Task<XElement> Get(Guid callid, string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            var result = CallDTO.ToXElement(new CallDTO()
            {
                Id = Guid.NewGuid(),
                Phone = "+7(123)235-14-11",
                Client = "Валерий Рябов",
                DateTime = DateTime.Now,
                CallReceiver = "Артем Аленин",
                IsRepeatCall = false,
                EventStatus = "Принят",
                EventResultType = "Передан",
                DepartmentUser = "Денис Стенюшкин",
                DepartmentUserDepartment = "ОП 3-2"
            });
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));


            hubProxy.Invoke("OperationCallback", sessionKey, requestId, result);
            return zapad.Model.API.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion

        #region CRUD-методы для реестра звонков
        #region /Create: Создание звонка
        /// <param name="req">XML c заполненными данными по звонку</param>
        [Route("Create"), HttpPost]
        public async Task<XElement> Create([FromBody] XElement req)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            var result = CallDTO.ToXElement(new CallDTO()
            {
                Id = Guid.NewGuid(),
                Phone = "+7(123)235-14-11",
                Client = "Валерий Рябов",
                DateTime = DateTime.Now,
                CallReceiver = "Артем Аленин",
                IsRepeatCall = false,
                EventStatus = "Принят",
                EventResultType = "Передан",
                DepartmentUser = "Денис Стенюшкин",
                DepartmentUserDepartment = "ОП 3-2"
            });
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));
            
            hubProxy.Invoke("OperationCallback", req.Element("sessionKey").Value, long.Parse(req.Element("requestId").Value), result);
            return zapad.Model.API.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion

        #region /Update: Изменение данных по идентификатору звонка
        /// <param name="req">XML с измененными полями и идентификатором звонка CallId</param>
        [Route("Update"), HttpPost]
        public async Task<XElement> Update([FromBody] XElement req)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            var result = CallDTO.ToXElement(new CallDTO()
            {
                Id = Guid.NewGuid(),
                Phone = "+7(123)235-14-11",
                Client = "Валерий Рябов",
                DateTime = DateTime.Now,
                CallReceiver = "Артем Аленин",
                IsRepeatCall = false,
                EventStatus = "Принят",
                EventResultType = "Передан",
                DepartmentUser = "Денис Стенюшкин",
                DepartmentUserDepartment = "ОП 3-2"
            });
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));
            
            hubProxy.Invoke("OperationCallback", req.Element("sessionKey").Value, long.Parse(req.Element("requestId").Value), result);
            return zapad.Model.API.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion
        #endregion
    }
}
