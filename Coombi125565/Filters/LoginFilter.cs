using System;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Coombi125565.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class LoginFilter : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            String cn = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName ;
            if (!(cn == "account" || cn == "Account"))
            {
                if (!WebSecurity.IsAuthenticated)
                {
                    filterContext.Result = new RedirectResult("/account/login");
                }
            }
        }
    }
}