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
    public class ProductsController : Controller
    {
        private capstone_mwdEntities db = new capstone_mwdEntities();

        // GET: Products
        public ActionResult Index()
        {
            var tbl_products = db.tbl_products.Include(t => t.tbl_accounts);
            return View(tbl_products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_products tbl_products = db.tbl_products.Find(id);
            if (tbl_products == null)
            {
                return HttpNotFound();
            }
            return View(tbl_products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.product_owner = new SelectList(db.tbl_accounts, "account_id", "account_email");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_id,product_name,product_desc,product_price,product_owner,product_quantity")] tbl_products tbl_products)
        {
            if (ModelState.IsValid)
            {
                db.tbl_products.Add(tbl_products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_owner = new SelectList(db.tbl_accounts, "account_id", "account_email", tbl_products.product_owner);
            return View(tbl_products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_products tbl_products = db.tbl_products.Find(id);
            if (tbl_products == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_owner = new SelectList(db.tbl_accounts, "account_id", "account_email", tbl_products.product_owner);
            return View(tbl_products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_id,product_name,product_desc,product_price,product_owner,product_quantity")] tbl_products tbl_products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_owner = new SelectList(db.tbl_accounts, "account_id", "account_email", tbl_products.product_owner);
            return View(tbl_products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_products tbl_products = db.tbl_products.Find(id);
            if (tbl_products == null)
            {
                return HttpNotFound();
            }
            return View(tbl_products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_products tbl_products = db.tbl_products.Find(id);
            db.tbl_products.Remove(tbl_products);
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
