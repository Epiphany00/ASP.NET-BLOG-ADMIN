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
    public class CategoriesController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ProjectDB db = new ProjectDB();

        // GET: Categories
        public ActionResult Index()
        {
            db.Configuration.LazyLoadingEnabled = false;
            logger.Info($" AN ADMIN SHOWS  => CATEGORIES  NAME:{Session["UserName"]}---ID {Session["ID"]}");
            return View(db.Categories.Include("Blogs").ToList().OrderByDescending(x => x.categoryID));
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            logger.Info($" AN ADMIN SHOW DETAILS CATEGORY : {category.categoryID}-- :{category.categoryname}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            return View(category);
        }


       
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                logger.Info($" AN ADMIN CREATE A CATEGORY : {category.categoryID}-- :{category.categoryname}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                return RedirectToAction("Index");
            }

            return View();

        }

        public ActionResult Edit(int id)
        {

            var category = db.Categories.Where(x => x.categoryID == id).SingleOrDefault();
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                var tmp = db.Categories.Where(x => x.categoryID == id).SingleOrDefault();
                logger.Info($" AN ADMIN EDIT A CATEGORY : {category.categoryID}-- :{category.categoryname}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                tmp.categoryname = category.categoryname;
                tmp.description = category.description;
                tmp.Blogs = category.Blogs;

                db.SaveChanges();
                logger.Info($"  NEW CATEGORY : {category.categoryID}-- :{category.categoryname}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                return RedirectToAction("Index");
            }

            return View(category);

        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            logger.Info($" AN ADMIN DELETE A CATEGORY : {category.categoryID}-- :{category.categoryname}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            db.Categories.Remove(category);
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
