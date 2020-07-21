using NLog;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class AdminsController : Controller
        
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ProjectDB db = new ProjectDB();

        // GET: Admins
        public ActionResult Index()
        {
            ViewBag.CommentValidation = db.Comments.Where(x => x.validation == "False").Count();
            ViewBag.CommentCount = db.Comments.ToList().Count();
            ViewBag.BlogCount = db.Blogs.ToList().Count();
            ViewBag.CategoryCount = db.Categories.ToList().Count();
            ViewBag.UsersCount = db.Users.Where(x=>x.role=="User").Count(); 
            ViewBag.Title = "Welcome Master";
            var tmp = db.Users.Where(x => x.role == "User").ToList();
            logger.Info($"Entered the mainpage=> NAME :{Session["UserName"]}-- ID:{Session["ID"]}-- ROLE:{Session["ROLE"]}");


            return View(tmp);
        }
        public ActionResult ShowAdmins()
        {

            var tmp = db.Users.Where(x => x.role == "Admin").ToList();
            logger.Info($"LISTED ALL ADMINS=> NAME :{Session["UserName"]}-- ID:{Session["ID"]}-- ROLE:{Session["ROLE"]}");
            return View(tmp);
        }
        public ActionResult UserPage()
        {
            ViewBag.Title = "Welcome Back Again";
            logger.Info($"Entered the USER/PAGE=>NAME :{Session["UserName"]}-- ID:{Session["ID"]}-- ROLE:{Session["ROLE"]}");

            return View();
        }
        public ActionResult UserPageShowDetails(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Where(x=>x.ID==id).SingleOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.role == "Admin")
            {

                return RedirectToAction("Index");
            }

            logger.Info($"Entered the USER/DETAIL=>NAME :{Session["UserName"]}-- ID:{Session["ID"]}-- ROLE:{Session["ROLE"]}");
            return View(user);
        }



        public ActionResult UserPageEditDetails(int id)
        {
            var user = db.Users.Where(x => x.ID == id).SingleOrDefault();
            logger.Info($"Entered the USER/EDITPAGE=> NAME :{Session["UserName"]}-- ID:{Session["ID"]}-- ROLE:{Session["ROLE"]}");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UserPageEditDetails(int id, User user, string password, HttpPostedFileBase Picture)
        {
            if (ModelState.IsValid)
            {
                var tmp = db.Users.Where(x => x.ID == id).SingleOrDefault();

                if (Picture != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(tmp.Picture)))
                    {
                        logger.Info($"EDIT PICTURE=>NAME :{Session["UserName"]}-- ID:{Session["ID"]}-- ROLE:{Session["ROLE"]}");
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
                tmp.Password = Crypto.Hash(password, "MD5");
                tmp.role = user.role;
                logger.Info($"EDITED PROFILE=>NAME :{Session["UserName"]}-- ID:{Session["ID"]}-- ROLE:{Session["ROLE"]}");
                db.SaveChanges();
                return RedirectToAction("UserPage");
            }

            return View(user);

        }

        
        public ActionResult UserPageShowAbout(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                logger.Error("HttpNotFound");
                return HttpNotFound();
            }
            logger.Info($"SHOW-ABOUT PAGE ABOUT PROFILE=>  NAME :{Session["UserName"]}-- ID:{Session["ID"]}-- ROLE:{Session["ROLE"]}");
            return View(user);
        }

        public ActionResult UserPageShowComments(int id)
        {
           var tmp = db.Comments.Where(x => x.UserID == id).SingleOrDefault();
            logger.Info($"SHOW COMMENTS=>  NAME :{Session["UserName"]}-- ID:{Session["ID"]}-- ROLE:{Session["ROLE"]}");
            return View(tmp);

          
        }


        public ActionResult UserPageEditComments(int? id)
        {
           
            Comment comment = db.Comments.Find(id);
            
            ViewBag.CommentID = new SelectList(db.Blogs, "blogID", "title", comment.CommentID);
            ViewBag.CommentID = new SelectList(db.Users, "ID", "Name", comment.CommentID);
            logger.Info($"ENTERED USER PAGE EDIT COMMENTS=> NAME :{Session["UserName"]}-- ID:{Session["ID"]}-- ROLE:{Session["ROLE"]}");
            return View(comment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UserPageEditComments([Bind(Include = "CommentID,UserName,Email,Comment1,blogID,UserID,validation")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserPage");
            }
            ViewBag.CommentID = new SelectList(db.Blogs, "blogID", "title", comment.CommentID);
            ViewBag.CommentID = new SelectList(db.Users, "ID", "Name", comment.CommentID);
            logger.Info($"COMMENTS EDITTED=> NAME :{Session["UserName"]}-- ID:{Session["ID"]}-- ROLE:{Session["ROLE"]}");
            return View(comment);
        }

        public ActionResult ShowUsers()
        {

            var tmp = db.Users.Where(x => x.role == "User").ToList();
            logger.Info($"AN ADMIN SHOWED USERS=> NAME :{Session["UserName"]}-- ID:{Session["ID"]}-- ROLE:{Session["ROLE"]}");

            return View(tmp);
        }

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                logger.Error($"USER NOT FOUND");
                return HttpNotFound();
            }
            logger.Info($"AN ADMIN SHOWED USERS=> NAME :{Session["UserName"]}-- ID:{Session["ID"]}-- ROLE:{Session["ROLE"]} ///  SHOWED USER ID:{id}");
            return View(user);
        }
        public ActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Login(User user)
        {
                var login = db.Users.Where(x => x.Email == user.Email).SingleOrDefault();

                if (login.Email == "NULL")
                {
                logger.Error($"LOGIN PAGE EMAIL ENTERED NULL");
                ViewBag.Uyari = "Password or Email invalid";
                return View("Login");
                }

                if (login.Email == user.Email && login.Password == Crypto.Hash(user.Password, "MD5"))
                {
                    Session["ID"] = login.ID;
                    Session["Email"] = login.Email;
                    Session["Role"] = login.role;
                    Session["Name"] = login.Name;
                    Session["LastName"] = login.LastName;
                    Session["UserName"] = login.UserName;
                    Session["Picture"] = login.Picture;
          

                if (login.role == "User")
                {
                    logger.Info($"Login an User =>NAME :{user.UserName}--ID :{user.ID}");
                    return RedirectToAction("UserPage", "Admins");

                }

                if (login.role == "Admin")
                {
                    logger.Info($"Login an Admin =>NAME :{user.UserName}--ID :{user.ID}");
                    return RedirectToAction("Index", "Admins");

                }



            }

            logger.Info($"The Password or email entered uncorrectly=>NAME : {user.UserName}-- ID:{user.ID}");
            ViewBag.Uyari = "Password or Email invalid";
            return View("Login");

        }

        public ActionResult Logout()
        {
            Session["ID"] = null;
            Session["Email"] = null;
            Session["Role"] = null;
            Session["Name"] = null;
            Session["LastName"] = null;
            Session["UserName"] = null;
            Session["Picture"] = null;
            Session.Abandon();
            logger.Info($"USER LOGOUT THE SYSTEM =>NAME : {Session["UserName"]}-- ID:{Session["ID"]}");
            return RedirectToAction( "Login");
        }

        public ActionResult ForgotMyPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotMyPassword(string Email = null)
        {
            var email = db.Users.Where(x => x.Email == Email).SingleOrDefault();
            if (email != null)
            {
                Random rnd = new Random();
                int newpassword = rnd.Next();

                User user = new User();
                email.Password = Crypto.Hash(Convert.ToString(newpassword), "MD5");
                db.SaveChanges();

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "temporarydemo12@gmail.com";
                WebMail.Password = "123456temp";
                WebMail.SmtpPort = 587;
                WebMail.Send(Email, "Admin Panel Entry Password", "Password :" + newpassword);
                logger.Info($"SENDED NEW PASSWORD TO  =>EMAIL : {Email}");
                ViewBag.Uyari = " New Password  Sended Successfuly";


            }
            else
            {
                logger.Info($"THERE IS AN ERROR WHEN SENDING EMAIL =>EMAIL : {Email}");
                ViewBag.Uyari = "There is an error,Please try later";
            }
            return View();
        }


        public ActionResult CreateAccount()
        {
            return View();
            
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateAccount(User user, string password, HttpPostedFileBase Picture)
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
                
                if (db.Users.Where(x => x.Email == user.Email).FirstOrDefault() != null)
                {
                    //ViewBag.Uyari = "This Email Already Exist";
                    return RedirectToAction("CreateAccountError");
                }
               
                if (db.Users.Where(x => x.UserName == user.UserName).FirstOrDefault()  != null)
                {
                    //ViewBag.Uyari = "This UserName Already Taken";
                    //didnt worked
                    return RedirectToAction("CreateAccountError2");
                }

                user.Password = Crypto.Hash(password, "MD5");
                db.Users.Add(user);
                db.SaveChanges();
                logger.Info($"CREATED AN ACCOUNT  =>NAME : {user.UserName}-- ID:{user.ID}");
                return RedirectToAction("Login");
            }

            return View();


        }

        public ActionResult CreateAccountError()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateAccountError(User user, string password, HttpPostedFileBase Picture)
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
                if (db.Users.Where(x => x.Email == user.Email) != null)
                {
                    ViewBag.Uyari = "This Email Already Exist";
                    logger.Error($"THIS EMAIL ALREADY TAKEN=> EMAIL: {user.Email} ");
                    return RedirectToAction("CreateAccountError");
                }
                if (db.Users.Where(x => x.UserName == user.UserName) != null)
                {
                    ViewBag.Uyari = "This UserName Already Taken";
                    logger.Error($"THIS USERNAME ALREADY TAKEN=> USERNAME: {user.UserName} ");
                    return RedirectToAction("CreateAccountError2");
                }


                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View();

        }


        public ActionResult CreateAccountError2()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateAccountError2(User user, string password, HttpPostedFileBase Picture)
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
                if (db.Users.Where(x => x.UserName == user.UserName) != null)
                {
                    ViewBag.Uyari = "This UserName Already Exist";
                    logger.Error($"THIS EMAIL ALREADY TAKEN=> EMAIL: {user.Email} ");
                    return RedirectToAction("CreateAccountError2");
                }
                if (db.Users.Where(x => x.Email == user.Email) != null)
                {
                    ViewBag.Uyari = "This Email Already Exist";
                    logger.Error($"THIS USERNAME ALREADY TAKEN=> USERNAME: {user.UserName} ");
                    return RedirectToAction("CreateAccountError");
                }


                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View();

        }


        public ActionResult Create()
        {
            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create( User user,string password,HttpPostedFileBase Picture)
        {

            if (ModelState.IsValid)
            {

                if (Picture != null)
                {

                    WebImage img = new WebImage(Picture.InputStream);
                    FileInfo imginfo = new FileInfo(Picture.FileName);

                    string picid = Picture.FileName + imginfo.Extension;
                    img.Resize(100, 100);
                    img.Save("~/Uploads/Admins/" + picid);

                    user.Picture = "/Uploads/Admins/" + picid;
                }

                user.Password = Crypto.Hash(password, "MD5");

                db.Users.Add(user);
                db.SaveChanges();
                logger.Info($" AN ADMIN ENTERED AN ADMIN => CREATED ADMIN NAME : {user.UserName}-- ID:{user.ID}--------CREATER NAME:{Session["UserName"]}");
                return RedirectToAction("ShowAdmins");
            }

            return View();

        }

        public ActionResult Edit(int id)
        {
            var user = db.Users.Where(x => x.ID == id).SingleOrDefault();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, User user, string password,HttpPostedFileBase Picture)
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
                logger.Info($" AN ADMIN EDIT AN ADMIN => EDITTED ADMIN NAME : {user.UserName}-- ID:{user.ID}--------EDITOR NAME:{Session["UserName"]}");
                return RedirectToAction("ShowAdmins");
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
        [ValidateInput(false)]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.FirstOrDefault(x => x.ID == id);
            db.Users.Remove(user);
            db.SaveChanges();
            logger.Info($" AN ADMIN DELETE AN ADMIN => DELETED ADMIN NAME : {user.UserName}-- ID:{user.ID}--------WHO DELETE NAME:{Session["UserName"]}");
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
