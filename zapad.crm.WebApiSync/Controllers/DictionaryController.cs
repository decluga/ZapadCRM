using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using zapad.crm.WebApiSync.Properties;
using zapad.Model.DTO.Dictionaries;

namespace zapad.crm.WebApiSync.Controllers
{
    public class DictionaryController : ApiController
    {
        #region Справочник "Периоды события"
        #region /GetCallPeriods: Получение справочника периодов события
        /// <summary>
        /// Сущность периода:
        /// Id, Name, Timespan
        /// </summary>
        /// <returns></returns>
        [Route("GetEventPeriods"), HttpGet]
        public async Task<XElement> GetEventPeriods(string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            var result = EventPeriodDTO.ArrayToXElement(new EventPeriodDTO[]
            {
                new EventPeriodDTO()
                {
                    Id = 1,
                    Name = "Звонки за период",
                },
                new EventPeriodDTO()
                {
                    Id = 2,
                    Name = "Звонки, поступившие сегодня",
                },
                new EventPeriodDTO()
                {
                    Id = 3,
                    Name = "Звонки, поступившие вчера"
                },
                new EventPeriodDTO()
                {
                    Id = 4,
                    Name = "Звонки, поступившие за посл. неделю",
                },
                new EventPeriodDTO()
                {
                    Id = 5,
                    Name = "Звонки, поступившие за посл. месяц",
                }
            });
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));
            
            hubProxy.Invoke("OperationCallback", sessionKey, requestId, result);
            return zapad.Model.API.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion
        #endregion

        #region Справочник "Статусы события"
        #region /GetEventStatuses: Получение справочника статусов события
        /// <summary>
        /// Сущность статусов события:
        /// Id, Name
        /// </summary>
        /// <returns></returns>
        [Route("GetEventStatuses"), HttpGet]
        public async Task<XElement> GetEventStatuses(string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            var result = EventStatusDTO.ArrayToXElement(new EventStatusDTO[]
            {
                new EventStatusDTO()
                {
                    Id = 1,
                    Name = "Принят",
                },
                new EventStatusDTO()
                {
                    Id = 2,
                    Name = "Пропущен, перезвонили",
                },
                new EventStatusDTO()
                {
                    Id = 3,
                    Name = "Звонок с сайта"
                }
            });
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));

            hubProxy.Invoke("OperationCallback", sessionKey, requestId, result);
            return zapad.Model.API.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion
        #endregion

        #region Справочник "Тип результата события"
        #region /GetEventResultTypes: Получение справочника типов результата события
        /// <summary>
        /// Сущность типа результатов события:
        /// Id, Name
        /// </summary>
        /// <returns></returns>
        [Route("GetEventResultTypes"), HttpGet]
        public async Task<XElement> GetEventResultTypes(string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            var result = EventResultTypeDTO.ArrayToXElement(new EventResultTypeDTO[]
            {
                new EventResultTypeDTO()
                {
                    Id = 1,
                    Name = "Передан",
                },
                new EventResultTypeDTO()
                {
                    Id = 2,
                    Name = "ПЕРЕЗВОНИТЬ",
                },
                new EventResultTypeDTO()
                {
                    Id = 3,
                    Name = "Непрофильный"
                }
            });
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));

            hubProxy.Invoke("OperationCallback", sessionKey, requestId, result);
            return zapad.Model.API.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion
        #endregion

        #region Справочник "Отделы"
        #region /GetDepartments: Получение справочника отделов
        /// <summary>
        /// Сущность отдел:
        /// Id, Name
        /// </summary>
        /// <returns></returns>
        [Route("GetDepartments"), HttpGet]
        public async Task<XElement> GetDepartments(string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            var result = DepartmentDTO.ArrayToXElement(new DepartmentDTO[]
            {
                new DepartmentDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "ОП З-1",
                },
                new DepartmentDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "ОП З-2",
                },
                new DepartmentDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "Служба клиентского сервиса"
                },
                new DepartmentDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "Закупки"
                },
                new DepartmentDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "Бухгалтерия"
                },
                new DepartmentDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "Юридический отдел"
                }
            });
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));

            hubProxy.Invoke("OperationCallback", sessionKey, requestId, result);
            return zapad.Model.API.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion
        #endregion

        #region Справочник "Сотрудники"
        #region /GetPeoples: Получение списка пользователей
        [Route("GetPeoples"), HttpGet]
        public async Task<XElement> GetPeoples(string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            var result = PeopleDTO.ArrayToXElement(new PeopleDTO[]
            {
                new PeopleDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "Артем Аленин",
                },
                new PeopleDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "Иванов Александр",
                },
                new PeopleDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "Денис Стенюшкин"
                },
                new PeopleDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "Малышева Ирина"
                },
                new PeopleDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "Валерий Рябов"
                }
            });
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));


            hubProxy.Invoke("OperationCallback", sessionKey, requestId, result);
            return zapad.Model.API.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion
        #endregion
    }
}
