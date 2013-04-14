using System.Linq;
using System.Web.Mvc;

using WebMatrix.WebData;
using Coombi125565.Models;

namespace Coombi125565.Controllers
{
    public class IndexController : Controller
    {
        //See timeline
        public ActionResult Index()
        {
            MyContext db = new MyContext();
            var result = db.Posts.SqlQuery("Select Post.* FROM Post LEFT JOIN Follow ON (Follow.FollowedId = Post.UserId AND Follow.FollowerId = " + WebSecurity.CurrentUserId + ") WHERE Post.GroupId = 0 AND (Post.UserId = " + WebSecurity.CurrentUserId + " OR Follow.FollowId IS NOT null) ORDER BY Post.Time DESC").ToList();
            return View(result);
        }

        public ActionResult About()
        {
            return View();
        }

    }
}
