using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using zapad.Public.WebInterface.Controllers;
using zapad.Public.WebInterface.Models.ServiceInteraction;
using zapad.Public.WebInterface.Models.Tools;

namespace zapad.Public.WebInterface.Models.Authorization
{
    /// <summary>
    /// Хелпер аутентификации и авторизации
    /// </summary>
    public static class Authentificate
    {
        /// <summary>
        /// Хелпер для доступа к сервису бизнес-данных и операций
        /// </summary>
        static private IServiceAccess service = new WebHostCacheWrapper();

        /// <summary>
        /// Проверяет, аутентифицировн ли пользователь
        /// </summary>
        /// <param name="session">Текущая сессия</param>
        /// <param name="Page">Контроллер, к которому идет обращение</param>
        /// <returns>true - пользовтель аутентифицирован, false - иначе</returns>
        public static bool checkAuthentificate(out UserSessionSet.UserSession session, BaseController Page)
        {
            session = UserSessionSet.Current.GetOrCreateSessionByCookie(Page.Request, Page.Response);
            try
            {
                if (session.State == UserSessionSet.SessionStates.Authenticated)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Получает права доступа к конкретной странице
        /// </summary>
        /// <param name="session">Текущая сессия</param>
        /// <param name="Pages">ID страницы</param>
        /// <returns>Права доступа к странице</returns>
        public static ObjectAccessResult checkAuthorization(UserSessionSet.UserSession session, long Pages)
        {
            ObjectAccessResult ResultInfo = new ObjectAccessResult() { Id = -1, Access = new CheckAccessResult() { Delete = false, Update = false, Read = false, InsertChildren = false } };

            XElement access = service.GetPageAccessRules(Pages, session.Key.ToString());

            if (access.Element("rc").getValue(5) == 0)
            {
                ResultInfo = access.Element("Answer").Elements("item").Select(u => new ObjectAccessResult()
                {
                    Id = u.Element("Id").getValue(-1),
                    Access = new CheckAccessResult()
                    {
                        Delete = u.Element("Delete").getValue(0) == 1,
                        InsertChildren = u.Element("InsertChild").getValue(0) == 1,
                        Read = u.Element("Read").getValue(0) == 1,
                        Update = u.Element("Update").getValue(0) == 1
                    }
                }).First();
            }
            return ResultInfo;
        }

        #region <Унаследованный код. На данный момент не требуется>

        //public static bool checkAuthorization(UserSessionSet.UserSession session, string Url = null, long TaskId = -1, long ProjectId = 1000)
        //{
        //    XElement request = new XElement("request");
        //    request.Add(new XElement("whguid", session.Key.ToString()));
        //    request.Add(new XElement("p", ProjectId));
        //    if (Url != null) request.Add(new XElement("m", Url));
        //    if (TaskId > -1) request.Add(new XElement("t", TaskId));
        //    XElement Access = WebApiZone.Current.GetResponse<XElement>("/mainarea/getAccess", request, WebApiZone.ContentTypes.xml);
        //    if (Access.Element("rc").getValue(0) == 0) return false;
        //    return true;
        //}

        //public static List<ObjectAccessResult> checkAuthorizationTasks(UserSessionSet.UserSession session, List<long> Tasks)
        //{
        //    List<ObjectAccessResult> ResultInfo = new List<ObjectAccessResult>();
        //    XElement request = new XElement("request");
        //    request.Add(new XElement("whguid", session.Key.ToString()));
        //    request.Add(new XElement("Items", String.Join(",", Tasks)));
        //    XElement Access = WebApiZone.Current.GetResponse<XElement>("/access/Task", request, WebApiZone.ContentTypes.xml);
        //    if (Access.Element("rc").getValue(5) == 0)
        //    {
        //        ResultInfo.AddRange(Access.Element("Answer").Elements("item").Select(u => new ObjectAccessResult()
        //        {
        //            Id = u.Element("Id").getValue(-1),
        //            Access = new CheckAccessResult()
        //            {
        //                Delete = u.Element("Delete").getValue(0) == 1,
        //                InsertChildren = u.Element("InsertChild").getValue(0) == 1,
        //                Read = u.Element("Read").getValue(0) == 1,
        //                Update = u.Element("Update").getValue(0) == 1
        //            }
        //        }));
        //    }
        //    return ResultInfo;
        //}

        //public static ObjectAccessResult checkAuthorizationTasks(UserSessionSet.UserSession session, long Tasks)
        //{
        //    ObjectAccessResult ResultInfo = new ObjectAccessResult() { Id = -1, Access = new CheckAccessResult() { Delete = false, Update = false, Read = false, InsertChildren = false } };
        //    XElement request = new XElement("request");
        //    request.Add(new XElement("whguid", session.Key.ToString()));
        //    request.Add(new XElement("Items", Tasks));
        //    XElement Access = WebApiZone.Current.GetResponse<XElement>("/access/Task", request, WebApiZone.ContentTypes.xml);
        //    if (Access.Element("rc").getValue(5) == 0)
        //    {
        //        ResultInfo = Access.Element("Answer").Elements("item").Select(u => new ObjectAccessResult()
        //        {
        //            Id = u.Element("Id").getValue(-1),
        //            Access = new CheckAccessResult()
        //            {
        //                Delete = u.Element("Delete").getValue(0) == 1,
        //                InsertChildren = u.Element("InsertChild").getValue(0) == 1,
        //                Read = u.Element("Read").getValue(0) == 1,
        //                Update = u.Element("Update").getValue(0) == 1
        //            }
        //        }).First();
        //    }
        //    return ResultInfo;
        //}
        #endregion
    }
}