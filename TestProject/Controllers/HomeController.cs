
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using NLog;
using PagedList;
using PagedList.Mvc;

using TestProject.Models;

namespace KurumsalWeb.Controllers
{
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ProjectDB db = new ProjectDB();
      
        public ActionResult Index()
        {
            ViewBag.Identities = db.Identities.SingleOrDefault();
            ViewBag.Services = db.Services.ToList().OrderByDescending(x => x.serviceID);
            logger.Info($" SOMEBODY ENTERED INTO => WEBSITE  NAME:{Session["UserName"]}---ID {Session["ID"]}---ROLE: {Session["Role"]}");
            return View();
        }

        public ActionResult SliderPartial()
        {
            ViewBag.Identities = db.Identities.SingleOrDefault();
            return View(db.Sliders.ToList().OrderByDescending(x => x.sliderID));
        }

        public ActionResult ServicePartial()
        {
            ViewBag.Identities = db.Identities.SingleOrDefault();
            return View(db.Services.ToList().OrderByDescending(X=> X.serviceID));
        }
        public ActionResult Abouts()
        {
            ViewBag.Identities = db.Identities.SingleOrDefault();
            ViewBag.Services = db.Services.ToList().OrderByDescending(x => x.serviceID);
            logger.Info($" SOMEBODY ENTERED INTO => ABOUTS  NAME:{Session["UserName"]}---ID {Session["ID"]}---ROLE: {Session["Role"]}");
            return View(db.Abouts.SingleOrDefault());
        }

        public ActionResult Services()
        {
            ViewBag.Identities = db.Identities.SingleOrDefault();
            logger.Info($" SOMEBODY ENTERED INTO => SERVICES  NAME:{Session["UserName"]}---ID {Session["ID"]}---ROLE: {Session["Role"]}");
            return View(db.Services.ToList().OrderByDescending(x => x.serviceID));
        }

        public ActionResult Contacts()
        {
            ViewBag.Identities = db.Identities.SingleOrDefault();
            logger.Info($" SOMEBODY ENTERED INTO => CONTACTS  NAME:{Session["UserName"]}---ID {Session["ID"]}---ROLE: {Session["Role"]}");
            return View(db.Contacts.SingleOrDefault());
        }

        [HttpPost]
        public ActionResult Contacts(string NameLastName=null,string Email=null, string Subject=null,string Message=null)
        {
            ViewBag.Identities = db.Identities.SingleOrDefault();

            WebMail.SmtpServer = "smtp.gmail.com";
            WebMail.EnableSsl = true;
            WebMail.UserName = "temporarydemo12@gmail.com";
            WebMail.Password = "123456temp";
            WebMail.SmtpPort = 587;
            WebMail.Send("temporarydemo12@gmail.com", Subject,  Message );
            ViewBag.Uyari = " Your Message Sended Successfuly";
            logger.Info($" SOMEBODY SENDED A MAIL => ABOUTS  NAME:{Session["UserName"]}---ID {Session["ID"]}---ROLE: {Session["Role"]}----EMAIL:{Session["Email"]}");
            return View();
        }



        public ActionResult Blogs(int Page = 1)
        {
            ViewBag.Identities = db.Identities.SingleOrDefault();
            logger.Info($" SOMEBODY ENTERED INTO => BLOGS  NAME:{Session["UserName"]}---ID {Session["ID"]}---ROLE: {Session["Role"]}");
            return View(db.Blogs.Include("Category").OrderByDescending(x => x.blogID).ToPagedList(Page,5));
        }
        public ActionResult CategoryBlog(int id, int Page = 1)
        {
            ViewBag.Identities = db.Identities.SingleOrDefault();

            return View(db.Blogs.Include("Category").OrderByDescending(x=>x.blogID).Where(x => x.Category.categoryID==id).ToPagedList(Page,5));
        }

        public ActionResult BlogDetail(int id )
        {
            ViewBag.User = db.Users.FirstOrDefault().UserName.ToString();
            ViewBag.Identities = db.Identities.SingleOrDefault();
            var tmp = db.Blogs.Include("Category").Include("Comments").Where(x => x.blogID == id).SingleOrDefault();
            logger.Info($" SOMEBODY ENTERED INTO => BLOGS  NAME:{Session["UserName"]}-- --ID {Session["ID"]}---ROLE: {Session["Role"]} ////// BLOG :{tmp.title}");

            return View(tmp);
        }

        public JsonResult MakeAComment(string UserName,string Email,string Comment1, int blogID)
        {
            ViewBag.Identities = db.Identities.SingleOrDefault();
          
            if (Comment1 == null)
            {

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            db.Comments.Add(new Comment { UserName = UserName, Email = Email, Comment1 = Comment1, blogID = blogID, validation="false" });
            logger.Info($" SOMEBODY MAKE COMMENTS INTO => BLOGS  NAME:{Session["UserName"]}-- --ID {Session["ID"]}---ROLE: {Session["Role"]} ////// BLOG :{blogID}");
            db.SaveChanges();
            return Json(false,JsonRequestBehavior.AllowGet);
        }
        public ActionResult BlogCategoryPartial()
        {
            ViewBag.Identities = db.Identities.SingleOrDefault();
            return PartialView(db.Categories.Include("Blogs").ToList().OrderByDescending(x => x.categoryname));
        }
        public ActionResult BlogSubmitDetail()
        {
            ViewBag.Identities = db.Identities.SingleOrDefault();
            return PartialView(db.Blogs.ToList().OrderByDescending(x => x.blogID));
        }

        public ActionResult FooterPartial()
        {
            ViewBag.Identities = db.Identities.SingleOrDefault();
            ViewBag.Abouts = db.Abouts.ToList().OrderByDescending(x => x.aboutID);
            ViewBag.Contacts = db.Contacts.SingleOrDefault();
            ViewBag.Blogs = db.Blogs.ToList().OrderByDescending(x => x.blogID);
            ViewBag.Services = db.Services.ToList().OrderByDescending(x => x.serviceID);
            return PartialView();
        }
    }
}