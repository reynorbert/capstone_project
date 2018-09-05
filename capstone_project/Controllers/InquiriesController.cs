using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using capstone_project.Models;

namespace capstone_project.Controllers
{
    public class InquiriesController : Controller
    {
        private capstone_mwdEntities db = new capstone_mwdEntities();

        // GET: Inquiries
        public ActionResult Index()
        {
            var tbl_inquiries = db.tbl_inquiries.Include(t => t.tbl_accounts).Include(t => t.tbl_accounts1).Include(t => t.tbl_threads);
            return View(tbl_inquiries.ToList());
        }

        // GET: Inquiries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_inquiries tbl_inquiries = db.tbl_inquiries.Find(id);
            if (tbl_inquiries == null)
            {
                return HttpNotFound();
            }
            return View(tbl_inquiries);
        }

        // GET: Inquiries/Create
        public ActionResult Create()
        {
            ViewBag.inq_from = new SelectList(db.tbl_accounts, "account_id", "account_email");
            ViewBag.inq_to = new SelectList(db.tbl_accounts, "account_id", "account_email");
            ViewBag.thread_id = new SelectList(db.tbl_threads, "thread_id", "thread_title");
            return View();
        }

        // POST: Inquiries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "inq_id,inq_content,inq_date,inq_from,inq_to,thread_id")] tbl_inquiries tbl_inquiries)
        {
            if (ModelState.IsValid)
            {
                db.tbl_inquiries.Add(tbl_inquiries);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.inq_from = new SelectList(db.tbl_accounts, "account_id", "account_email", tbl_inquiries.inq_from);
            ViewBag.inq_to = new SelectList(db.tbl_accounts, "account_id", "account_email", tbl_inquiries.inq_to);
            ViewBag.thread_id = new SelectList(db.tbl_threads, "thread_id", "thread_title", tbl_inquiries.thread_id);
            return View(tbl_inquiries);
        }

        // GET: Inquiries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_inquiries tbl_inquiries = db.tbl_inquiries.Find(id);
            if (tbl_inquiries == null)
            {
                return HttpNotFound();
            }
            ViewBag.inq_from = new SelectList(db.tbl_accounts, "account_id", "account_email", tbl_inquiries.inq_from);
            ViewBag.inq_to = new SelectList(db.tbl_accounts, "account_id", "account_email", tbl_inquiries.inq_to);
            ViewBag.thread_id = new SelectList(db.tbl_threads, "thread_id", "thread_title", tbl_inquiries.thread_id);
            return View(tbl_inquiries);
        }

        // POST: Inquiries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "inq_id,inq_content,inq_date,inq_from,inq_to,thread_id")] tbl_inquiries tbl_inquiries)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_inquiries).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.inq_from = new SelectList(db.tbl_accounts, "account_id", "account_email", tbl_inquiries.inq_from);
            ViewBag.inq_to = new SelectList(db.tbl_accounts, "account_id", "account_email", tbl_inquiries.inq_to);
            ViewBag.thread_id = new SelectList(db.tbl_threads, "thread_id", "thread_title", tbl_inquiries.thread_id);
            return View(tbl_inquiries);
        }

        // GET: Inquiries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_inquiries tbl_inquiries = db.tbl_inquiries.Find(id);
            if (tbl_inquiries == null)
            {
                return HttpNotFound();
            }
            return View(tbl_inquiries);
        }

        // POST: Inquiries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_inquiries tbl_inquiries = db.tbl_inquiries.Find(id);
            db.tbl_inquiries.Remove(tbl_inquiries);
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
