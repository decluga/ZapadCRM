using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using zapad.crm.WebApiSync.Properties;

namespace zapad.crm.WebApiSync.Controllers
{
    public class CallRegistryController : ApiController
    {
        #region Расширенные методы для реестра звонков
        #region /GetAll: Получение списка звонков для реестра
        /// <param name="page">Текущая страница</param>
        /// <param name="count">Количество записей на странице</param>
        /// <param name="filter">Фильтр</param>
        /// <param name="searchTerm">Поиск</param>
        [Route("GetAll"), HttpPost]
        public async Task<XElement> GetAll([FromBody]XElement requestParameters)
        {
            #region Заглушка
            var hubConn = new HubConnection(Settings.Default.ResponseHubUrl);
            var hubProxy = hubConn.CreateHubProxy("ResponseHub");
            await hubConn.Start();

            var result = new XElement("Calls",
                                new XElement("Call",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Phone", "+7(123)235-14-11"),
                                    new XElement("Client",
                                        new XElement("Id", Guid.NewGuid()),
                                        new XElement("Name", "Валерий Рябов")
                                    ),
                                    new XElement("DateTime", DateTime.Now),
                                    new XElement("CallReceiver",
                                        new XElement("Id", Guid.NewGuid()),
                                        new XElement("Name", "Артем Аленин")
                                    ),
                                    new XElement("IsRepeatCall", false),
                                    new XElement("Status",
                                        new XElement("Id", 1),
                                        new XElement("Name", "Принят")
                                    ),
                                    new XElement("CallResultType",
                                        new XElement("Id", 1),
                                        new XElement("Name", "Передан")
                                    ),
                                    new XElement("DepartmentUser",
                                        new XElement("Id", Guid.NewGuid()),
                                        new XElement("Name", "Денис Стенюшкин"),
                                        new XElement("Department",
                                            new XElement("Id", Guid.NewGuid()),
                                            new XElement("Name", "ОП 3-2")
                                        )
                                    )
                                ),
                                new XElement("Call",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Phone", "+7(355)221-22-11"),
                                    new XElement("Client",
                                        new XElement("Id", Guid.NewGuid()),
                                        new XElement("Name", "Иванов Георгий")
                                    ),
                                    new XElement("DateTime", DateTime.Now),
                                    new XElement("CallReceiver",
                                        new XElement("Id", Guid.NewGuid()),
                                        new XElement("Name", "Иванов Александр")
                                    ),
                                    new XElement("IsRepeatCall", true),
                                    new XElement("Status",
                                        new XElement("Id", 1),
                                        new XElement("Name", "ПРОПУЩЕН!")
                                    ),
                                    new XElement("CallResultType",
                                        new XElement("Id", 1),
                                        new XElement("Name", "ПЕРЕЗВОНИТЬ")
                                    ),
                                    new XElement("DepartmentUser",
                                        new XElement("Id", Guid.NewGuid()),
                                        new XElement("Name", "Бекшин Олег"),
                                        new XElement("Department",
                                            new XElement("Id", Guid.NewGuid()),
                                            new XElement("Name", "ОП 3-1")
                                        )
                                    )
                                )
                            );
            result.Add(new XElement("Total", 50));
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));

            hubProxy.Invoke("OperationCallback", requestParameters.Element("sessionKey").Value, long.Parse(requestParameters.Element("requestId").Value), result);
            return WebHostCache.Models.DTO.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion

        //TODO vryabov: возможно объединить запросы с GetAll?
        #region /GetCallRegistryPeoples: Получение списка пользователей для фильтра "Кто принял"
        [Route("GetCallRegistryPeoples"), HttpGet]
        public async Task<XElement> GetCallRegistryPeoples(string sessionKey, long requestId)
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
            return WebHostCache.Models.DTO.ReturnCodes.BuildRcAnswer(0, "Успешно");
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

            var result = new XElement("Call",
                                new XElement("Id", Guid.NewGuid()),
                                new XElement("Phone", "+7(123)235-14-11"),
                                new XElement("Client",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Валерий Рябов")
                                ),
                                new XElement("DateTime", DateTime.Now),
                                new XElement("CallReceiver",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Артем Аленин")
                                ),
                                new XElement("IsRepeatCall", false),
                                new XElement("Status",
                                    new XElement("Id", 1),
                                    new XElement("Name", "Принят")
                                ),
                                new XElement("CallResultType",
                                    new XElement("Id", 1),
                                    new XElement("Name", "Передан")
                                ),
                                new XElement("DepartmentUser",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Денис Стенюшкин"),
                                    new XElement("Department",
                                        new XElement("Id", Guid.NewGuid()),
                                        new XElement("Name", "ОП 3-2")
                                    )
                                )
                            );
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));


            hubProxy.Invoke("OperationCallback", sessionKey, requestId, result);
            return WebHostCache.Models.DTO.ReturnCodes.BuildRcAnswer(0, "Успешно");
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

            var result = new XElement("Call",
                                new XElement("Id", Guid.NewGuid()),
                                new XElement("Phone", "+7(123)235-14-11"),
                                new XElement("Client",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Валерий Рябов")
                                ),
                                new XElement("DateTime", DateTime.Now),
                                new XElement("CallReceiver",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Артем Аленин")
                                ),
                                new XElement("IsRepeatCall", false),
                                new XElement("Status",
                                    new XElement("Id", 1),
                                    new XElement("Name", "Принят")
                                ),
                                new XElement("CallResultType",
                                    new XElement("Id", 1),
                                    new XElement("Name", "Передан")
                                ),
                                new XElement("DepartmentUser",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Денис Стенюшкин"),
                                    new XElement("Department",
                                        new XElement("Id", Guid.NewGuid()),
                                        new XElement("Name", "ОП 3-2")
                                    )
                                )
                            );
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));
            
            hubProxy.Invoke("OperationCallback", req.Element("sessionKey").Value, long.Parse(req.Element("requestId").Value), result);
            return WebHostCache.Models.DTO.ReturnCodes.BuildRcAnswer(0, "Успешно");
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

            var result = new XElement("Call",
                                new XElement("Id", Guid.NewGuid()),
                                new XElement("Phone", "+7(123)235-14-11"),
                                new XElement("Client",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Валерий Рябов")
                                ),
                                new XElement("DateTime", DateTime.Now),
                                new XElement("CallReceiver",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Артем Аленин")
                                ),
                                new XElement("IsRepeatCall", false),
                                new XElement("Status",
                                    new XElement("Id", 1),
                                    new XElement("Name", "Принят")
                                ),
                                new XElement("CallResultType",
                                    new XElement("Id", 1),
                                    new XElement("Name", "Передан")
                                ),
                                new XElement("DepartmentUser",
                                    new XElement("Id", Guid.NewGuid()),
                                    new XElement("Name", "Денис Стенюшкин"),
                                    new XElement("Department",
                                        new XElement("Id", Guid.NewGuid()),
                                        new XElement("Name", "ОП 3-2")
                                    )
                                )
                            );
            result.Add(new XElement("rc", 0));
            result.Add(new XElement("msg", ""));
            
            hubProxy.Invoke("OperationCallback", req.Element("sessionKey").Value, long.Parse(req.Element("requestId").Value), result);
            return WebHostCache.Models.DTO.ReturnCodes.BuildRcAnswer(0, "Успешно");
            #endregion
        }
        #endregion
        #endregion
    }
}
