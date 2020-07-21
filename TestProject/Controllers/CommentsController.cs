using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestProject.Models;

namespace TestProject.Controllers
{
    public class CommentsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ProjectDB db = new ProjectDB();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Blog).Include(c => c.User).OrderByDescending(x=>x.CommentID);
            logger.Info($" AN ADMIN SHOWS  => COMMENTS  NAME:{Session["UserName"]}---ID {Session["ID"]}");
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            logger.Info($" AN ADMIN SHOW DETAILS COMMENTS :COMMENT ID {comment.CommentID}-- BLOG ID:{comment.blogID}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.CommentID = new SelectList(db.Blogs, "blogID", "title");
            ViewBag.CommentID = new SelectList(db.Users, "ID", "Name");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentID,UserName,Email,Comment1,blogID,UserID,validation")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CommentID = new SelectList(db.Blogs, "blogID", "title", comment.CommentID);
            ViewBag.CommentID = new SelectList(db.Users, "ID", "Name", comment.CommentID);
            logger.Info($" AN ADMIN CREATE COMMENTS :COMMENT ID {comment.CommentID}-- BLOG ID:{comment.blogID}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CommentID = new SelectList(db.Blogs, "blogID", "title", comment.CommentID);
            ViewBag.CommentID = new SelectList(db.Users, "ID", "Name", comment.CommentID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,UserName,Email,Comment1,blogID,UserID,validation")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CommentID = new SelectList(db.Blogs, "blogID", "title", comment.CommentID);
            ViewBag.CommentID = new SelectList(db.Users, "ID", "Name", comment.CommentID);
            logger.Info($" AN ADMIN EDIT COMMENTS :COMMENT ID {comment.CommentID}-- BLOG ID:{comment.blogID}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            logger.Info($" AN ADMIN DELETE COMMENTS :COMMENT ID {comment.CommentID}-- BLOG ID:{comment.blogID}--------ADMIN :{Session["UserName"]}--ID:{Session["ID"]}");
            db.Comments.Remove(comment);
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
