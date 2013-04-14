using System;
using System.Linq;
using System.Web.Mvc;
using Coombi125565.Models;
using WebMatrix.WebData;
using System.Text.RegularExpressions;
using Coombi125565.Service;

namespace Coombi125565.Controllers
{
    public class PostController : Controller
    {
        private MyContext db = new MyContext();

        // GET: /Post/
        public ActionResult Index()
        {
            String domainName = Regex.Match(WebSecurity.CurrentUserName, "^.*@(.*)$").Value;
            ViewBag.domainName = domainName;
            var result = db.Posts.Where(p => p.User.Email.EndsWith(domainName)).ToList();
            return View(result);  
        }

        // GET: /Post/Details/5
        public ActionResult Details(int id = 0)
        {
            PostModel postmodel = db.Posts.Find(id);
            if (postmodel == null)
            {
                return HttpNotFound();
            }
            return View(postmodel);
        }

        // GET: /Post/Create
        public ActionResult Create(int gid = 0)
        {
            return View();
        }
        // POST: /Post/Create
        [HttpPost]
        public ActionResult Create(AddPostModel addPostModel, int gid = 0)
        {
            if (ModelState.IsValid)
            {
                PostModel post = new PostModel();       
                post.UserId = WebSecurity.CurrentUserId;
                post.Content = addPostModel.Content;
                post.GroupId = gid;
                post.Time = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                if (addPostModel.Picture != null && addPostModel.Picture.ContentType == "image/jpeg")
                {//seul les jpeg sont acceptée
                    BlobService blobService = new BlobService();
                    post.Picture = blobService.uploadBlobFromStream(post.PostId.ToString() + ".jpeg", addPostModel.Picture.InputStream, "image/jpeg");
                }
                db.SaveChanges();
                if (gid != 0)
                {
                    return Redirect("/Group/Details/"+gid.ToString());                
                }
                else
                {
                    return RedirectToAction("Index", "Index");
                }
            }

            return View(addPostModel);
        }

        // POST: /Post/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            PostModel postmodel = db.Posts.Find(id);
            if (postmodel.UserId == WebSecurity.CurrentUserId) {
                postmodel.Likes.RemoveAll(l => l.LikeId != 0);
                postmodel.Comments.RemoveAll(l => l.CommentId != 0);
                db.Posts.Remove(postmodel);
                db.SaveChanges();
            }
            return null;
        }

        //Like a post
        public ActionResult Like(int id = 0)
        {
            PostModel post = db.Posts.Find(id);
            post.Likes.Add(new LikeModel{ UserId = WebSecurity.CurrentUserId});
            db.SaveChanges();
            return null;
        }

        //Unlike a post
        public ActionResult Unlike(int id = 0)
        {
            PostModel post = db.Posts.Find(id);
            post.Likes.RemoveAll(l => l.UserId == WebSecurity.CurrentUserId);
            db.SaveChanges();
            return null;
        }

        //Comment a post
        public ActionResult Comment(String Comment = "",int id = 0)
        {
            PostModel post = db.Posts.Find(id);
            CommentModel comment = new CommentModel { UserId = WebSecurity.CurrentUserId, Content = Comment, Time = DateTime.Now };
            post.Comments.Add(comment);
            db.SaveChanges();
            comment.User = db.Users.Find(WebSecurity.CurrentUserId);
            return View(comment);
        }

        //Delete a comment
        public ActionResult DeleteComment(int id = 0)
        {
            CommentModel comment = db.Comments.Find(id);
            if (comment.UserId == WebSecurity.CurrentUserId) {
                db.Comments.Remove(comment);
            }
            db.SaveChanges();
            return null;
        }
    }
}