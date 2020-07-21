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
    public class SlidersController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ProjectDB db = new ProjectDB();

        // GET: Sliders
        public ActionResult Index()
        {
            logger.Info($" AN ADMIN SHOWS  => SLIDERS  NAME:{Session["UserName"]}---ID {Session["ID"]}");
            return View(db.Sliders.ToList());
        }

        // GET: Sliders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }

            logger.Info($" AN ADMIN SHOW DETAILS SLIDERS : {slider.sliderID}-- :{slider.title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            return View(slider);
        }

        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Slider slider, HttpPostedFileBase pictureURL)
        {
            if (ModelState.IsValid)
            {

                if (pictureURL != null)
                {

                    WebImage img = new WebImage(pictureURL.InputStream);
                    FileInfo imginfo = new FileInfo(pictureURL.FileName);

                    string picid = pictureURL.FileName + imginfo.Extension;
                    img.Resize(1024, 360);
                    img.Save("~/Uploads/Sliders/" + picid);

                    slider.pictureURL = "/Uploads/Sliders/" + picid;
                }

                db.Sliders.Add(slider);
                db.SaveChanges();
                logger.Info($" AN ADMIN CREATE  SLIDERS : {slider.sliderID}-- :{slider.title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                return RedirectToAction("Index");
            }

            return View();

        }
        // GET: Sliders/Edit/5

        public ActionResult Edit(int id)
        {

            var slider = db.Sliders.Where(x => x.sliderID == id).SingleOrDefault();
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Slider slider,  HttpPostedFileBase pictureURL)
        {
            if (ModelState.IsValid)
            {
                var tmp = db.Sliders.Where(x => x.sliderID == id).SingleOrDefault();

                if (pictureURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(tmp.pictureURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(tmp.pictureURL));
                    }
                    WebImage img = new WebImage(pictureURL.InputStream);
                    FileInfo imginfo = new FileInfo(pictureURL.FileName);

                    string picid = pictureURL.FileName + imginfo.Extension;
                    img.Resize(1024, 360);
                    img.Save("~/Uploads/Sliders/" + picid);

                    tmp.pictureURL = "/Uploads/Sliders/" + picid;
                }
                tmp.title = slider.title;
                tmp.description = slider.description;
                logger.Info($" AN ADMIN EDIT  SLIDERS : {slider.sliderID}-- :{slider.title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slider);

        }

        // GET: Sliders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.Sliders.Find(id);
            logger.Info($" AN ADMIN DELETE  SLIDERS : {slider.sliderID}-- :{slider.title}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            db.Sliders.Remove(slider);
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
