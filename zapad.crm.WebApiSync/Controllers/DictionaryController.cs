using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using zapad.crm.WebApiSync.Properties;

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

            var result = new XElement("EventPeriod",
                                new XElement("EventPeriod",
                                    new XElement("Id", 1),
                                    new XElement("Name", "Звонки за период")

                                ),
                                new XElement("EventPeriod",
                                    new XElement("Id", 2),
                                    new XElement("Name", "Звонки, поступившие сегодня")

                                ),
                                new XElement("EventPeriod",
                                    new XElement("Id", 3),
                                    new XElement("Name", "Звонки, поступившие вчера")

                                ),
                                new XElement("EventPeriod",
                                    new XElement("Id", 4),
                                    new XElement("Name", "Звонки, поступившие за посл. неделю")

                                ),
                                new XElement("EventPeriod",
                                    new XElement("Id", 5),
                                    new XElement("Name", "Звонки, поступившие за посл. месяцр")

                                )
                            );
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

            var result = new XElement("EventStatuses",
                                //new XElement("EventStatus",
                                //    new XElement("Id", 0),
                                //    new XElement("Name", "Все")

                                //),
                                new XElement("EventStatus",
                                    new XElement("Id", 1),
                                    new XElement("Name", "Принят")

                                ),
                                new XElement("EventStatus",
                                    new XElement("Id", 2),
                                    new XElement("Name", "Пропущен, перезвонили")

                                ),
                                new XElement("EventStatus",
                                    new XElement("Id", 3),
                                    new XElement("Name", "Звонок с сайта")

                                )
                            );
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

            var result = new XElement("EventResultTypes",
                                //new XElement("EventResultType",
                                //    new XElement("Id", 0),
                                //    new XElement("Name", "Все")

                                //),
                                new XElement("EventResultType",
                                    new XElement("Id", 1),
                                    new XElement("Name", "Передан")

                                ),
                                new XElement("EventResultType",
                                    new XElement("Id", 2),
                                    new XElement("Name", "ПЕРЕЗВОНИТЬ")

                                ),
                                new XElement("EventResultType",
                                    new XElement("Id", 3),
                                    new XElement("Name", "Непрофильный")

                                )
                            );
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

            var result = new XElement("Departments",
                                //new XElement("Department",
                                //    new XElement("Id", 0),
                                //    new XElement("Name", "Все")

                                //),
                                new XElement("Department",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "ОП З-1")

                                ),
                                new XElement("Department",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "ОП З-2")

                                ),
                                new XElement("Department",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Служба клиентского сервиса")

                                ),
                                new XElement("Department",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Закупки")

                                ),
                                new XElement("Department",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Бухгалтерия")

                                ),
                                new XElement("Department",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Юридический отдел")

                                )
                            );
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

            var result = new XElement("Peoples",
                                new XElement("People",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Артем Аленин")

                                ),
                                new XElement("People",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Иванов Александр")

                                ),
                                new XElement("People",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Денис Стенюшкин")

                                ),
                                new XElement("People",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Валерий Рябов")

                                ),
                                new XElement("People",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Малышева Ирина")

                                )
                            );
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
