using System.Linq;
using System.Web;
using DataAccessLayer.Enums;

namespace WebUI.FullFramework.Core
{
    public static class IdentityHelper
    {
        public static bool IsUserInRole(params Role[] roles)
        {
            var sessionRole = HttpContext.Current.Session["UserRole"];

            if (sessionRole != null)
            {
                return roles.Any(x => x.ToString() == sessionRole.ToString());
            }

            foreach (var role1 in roles)
            {
                if (HttpContext.Current.User.IsInRole(role1.ToString()))
                {
                    HttpContext.Current.Session["UserRole"] = role1.ToString();
                    return true;
                }
            }

            return false;
        }
    }
}