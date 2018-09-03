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
    public class AccountsController : Controller
    {
        private capstone_mwdEntities db = new capstone_mwdEntities();

        // GET: tbl_accounts
        public ActionResult Index()
        {
            var tbl_accounts = db.tbl_accounts.Include(t => t.tbl_companies);
            return View(tbl_accounts.ToList());
        }

        // GET: tbl_ccounts/Details/5
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
        public ActionResult Create([Bind(Include = "account_id,account_email,account_status,account_password,account_type,company_id")] tbl_accounts tbl_accounts)
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
        public ActionResult Edit([Bind(Include = "account_id,account_email,account_status,account_password,acoununt_type,company_id")] tbl_accounts tbl_accounts)
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

        public ActionResult Login()
        {
            //var tbl_accounts = db.tbl_accounts.Include(t => t.tbl_companies);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "account_id,account_email,account_status,account_password,acoununt_type,company_id")] tbl_accounts tbl_accounts)
        {
            if (ModelState.IsValid)
            {
                db.tbl_accounts.Add(tbl_accounts);
                var account = db.tbl_accounts
                    .Where(table => table.account_email == tbl_accounts.account_email)
                    .Where(table => table.account_password == tbl_accounts.account_password)
                    .ToList();
                Session["Account_id"] = account[0].account_id;
                Session["Account_type"] = account[0].account_type;
                if(account[0].account_type == 1)
                {
                    return RedirectToAction("../Admin/Index");
                } else
                {
                    return RedirectToAction("Index");
                }
                
            }

            ViewBag.company_id = new SelectList(db.tbl_companies, "company_id", "company_name", tbl_accounts.company_id);
            return View(tbl_accounts);
        }


        // GET: tbl_accounts/Create
        public ActionResult Registration()
        {
            ViewBag.company_id = new SelectList(db.tbl_companies, "company_id", "company_name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "account_id,account_email,account_status,account_password,account_type,company_id")] tbl_accounts tbl_accounts)
        {
            tbl_accounts.account_type = int.Parse(Request.Form["account_type"]);
            tbl_accounts.account_status = 0;

            var a = new tbl_companies();
            a.company_name = Request.Form["tbl_companies.company_name"];
            a.company_address = Request.Form["tbl_companies.company_address"];

            if (ModelState.IsValid)
            {

                db.tbl_companies.Add(a);
                db.SaveChanges();
                tbl_accounts.company_id = db.tbl_companies.Max(u => u.company_id);
                db.tbl_accounts.Add(tbl_accounts);
                db.SaveChanges();
                return RedirectToAction("./../Home/");
            }

            ViewBag.company_id = new SelectList(db.tbl_companies, "company_id", "company_name", tbl_accounts.company_id);
            return View(tbl_accounts);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("../Home/");
        }
    }
}
