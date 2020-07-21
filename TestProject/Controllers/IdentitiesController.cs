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
    public class IdentitiesController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ProjectDB db = new ProjectDB();

        // GET: Identities
        public ActionResult Index()
        {
            logger.Info($" AN ADMIN SHOWS  => IDENTITIES  NAME:{Session["UserName"]}---ID {Session["ID"]}");
            return View(db.Identities.ToList());
        }

        // GET: Identities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Identity identity = db.Identities.Find(id);
            if (identity == null)
            {
                return HttpNotFound();
            }
            logger.Info($" AN ADMIN SHOW DETAILS IDENTITIES : {identity.IdentityID}-- :{identity.Title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            return View(identity);
        }

        // GET: Identities/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Identity identity,  HttpPostedFileBase LogoURL)
        {
            if (ModelState.IsValid)
            {

                if (LogoURL != null)
                {

                    WebImage img = new WebImage(LogoURL.InputStream);
                    FileInfo imginfo = new FileInfo(LogoURL.FileName);

                    string picid = LogoURL.FileName + imginfo.Extension;
                    img.Resize(100, 100);
                    img.Save("~/UploadS/Identities/" + picid);

                    identity.LogoURL = "/Uploads/Identities/" + picid;
                }
                db.Identities.Add(identity);
                db.SaveChanges();
                logger.Info($" AN ADMIN CREATE AN IDENTITIES : {identity.IdentityID}-- :{identity.Title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                return RedirectToAction("Index");
            }

            return View();

        }
        // GET: Identities/Edit/5
        public ActionResult Edit(int id)
        {

            var identity = db.Identities.Where(x => x.IdentityID == id).SingleOrDefault();
            return View(identity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Identity identity, HttpPostedFileBase LogoURL)
        {
            if (ModelState.IsValid)
            {
                var tmp = db.Identities.Where(x => x.IdentityID == id).SingleOrDefault();

                if (LogoURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(tmp.LogoURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(tmp.LogoURL));
                    }
                    WebImage img = new WebImage(LogoURL.InputStream);
                    FileInfo imginfo = new FileInfo(LogoURL.FileName);

                    string picid = LogoURL.FileName + imginfo.Extension;
                    img.Resize(100, 100);
                    img.Save("~/Uploads/Identities/" + picid);

                    tmp.LogoURL = "/Uploads/Identities/" + picid;
                }
                logger.Info($" AN ADMIN EDIT AN IDENTITIES : {identity.IdentityID}-- :{identity.Title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                tmp.Title = identity.Title;
                tmp.Description = identity.Description;
                db.SaveChanges();
                logger.Info($" NEW IDENTITIES : {identity.IdentityID}-- :{identity.Title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                return RedirectToAction("Index");
            }

            return View(identity);

        }

        // GET: Identities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Identity identity = db.Identities.Find(id);
            if (identity == null)
            {
                return HttpNotFound();
            }
            return View(identity);
        }

        // POST: Identities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Identity identity = db.Identities.Find(id);
            logger.Info($" AN ADMIN DELETE AN IDENTITIES : {identity.IdentityID}-- :{identity.Title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            db.Identities.Remove(identity);
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
