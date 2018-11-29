using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using capstone_project.Models;
using System.Configuration;
using Stripe;

namespace capstone_project.Views
{
    public class UserController : Controller
    {
        private capstone_mwdEntities db = new capstone_mwdEntities();

        // GET: User
        public ActionResult Index()
        {
            try
            {
                string account = Session["Account_id"].ToString();


                string input = Request.Form["search-input"];
                var x = input == null || input == "" ? db.tbl_products.Where(y => y.product_owner.ToString() != account).ToList() : db.tbl_products.Where(i => i.product_name.Contains(input) || i.product_desc.Contains(input)).Where(y => y.product_owner.ToString() != account).ToList();
                return View(x);
            }
            catch
            {
                string account = Session["Account_id"].ToString();
                string input = Request.Form["search-input"];
                var x = input == null || input == "" ? db.tbl_products.Where(y => y.product_owner.ToString() != account).ToList() : db.tbl_products.Where(i => i.product_name.Contains(input) || i.product_desc.Contains(input)).Where(y => y.product_owner.ToString() != account).ToList();
                return View(x);
            }

        }

        public ActionResult records()
        {
            return View();
        }

        public ActionResult profile()
        {
            int buyer = int.Parse(Session["Account_id"].ToString());
            var account = db.tbl_accounts.Where(x => x.account_id == buyer).ToList();
            ViewBag.account = db.tbl_accounts.Where(x => x.account_id == buyer).ToList();
            return View(account);
        }


        // GET: User/Create
        public ActionResult Create()
        {
            ViewBag.company_id = new SelectList(db.tbl_companies, "company_id", "company_name");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "account_id,account_email,account_status,account_password,account_type,account_img,company_id")] tbl_accounts tbl_accounts)
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

        // GET: User/Edit/5
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

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "account_id,account_email,account_status,account_password,account_type,account_img,company_id")] tbl_accounts tbl_accounts)
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

        // GET: User/Delete/5
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

        // POST: User/Delete/5
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }//

        public ActionResult payment_success()
        {
          
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult faq()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_products tbl_products = db.tbl_products.Find(id);
            ViewBag.products = db.tbl_products.ToList();
            if (tbl_products == null)
            {
                return HttpNotFound();
            }
            return View(tbl_products);
        }

        public ActionResult cart()
        {
            int buyer = int.Parse(Session["Account_id"].ToString());
            var cart = db.tbl_cart.Where(c => c.tbl_transactions.trans_status == "cart").Where(c => c.tbl_transactions.trans_buyer == buyer).ToList();

            ViewBag.sum = db.tbl_cart.Where(c => c.tbl_transactions.trans_status == "cart").Where(c => c.tbl_transactions.trans_buyer == buyer).Sum(c => c.tbl_products.product_price * c.cart_quantity);
            ViewBag.sumTax = db.tbl_cart.Where(c => c.tbl_transactions.trans_status == "cart").Where(c => c.tbl_transactions.trans_buyer == buyer).Sum(c => c.tbl_products.product_price * c.cart_quantity) + 500;
            return View(cart);
        }


        [Route("add_cart")]
        [HttpPost]
        public void add_cart(string prodId, int quantity)
        {
            tbl_transactions trans = new tbl_transactions();
            tbl_cart cart = new tbl_cart();
            int buyer = int.Parse(Session["Account_id"].ToString());

            var transCount = db.tbl_transactions.Where(t => t.trans_status == "cart").Where(t => t.trans_buyer == buyer).ToList();
            tbl_transactions obj_trans = new tbl_transactions();
            tbl_cart obj_cart = new tbl_cart();

            if (transCount.Count > 0)
            {
                var transCount2 = db.tbl_transactions.Where(t => t.trans_status == "cart").Where(t => t.trans_buyer == buyer).OrderByDescending(t => t.trans_id).FirstOrDefault();


                obj_cart.trans_id = transCount2.trans_id;
                obj_cart.product_id = int.Parse(prodId);
                obj_cart.cart_quantity = quantity;
                db.tbl_cart.Add(obj_cart);
                db.SaveChanges();

            }
            else
            {
                //obj_trans.trans_product = int.Parse(prodId);
                obj_trans.trans_buyer = buyer;
                obj_trans.trans_status = "cart";

                db.tbl_transactions.Add(obj_trans);
                db.SaveChanges();

                var transCount2 = db.tbl_transactions.Where(t => t.trans_status == "cart").Where(t => t.trans_buyer == buyer).OrderByDescending(t => t.trans_id).FirstOrDefault();


                obj_cart.trans_id = transCount2.trans_id;
                obj_cart.product_id = int.Parse(prodId);
                obj_cart.cart_quantity = quantity;
                db.tbl_cart.Add(obj_cart);
                db.SaveChanges();

            }

            //tbl_accounts tbl_accounts = db.tbl_accounts.Find(int.Parse(id));
            //tbl_accounts.account_status = 2;
            //db.SaveChanges();

        }


        [Route("remove_cart")]
        [HttpPost]
        public void remove_cart(string cart)
        {
            tbl_cart obj_cart = db.tbl_cart.Find(int.Parse(cart));
            db.tbl_cart.Remove(obj_cart);
            db.SaveChanges();

        }

        public ActionResult Charge(string stripeEmail, string stripeToken)
        {
            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();

            var customer = customers.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            var charge = charges.Create(new StripeChargeCreateOptions
            {
                Amount = int.Parse(Request.Form["stripeAmount"]),//charge in cents
                Currency = "usd",
                CustomerId = customer.Id
            });

            // further application specific code goes here
            pay_now();
            return RedirectToAction("payment_success");
        }

        public ActionResult check_out()
        {
            
            var stripePublishKey = "pk_test_NLldSQJtWpv4aufQyoQ4Jfqw";
            ViewBag.StripePublishKey = stripePublishKey;

            int buyer = int.Parse(Session["Account_id"].ToString());
            var cart = db.tbl_cart.Where(c => c.tbl_transactions.trans_status == "cart").Where(c => c.tbl_transactions.trans_buyer == buyer).ToList();

            ViewBag.sum = db.tbl_cart.Where(c => c.tbl_transactions.trans_status == "cart").Where(c => c.tbl_transactions.trans_buyer == buyer).Sum(c => c.tbl_products.product_price * c.cart_quantity);
            ViewBag.sumTax = db.tbl_cart.Where(c => c.tbl_transactions.trans_status == "cart").Where(c => c.tbl_transactions.trans_buyer == buyer).Sum(c => c.tbl_products.product_price * c.cart_quantity) + 500;
            ViewBag.sumTaxStripe = ViewBag.sumTax * 100;
            return View(cart);
        }

        

        [Route("verify")]
        [HttpPost]
        public double verify(string disc_code)
        {

            int buyer = int.Parse(Session["Account_id"].ToString());
            double discount = Convert.ToDouble(db.tbl_discounts.Where(x => x.discount_code == disc_code).FirstOrDefault().discount_amount);
        
            return discount;

        }
        [Route("deliver")]
        [HttpPost]
        public void deliver(string disc_code)
        {

            int buyer = int.Parse(Session["Account_id"].ToString());
            var cart = db.tbl_cart.Where(c => c.tbl_transactions.trans_status == "cart").Where(c => c.tbl_transactions.trans_buyer == buyer).FirstOrDefault();

            double discount = Convert.ToDouble(db.tbl_discounts.Where(x => x.discount_code == disc_code).FirstOrDefault().discount_amount);

            tbl_transactions obj_trans = db.tbl_transactions.Find(cart.trans_id);
            obj_trans.trans_status = "For Delivery";
            obj_trans.trans_discount = discount;
            obj_trans.trans_date = DateTime.Now;

            db.SaveChanges();

        }

        [Route("pay_now")]
        [HttpPost]
        public void pay_now()
        {
  
            int buyer = int.Parse(Session["Account_id"].ToString());
            var cart = db.tbl_cart.Where(c => c.tbl_transactions.trans_status == "cart").Where(c => c.tbl_transactions.trans_buyer == buyer).FirstOrDefault();

            tbl_transactions obj_trans = db.tbl_transactions.Find(cart.trans_id);
            obj_trans.trans_status = "paid";
            obj_trans.trans_date = DateTime.Now;

            db.SaveChanges();

        }

        public ActionResult history()
        {
            int buyer = int.Parse(Session["Account_id"].ToString());
            var record = db.tbl_transactions.Where(x => x.trans_buyer == buyer).Where(x => x.trans_status != "cart").ToList();

            ViewBag.cart = db.tbl_cart.Where(y => y.tbl_transactions.trans_buyer == buyer).ToList();
            return View(record);
        }

        public ActionResult inquiry()
        {
            int from = int.Parse(Session["Account_id"].ToString());
            var x = db.tbl_inquiries.Where(y => y.inq_from == from || y.inq_to == from).OrderBy(a => a.thread_id).ToList();
            return View(x);
        }

        public ActionResult new_inquiry()
        {
            int from = int.Parse(Session["Account_id"].ToString());
            var obj = db.tbl_accounts.Where(x => x.account_type != 3).Where(x => x.account_id != from).ToList();
            var dist = obj.Distinct();
            return View(dist);
        }


        public ActionResult send_new()
        {
            int from = int.Parse(Session["Account_id"].ToString());
            tbl_threads thread = new tbl_threads();
            thread.thread_title = Request.Form["title"];
            db.tbl_threads.Add(thread);
            db.SaveChanges();

            var thread_id = db.tbl_threads.OrderByDescending(x => x.thread_id).FirstOrDefault().thread_id;

            tbl_inquiries inq = new tbl_inquiries();
            inq.thread_id = thread_id;
            inq.inq_content = Request.Form["message"];
            inq.inq_date = DateTime.Now;
            inq.inq_from = from;
            inq.inq_to = int.Parse(Request.Form["account_dest"]);
            db.tbl_inquiries.Add(inq);
            db.SaveChanges();
            return RedirectToAction("inquiry");
        }

        public ActionResult reply()
        {

            int from = int.Parse(Session["Account_id"].ToString());
            tbl_inquiries inq = new tbl_inquiries();
            inq.thread_id = int.Parse(Request.Form["id"]); 
            inq.inq_content = Request.Form["message"];
            inq.inq_date = DateTime.Now;
            inq.inq_from = from;
            inq.inq_to = int.Parse(Request.Form["to"]);

            db.tbl_inquiries.Add(inq);
            db.SaveChanges();
            return Redirect("thread?id=" + Request.Form["id"]);
        }

        public ActionResult thread(int? id)
        {
            ViewBag.title = db.tbl_threads.Where(a => a.thread_id == id).FirstOrDefault().thread_title;
            ViewBag.to = db.tbl_inquiries.Where(b => b.thread_id == id).FirstOrDefault().inq_to;
            ViewBag.id = id;
            var x = db.tbl_inquiries.Where(a => a.thread_id == id).ToList();

            return View(x);
        }

    }
}
