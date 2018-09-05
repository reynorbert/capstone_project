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
    public class TransactionsController : Controller
    {
        private capstone_mwdEntities db = new capstone_mwdEntities();

        // GET: Transactions
        public ActionResult Index()
        {
            var tbl_transactions = db.tbl_transactions.Include(t => t.tbl_accounts).Include(t => t.tbl_products);
            return View(tbl_transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_transactions tbl_transactions = db.tbl_transactions.Find(id);
            if (tbl_transactions == null)
            {
                return HttpNotFound();
            }
            return View(tbl_transactions);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.trans_buyer = new SelectList(db.tbl_accounts, "account_id", "account_email");
            ViewBag.trans_product = new SelectList(db.tbl_products, "product_id", "product_name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "trans_id,trans_date,trans_product,trans_buyer,trans_quantity,trans_discount")] tbl_transactions tbl_transactions)
        {
            if (ModelState.IsValid)
            {
                db.tbl_transactions.Add(tbl_transactions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.trans_buyer = new SelectList(db.tbl_accounts, "account_id", "account_email", tbl_transactions.trans_buyer);
            ViewBag.trans_product = new SelectList(db.tbl_products, "product_id", "product_name", tbl_transactions.trans_product);
            return View(tbl_transactions);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_transactions tbl_transactions = db.tbl_transactions.Find(id);
            if (tbl_transactions == null)
            {
                return HttpNotFound();
            }
            ViewBag.trans_buyer = new SelectList(db.tbl_accounts, "account_id", "account_email", tbl_transactions.trans_buyer);
            ViewBag.trans_product = new SelectList(db.tbl_products, "product_id", "product_name", tbl_transactions.trans_product);
            return View(tbl_transactions);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "trans_id,trans_date,trans_product,trans_buyer,trans_quantity,trans_discount")] tbl_transactions tbl_transactions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_transactions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.trans_buyer = new SelectList(db.tbl_accounts, "account_id", "account_email", tbl_transactions.trans_buyer);
            ViewBag.trans_product = new SelectList(db.tbl_products, "product_id", "product_name", tbl_transactions.trans_product);
            return View(tbl_transactions);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_transactions tbl_transactions = db.tbl_transactions.Find(id);
            if (tbl_transactions == null)
            {
                return HttpNotFound();
            }
            return View(tbl_transactions);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_transactions tbl_transactions = db.tbl_transactions.Find(id);
            db.tbl_transactions.Remove(tbl_transactions);
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
