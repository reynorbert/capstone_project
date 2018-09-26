using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using capstone_project.Models;

namespace capstone_project.Views
{
    public class tbl_accountsController : Controller
    {
        private capstone_mwdEntities db = new capstone_mwdEntities();

        // GET: tbl_accounts
        public ActionResult Index()
        {
            var tbl_accounts = db.tbl_accounts.Include(t => t.tbl_companies);
            return View(tbl_accounts.ToList());
        }

        // GET: tbl_accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_accounts tbl_accounts = db.tbl_accounts.Find(id);
            if (tbl_accounts == null)
            {
                return HttpNotFound();
            }
            return View(tbl_accounts);
        }

        // GET: tbl_accounts/Create
        public ActionResult Create()
        {
            ViewBag.company_id = new SelectList(db.tbl_companies, "company_id", "company_name");
            return View();
        }

        // POST: tbl_accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "account_id,account_email,account_status,account_password,account_type,account_img,account_bankName,account_bankNum,company_id")] tbl_accounts tbl_accounts)
        {
            if (ModelState.IsValid)
            {
                db.tbl_accounts.Add(tbl_accounts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.company_id = new SelectList(db.tbl_companies, "company_id", "company_name", tbl_accounts.company_id);
            return View(tbl_accounts);
        }

        // GET: tbl_accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_accounts tbl_accounts = db.tbl_accounts.Find(id);
            if (tbl_accounts == null)
            {
                return HttpNotFound();
            }
            ViewBag.company_id = new SelectList(db.tbl_companies, "company_id", "company_name", tbl_accounts.company_id);
            return View(tbl_accounts);
        }

        // POST: tbl_accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "account_id,account_email,account_status,account_password,account_type,account_img,account_bankName,account_bankNum,company_id")] tbl_accounts tbl_accounts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_accounts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.company_id = new SelectList(db.tbl_companies, "company_id", "company_name", tbl_accounts.company_id);
            return View(tbl_accounts);
        }

        // GET: tbl_accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_accounts tbl_accounts = db.tbl_accounts.Find(id);
            if (tbl_accounts == null)
            {
                return HttpNotFound();
            }
            return View(tbl_accounts);
        }

        // POST: tbl_accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_accounts tbl_accounts = db.tbl_accounts.Find(id);
            db.tbl_accounts.Remove(tbl_accounts);
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
