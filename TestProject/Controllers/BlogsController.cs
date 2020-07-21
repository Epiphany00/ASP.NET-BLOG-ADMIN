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
    public class BlogsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ProjectDB db = new ProjectDB();

        // GET: Blogs
        public ActionResult Index()
        {

            db.Configuration.LazyLoadingEnabled = false;
            logger.Info($" AN ADMIN SHOWS  => BLOGS  NAME:{Session["UserName"]}---ID {Session["ID"]}");
            return View(db.Blogs.Include("Category").ToList().OrderByDescending(x => x.blogID));
        
        }

        // GET: Blogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            logger.Info($" AN ADMIN SHOW DETAILS BLOGS : {blog.blogID}-- :{blog.title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            return View(blog);
        }

        // GET: Blogs/Create
        public ActionResult Create()
        {
            ViewBag.categoryID = new SelectList(db.Categories, "categoryID", "categoryname");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Blog blog, HttpPostedFileBase pictureURL)
        {
            if (ModelState.IsValid)
            {

                if (pictureURL != null)
                {

                    WebImage img = new WebImage(pictureURL.InputStream);
                    FileInfo imginfo = new FileInfo(pictureURL.FileName);

                    string picid = pictureURL.FileName + imginfo.Extension;
                    img.Resize(100, 100);
                    img.Save("~/Uploads/Blogs/" + picid);

                    blog.pictureURL = "/Uploads/Blogs/" + picid;
                }
             
                db.Blogs.Add(blog);
                db.SaveChanges();
                logger.Info($" AN ADMIN CREATE BLOGS : {blog.blogID}-- :{blog.title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                return RedirectToAction("Index");
            }

            return View();

        }
        // GET: Blogs/Edit/5
        public ActionResult Edit(int id)
        {

            var blog = db.Blogs.Where(x => x.blogID == id).SingleOrDefault();
            ViewBag.categoryID = new SelectList(db.Categories, "categoryID", "categoryname");
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Blog blog, HttpPostedFileBase pictureURL)
        {
            if (ModelState.IsValid)
            {
                var tmp = db.Blogs.Where(x => x.blogID == id).SingleOrDefault();

                if (pictureURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(tmp.pictureURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(tmp.pictureURL));
                    }
                    WebImage img = new WebImage(pictureURL.InputStream);
                    FileInfo imginfo = new FileInfo(pictureURL.FileName);

                    string picid = pictureURL.FileName + imginfo.Extension;
                    img.Resize(100, 100);
                    img.Save("~/Uploads/Blogs/" + picid);

                    tmp.pictureURL = "/Uploads/Blogs/" + picid;
                }
                logger.Info($" AN ADMIN EDIT BLOGS : {blog.blogID}-- :{blog.title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                tmp.description = blog.description;
                tmp.title = blog.title;
                tmp.categoryID = blog.categoryID;
                db.SaveChanges();
                logger.Info($" NEW BLOG : {blog.blogID}-- :{blog.title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                return RedirectToAction("Index");
            }

            return View(blog);

        }
        // GET: Blogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            logger.Info($" AN ADMIN DELETE BLOGS : {blog.blogID}-- :{blog.title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            db.Blogs.Remove(blog);
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
