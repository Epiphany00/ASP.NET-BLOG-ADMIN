using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TestProject.Models;

namespace TestProject.Controllers
{
    public class UserController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        ProjectDB db = new ProjectDB();

        public ActionResult Index()
        {
            var tmp = db.Users.Where(x => x.role == "User").ToList();
            logger.Info($" AN ADMIN SHOWS ALL USERS NAME : NAME:{Session["UserName"]}---ID :{Session["ID"]}");
            return View(tmp);
        }

      
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            logger.Info($" AN ADMIN SHOW DETAILS A USER =>USER NAME : {user.UserName}-- ID:{user.ID}--------WHO SHOWS NAME:{Session["UserName"]}");
            return View(user);
        }
        public ActionResult ShowAbout(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult ShowComments(int? id)
        {
            ViewBag.Comments = db.Comments.Where(x => x.UserID == id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        



        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create( User user,string password, HttpPostedFileBase Picture)
        {
            if (ModelState.IsValid)
            {
               
                if (Picture != null)
                {
                   
                    WebImage img = new WebImage(Picture.InputStream);
                    FileInfo imginfo = new FileInfo(Picture.FileName);

                    string picid = Picture.FileName + imginfo.Extension;
                    img.Resize(100, 100);
                    img.Save("~/Uploads/Users/" + picid);

                    user.Picture = "/Uploads/Users/" + picid;
                }

                user.Password = Crypto.Hash(password, "MD5");
                db.Users.Add(user);
                db.SaveChanges();
                logger.Info($" AN ADMIN CREATE A USER =>USER NAME : {user.UserName}-- ID:{user.ID}--------ADMIN NAME:{Session["UserName"]}");
                return RedirectToAction("Index");
            }

            return View();

        }

        [Route("User/Login")]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {

            var login = db.Users.Where(x => x.Email == user.Email).SingleOrDefault();
            if (login.Email == user.Email && login.Password == Crypto.Hash(user.Password, "MD5"))
            {
                Session["UserID"] = login.ID;
                Session["Email"] = login.Email;
              
                return RedirectToAction("Index", "User");
            }
            ViewBag.Uyari = "Password or Email invalid";
            return View(user);

        }

        public ActionResult Edit(int id)
        {
            var user = db.Users.Where(x => x.ID == id).SingleOrDefault();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, User user, string password, HttpPostedFileBase Picture)
        {
            if (ModelState.IsValid)
            {
                var tmp = db.Users.Where(x => x.ID == id).SingleOrDefault();

                if (Picture != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(tmp.Picture)))
                    {
                        System.IO.File.Delete(Server.MapPath(tmp.Picture));
                    }
                    WebImage img = new WebImage(Picture.InputStream);
                    FileInfo imginfo = new FileInfo(Picture.FileName);

                    string picid = Picture.FileName + imginfo.Extension;
                    img.Resize(100, 100);
                    img.Save("~/Uploads/Users/" + picid);

                    tmp.Picture = "/Uploads/Users/" + picid;
                }
                tmp.Name = user.Name;
                tmp.LastName = user.LastName;
                tmp.UserName = user.UserName;
                tmp.Email = user.Email;
                tmp.birtday = user.birtday;
                tmp.About = user.About;
                tmp.role = user.role;
                tmp.Password = Crypto.Hash(password, "MD5");
                db.SaveChanges();
                logger.Info($" AN ADMIN EDIT A USER =>USER NAME : {user.UserName}-- ID:{user.ID}--------ADMIN NAME:{Session["UserName"]}");
                return RedirectToAction("Index");
            }

            return View(user);

        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.FirstOrDefault(x => x.ID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.FirstOrDefault(x => x.ID == id);
            logger.Info($" AN ADMIN DELETE A USER =>USER NAME : {user.UserName}-- ID:{user.ID}--------ADMIN NAME:{Session["UserName"]}");
            db.Users.Remove(user);
            db.SaveChanges();
     
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
