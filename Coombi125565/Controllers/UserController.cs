using Coombi125565.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Coombi125565.Controllers
{
    public class UserController : Controller
    {
        private MyContext db = new MyContext();

        //See all user
        public ActionResult Index()
        {
            /*String domainName = Regex.Match(WebSecurity.CurrentUserName, "^.*@(.*)$").Value;
            ViewBag.domainName = domainName;*/
            List<UserProfile> result = db.Users/*.Where(u => u.Email.EndsWith(domainName))*/.Where(u => u.UserId != WebSecurity.CurrentUserId).ToList();
            List<FollowModel> follows = db.Follows.Where(f => f.FollowerId == WebSecurity.CurrentUserId).ToList();
            List<int> followed = new List<int>();
            foreach (FollowModel follow in follows) {
                followed.Add(follow.FollowedId);
            }
            return View(new UserIndexModel { Users = result, Followed = followed});
        }

        //See details of a user, public posts ...
        public ActionResult Details(int id = 0)
        {
            UserProfile user = db.Users.Find(id);
            List<PostModel> posts = db.Posts.Where(p => p.UserId == id).Where(p => p.GroupId == 0).ToList();
            List<FollowModel> fm = db.Follows.Where(f => f.FollowerId == WebSecurity.CurrentUserId).Where(f => f.FollowedId == id).ToList();
            if (fm.Count > 0)
            {
                ViewBag.isFollowed = true;
            }
            else
            {
                ViewBag.isFollowed = false;
            }
            return View(new DetailsUser {User = user, Posts = posts });
        }

        //Follow an user
        public ActionResult Follow(int id  = 0)
        {
            db.Follows.Add(new FollowModel {FollowerId = WebSecurity.CurrentUserId, FollowedId = id});
            db.SaveChanges();
            return RedirectToAction("Index", "User");
        }

        //Follow an user
        public ActionResult Unfollow(int id = 0)
        {
            List<FollowModel> fm = db.Follows.Where(f => f.FollowerId == WebSecurity.CurrentUserId).Where(f => f.FollowedId == id).ToList();
            foreach (var f in fm) {
                db.Follows.Remove(f);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "User");
        }
    }
}
