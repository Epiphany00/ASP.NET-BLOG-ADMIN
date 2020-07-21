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
    public class AboutsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ProjectDB db = new ProjectDB();

        // GET: Abouts
        public ActionResult Index()
        {
            logger.Info($" AN ADMIN SHOWS  => ABOUTS  NAME:{Session["UserName"]}---ID {Session["ID"]}");
            return View(db.Abouts.ToList());
        }

        // GET: Abouts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            logger.Info($" AN ADMIN SHOW DETAILS ABOUTS : {about.aboutID}-- :{about.Description}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            return View(about);
        }

        // GET: Abouts/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(About about)
        {
            if (ModelState.IsValid)
            {

                db.Abouts.Add(about);
                db.SaveChanges();
                logger.Info($" AN ADMIN CREATE AN ABOUTS : {about.aboutID}-- :{about.Description}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                return RedirectToAction("Index");
            }
            return View();
        }


        public ActionResult Edit(int id)
        {

            var about = db.Abouts.Where(x => x.aboutID == id).SingleOrDefault();
            return View(about);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, About about)
        {
            if (ModelState.IsValid)
            {
                var tmp = db.Abouts.Where(x => x.aboutID == id).SingleOrDefault();
                logger.Info($" AN ADMIN EDIT AN ABOUT : {about.aboutID}-- :{about.Description}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                tmp.Description = about.Description;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(about);

        }
        // GET: Abouts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // POST: Abouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            About about = db.Abouts.Find(id);
            logger.Info($" AN ADMIN DELETE AN ABOUTS : {about.aboutID}-- :{about.Description}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            db.Abouts.Remove(about);
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
