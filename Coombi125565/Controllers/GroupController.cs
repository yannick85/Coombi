using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Coombi125565.Models;
using WebMatrix.WebData;

namespace Coombi125565.Controllers
{
    public class GroupController : Controller
    {
        private MyContext db = new MyContext();
        
        //See all my groups //link create a new group
        public ActionResult Index()
        {
            List<GroupModel> result = db.Groups.SqlQuery("SELECT Groups.* FROM Groups JOIN GroupMembership ON GroupMembership.GroupId = Groups.GroupId WHERE GroupMemberShip.UserId = " + WebSecurity.CurrentUserId + ";").ToList();
            List<GroupModel> mygroups = db.Groups.Where(g => g.OwnerId == WebSecurity.CurrentUserId).ToList();
            foreach (GroupModel group in mygroups) {
                result.Add(group);
            }
            return View(result);
        }

        // GET: /Group/Details/5 , See time of a group
        //if my group, link manage group members
        public ActionResult Details(int id = 0)
        {
            GroupModel groupmodel = db.Groups.Find(id);
            if (groupmodel == null)
            {
                return HttpNotFound();
            }
            List<PostModel> posts = db.Posts.Where(post => post.GroupId == id).OrderBy(post => post.Time).ToList();
            posts.Reverse();

            List<UserProfile> members = db.Users.SqlQuery("SELECT UserProfile.* FROM UserProfile JOIN GroupMembership ON GroupMembership.UserId = UserProfile.UserId WHERE GroupMembership.GroupId = " + id + ";").ToList();
            return View(new DetailsGroup {Group = groupmodel, Posts = posts, Members = members});
        }

        //Create a new Group
        public ActionResult Create()
        {
            return View();
        }
        // POST: /Group/Create
        [HttpPost]
        public ActionResult Create(AddGroupModel addgroupmodel)
        {
            if (ModelState.IsValid)
            {
                GroupModel group = new GroupModel();
                group.Name = addgroupmodel.Name;
                group.OwnerId = WebSecurity.CurrentUserId;
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(addgroupmodel);
        }

        // GET: /Group/Delete/5
        public ActionResult Delete(int id = 0)
        {
            GroupModel groupmodel = db.Groups.Find(id);
            if (groupmodel == null)
            {
                return HttpNotFound();
            }
            return View(groupmodel);
        }
        // POST: /Group/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GroupModel groupmodel = db.Groups.Find(id);
            db.Groups.Remove(groupmodel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //Unsuscribe for a group
        public ActionResult Unsuscribe(int id = 0)
        {
            List<GroupMembershipModel> gms = db.GroupMemberships.Where(g => g.GroupId == id && g.UserId == WebSecurity.CurrentUserId).ToList();
            foreach (GroupMembershipModel gm in gms)
            {
                db.GroupMemberships.Remove(gm);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Manage group member
        public ActionResult Manage(int id = 0)
        {
            List<UserProfile> Users = db.Users.Where(u => u.UserId != WebSecurity.CurrentUserId).ToList();
            List<GroupMembershipModel> gms = db.GroupMemberships.Where(g => g.GroupId == id).ToList();
            List<int> Members = new List<int>();
            foreach (GroupMembershipModel gm in gms) {
                Members.Add(gm.UserId);
            }
            return View(new ManageUsersGroup {Members = Members, Users = Users, Group = db.Groups.Find(id) });
        }
        [HttpPost]
        public ActionResult Manage(List<int> Members = null, int id = 0)
        {
            if (Members == null) {
                Members = new List<int>();
            }
            //On supprime les entrées pour ce groupe
            List<GroupMembershipModel> gms = db.GroupMemberships.Where(g => g.GroupId == id).ToList();
            foreach (GroupMembershipModel gm in gms) {
                db.GroupMemberships.Remove(gm);
            }
            foreach (int u in Members) {
                db.GroupMemberships.Add(new GroupMembershipModel {GroupId = id, UserId = u });
            }
            db.SaveChanges();
            List<UserProfile> Users = db.Users.Where(u => u.UserId != WebSecurity.CurrentUserId).ToList();
            return View(new ManageUsersGroup { Members = Members, Users = Users, Group = db.Groups.Find(id) });
        }
    }
}