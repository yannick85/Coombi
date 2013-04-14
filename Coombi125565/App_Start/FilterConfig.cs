using Coombi125565.Filters;
using Coombi125565.Models;
using System.Web;
using System.Web.Mvc;

namespace Coombi125565
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            /*MyContext mc = new MyContext();
            mc.Posts.Add(new PostModel { });*/
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LoginFilter());
            filters.Add(new InitializeSimpleMembershipAttribute());
        }
    }
}