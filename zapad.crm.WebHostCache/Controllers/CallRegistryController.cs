using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;

namespace zapad.crm.WebHostCache.Controllers
{
    public class CallRegistryController : ApiController
    {
        #region Расширенные методы для реестра звонков
        #region /GetAll: Получение списка звонков для реестра
        /// <param name="page">Текущая страница</param>
        /// <param name="count">Количество записей на странице</param>
        /// <param name="filter">Фильтр</param>
        /// <param name="searchTerm">Поиск</param>
        [Route("GetAll"), HttpGet]
        public async Task<XElement> GetAll(int page, int count, string filter, string searchTerm)
        {
            throw new NotImplementedException();
        }
        #endregion

        //TODO vryabov: возможно объединить запросы с GetAll?
        #region /GetCallRegistryPeoples: Получение списка пользователей для фильтра "Кто принял"
        [Route("GetCallRegistryPeoples"), HttpGet]
        public async Task<XElement> GetCallRegistryPeoples()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        #region CRUD-методы для реестра звонков
        #region /Get: Получение данных по идентификатору звонка
        /// <param name="req">XML с идентификатором звонка CallId</param>
        [Route("Get"), HttpGet]
        public async Task<XElement> Get(Guid callid)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region /Create: Создание звонка
        /// <param name="req">XML c заполненными данными по звонку</param>
        [Route("Create"), HttpPost]
        public async Task<XElement> Create([FromBody] XElement req)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region /Update: Изменение данных по идентификатору звонка
        /// <param name="req">XML с измененными полями и идентификатором звонка CallId</param>
        [Route("Update"), HttpPost]
        public async Task<XElement> Update([FromBody] XElement req)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
