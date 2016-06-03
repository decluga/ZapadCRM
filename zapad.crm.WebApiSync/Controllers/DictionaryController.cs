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
        #region Справочник "Периоды звоноков"
        #region /GetCallPeriods: Получение справочника периодов звонков
        /// <summary>
        /// Сущность периода звонков:
        /// Id, Name, Timespan
        /// </summary>
        /// <returns></returns>
        [Route("GetCallPeriods"), HttpGet]
        public async Task<XElement> GetCallPeriods(string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            var result = new XElement("CallPeriods",
                                new XElement("CallPeriod",
                                    new XElement("Id", 1),
                                    new XElement("Name", "Звонки за период")

                                ),
                                new XElement("CallPeriod",
                                    new XElement("Id", 2),
                                    new XElement("Name", "Звонки, поступившие сегодня")

                                ),
                                new XElement("CallPeriod",
                                    new XElement("Id", 3),
                                    new XElement("Name", "Звонки, поступившие вчера")

                                ),
                                new XElement("CallPeriod",
                                    new XElement("Id", 4),
                                    new XElement("Name", "Звонки, поступившие за посл. неделю")

                                ),
                                new XElement("CallPeriod",
                                    new XElement("Id", 5),
                                    new XElement("Name", "Звонки, поступившие за посл. месяцр")

                                )
                            );
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));
            
            hubProxy.Invoke("OperationCallback", sessionKey, requestId, result);
            return WebHostCache.Models.DTO.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion
        #endregion

        #region Справочник "Статусы звонка"
        #region /GetCallStatuses: Получение справочника статусов звонка
        /// <summary>
        /// Сущность статусов звонка:
        /// Id, Name
        /// </summary>
        /// <returns></returns>
        [Route("GetCallStatuses"), HttpGet]
        public async Task<XElement> GetCallStatuses(string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            var result = new XElement("CallStatuses",
                                //new XElement("CallStatus",
                                //    new XElement("Id", 0),
                                //    new XElement("Name", "Все")

                                //),
                                new XElement("CallStatus",
                                    new XElement("Id", 1),
                                    new XElement("Name", "Принят")

                                ),
                                new XElement("CallStatus",
                                    new XElement("Id", 2),
                                    new XElement("Name", "Пропущен, перезвонили")

                                ),
                                new XElement("CallStatus",
                                    new XElement("Id", 3),
                                    new XElement("Name", "Звонок с сайта")

                                )
                            );
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));

            hubProxy.Invoke("OperationCallback", sessionKey, requestId, result);
            return WebHostCache.Models.DTO.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion
        #endregion

        #region Справочник "Тип результата звонка"
        #region /GetCallResultTypes: Получение справочника типов результата звонка
        /// <summary>
        /// Сущность типа результатов звонка:
        /// Id, Name
        /// </summary>
        /// <returns></returns>
        [Route("GetCallResultTypes"), HttpGet]
        public async Task<XElement> GetCallResultTypes(string sessionKey, long requestId)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            var result = new XElement("CallResultTypes",
                                //new XElement("CallResultType",
                                //    new XElement("Id", 0),
                                //    new XElement("Name", "Все")

                                //),
                                new XElement("CallResultType",
                                    new XElement("Id", 1),
                                    new XElement("Name", "Передан")

                                ),
                                new XElement("CallResultType",
                                    new XElement("Id", 2),
                                    new XElement("Name", "ПЕРЕЗВОНИТЬ")

                                ),
                                new XElement("CallResultType",
                                    new XElement("Id", 3),
                                    new XElement("Name", "Непрофильный")

                                )
                            );
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));

            hubProxy.Invoke("OperationCallback", sessionKey, requestId, result);
            return WebHostCache.Models.DTO.ReturnCodes.BuildRcAnswer(0, "Успешно");
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
            return WebHostCache.Models.DTO.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion
        #endregion
    }
}
