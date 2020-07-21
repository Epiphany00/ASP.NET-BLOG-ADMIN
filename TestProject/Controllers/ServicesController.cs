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
    public class ServicesController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ProjectDB db = new ProjectDB();

        // GET: Services
        public ActionResult Index()
        {
            logger.Info($" AN ADMIN SHOWS  => SERVICES  NAME:{Session["UserName"]}---ID {Session["ID"]}");
            return View(db.Services.ToList());
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            logger.Info($" AN ADMIN SHOW DETAILS SERVICES : {service.serviceID}-- :{service.Description}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Service service, HttpPostedFileBase PictureURL)
        {
            if (ModelState.IsValid)
            {

                if (PictureURL != null)
                {

                    WebImage img = new WebImage(PictureURL.InputStream);
                    FileInfo imginfo = new FileInfo(PictureURL.FileName);

                    string picid = PictureURL.FileName + imginfo.Extension;
                    img.Resize(360, 360);
                    img.Save("~/Uploads/Services/" + picid);

                    service.PictureURL = "/Uploads/Services/" + picid;
                }
               
                db.Services.Add(service);
                db.SaveChanges();
                logger.Info($" AN ADMIN CREATE  SERVICES : {service.serviceID}-- :{service.Description}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                return RedirectToAction("Index");
            }

            return View();

        }

        // GET: Services/Edit/5

        public ActionResult Edit(int id)
        {

            var service = db.Services.Where(x => x.serviceID == id).SingleOrDefault();
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Service service, HttpPostedFileBase PictureURL)
        {
            if (ModelState.IsValid)
            {
                var tmp = db.Services.Where(x => x.serviceID == id).SingleOrDefault();

                if (PictureURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(tmp.PictureURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(tmp.PictureURL));
                    }
                    WebImage img = new WebImage(PictureURL.InputStream);
                    FileInfo imginfo = new FileInfo(PictureURL.FileName);

                    string picid = PictureURL.FileName + imginfo.Extension;
                    img.Resize(360, 360);
                    img.Save("~/Uploads/Services/" + picid);

                    tmp.PictureURL = "/Uploads/Services/" + picid;
                }
                tmp.Title = service.Title;
                tmp.Description = service.Description;
                logger.Info($" AN ADMIN EDIT A  SERVICE : {service.serviceID}-- :{service.Description}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(service);

        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.Services.Find(id);
            logger.Info($" AN ADMIN DELETE A  SERVICE : {service.serviceID}-- :{service.Description}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            db.Services.Remove(service);
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
