
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApiSync
{
    public class AuthorizeADAttribute : AuthorizeAttribute
    {
        public string Groups { get; set;}

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var config = SelfHostConfigRetriever.GetHostConfig();
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain, config.GetElementByName("domain").Value))
                {
                    UserPrincipal user = UserPrincipal.FindByIdentity(context, Thread.CurrentPrincipal.Identity.Name);
                    
                    List<string> userGroupsNames = new List<string>();
                    if (user != null)
                    {
                        PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();
                        
                        foreach (Principal p in groups)
                        {
                            if (p is GroupPrincipal)
                                userGroupsNames.Add(p.Name.ToString());
                        }

                        var requiredGroups = Groups.Split(new char[] { ',' });
                        foreach (var requiredGroup in requiredGroups)
                        {
                            if (userGroupsNames.Contains(requiredGroup))
                                return true;
                        }
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
