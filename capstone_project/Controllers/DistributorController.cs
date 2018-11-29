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
using System.IO;

namespace capstone_project.Controllers
{
    public class DistributorController : Controller
    {
        private capstone_mwdEntities db = new capstone_mwdEntities();
        // GET: Admin
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

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

        public ActionResult profile()
        {
            int buyer = int.Parse(Session["Account_id"].ToString());
            var account = db.tbl_accounts.Where(x => x.account_id == buyer).ToList();
            ViewBag.account = db.tbl_accounts.Where(x => x.account_id == buyer).ToList();
            return View(account);
        }

        public ActionResult account_request()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult reports()
        {
            int seller = int.Parse(Session["Account_id"].ToString());
            var record = db.tbl_cart.Where(y => y.tbl_products.product_owner == seller).ToList();
            //db.tbl_transactions.Where(x => x.tbl_products.product_owner == seller).Where(x => x.trans_status != "cart").ToList();

            ViewBag.cart = db.tbl_cart.Where(y => y.tbl_products.product_owner == seller).ToList();
            return View(record);
        }

        public ActionResult add_item()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Route("execute_add")]
        [HttpPost]
        public void execute_add(string name, string price, string desc, string quantity, string img)
        {
            string image = img == "" ? @"\\upload.png" : img;
            string[] words = image.Split('\\');
      

            tbl_products prod = new tbl_products();
            prod.product_name = name;
            prod.product_price = int.Parse(price);
            prod.product_desc = desc;
            prod.product_quantity = int.Parse(quantity);
            prod.prod_img = words[2];
            prod.product_owner = int.Parse(Session["Account_id"].ToString());
            db.tbl_products.Add(prod);
            db.SaveChanges();
            
        }

        public JsonResult Upload()
        {
            var ctr = db.tbl_products.OrderByDescending(u => u.product_id).FirstOrDefault().product_id;

            string target = Server.MapPath(@"\images\products\" + ctr + @"\");

            if (!System.IO.Directory.Exists(target))
            {
                System.IO.Directory.CreateDirectory(target);
            }

            for (int i = 0; i < 1; i++)
            {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                                                            //Use the following properties to get file's name, size and MIMEType
                int fileSize = file.ContentLength;
                string fileName = file.FileName;
                string mimeType = file.ContentType;
                System.IO.Stream fileContent = file.InputStream;
                //To save file, use SaveAs method
                file.SaveAs(target + fileName); //File will be saved in application root
            }

            return Json("Uploaded " + Request.Files.Count + " files");
        }
        public ActionResult my_products()
        {
            try
            {
                string account = Session["Account_id"].ToString();
                var x = db.tbl_products.Where(y => y.product_owner.ToString() == account).ToList();
                return View(x);
            }
            catch
            {
                var x = db.tbl_products.ToList();
                return View(x);
            }
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

        public ActionResult payment_success()
        {

            return View();
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

        [Route("deliver")]
        [HttpPost]
        public void deliver()
        {

            int buyer = int.Parse(Session["Account_id"].ToString());
            var cart = db.tbl_cart.Where(c => c.tbl_transactions.trans_status == "cart").Where(c => c.tbl_transactions.trans_buyer == buyer).FirstOrDefault();

            tbl_transactions obj_trans = db.tbl_transactions.Find(cart.trans_id);
            obj_trans.trans_status = "For Delivery";
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
            obj_trans.trans_status = "payed";

            db.SaveChanges();

        }

        [Route("ChangeToPaid")]
        [HttpPost]
        public void ChangeToPaid(string trans_id)
        {
            tbl_transactions obj_trans = db.tbl_transactions.Find(int.Parse(trans_id));
            obj_trans.trans_status = "payed";
            db.SaveChanges();

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

        public ActionResult edit_item(int? id)
        {
            var item = db.tbl_products.Where(x => x.product_id == id).ToList();
            return View(item);
        }

        [Route("execute_edit")]
        [HttpPost]
        public void execute_edit(string name, string price, string desc, string quantity, string img, int id)
        {
            string image = img == "" ? @"\\upload.png" : img;

            tbl_products prod = db.tbl_products.Find(id);
            prod.product_name = name;
            prod.product_price = int.Parse(price);
            prod.product_desc = desc;
            prod.product_quantity = int.Parse(quantity);
            if (img != "")
            {
                string[] words = img.Split('\\');
                prod.prod_img = words[2];
            }

            db.SaveChanges();

        }

        public JsonResult EditImage(string id)
        {
            int user = int.Parse(id);
            var ctr = db.tbl_products.Find(user).product_id;

            //~/ images / products / @Html.DisplayFor(modelItem => item.product_id) /
            string targetPathProfilePic = Server.MapPath(@"\images\products\" + ctr + "\\");

            System.IO.DirectoryInfo di = new DirectoryInfo(targetPathProfilePic);

            if (System.IO.Directory.Exists(targetPathProfilePic))
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory(targetPathProfilePic);
            }

            for (int i = 0; i < 1; i++)
            {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file

                int fileSize = file.ContentLength;
                string fileName = file.FileName;
                string mimeType = file.ContentType;
                System.IO.Stream fileContent = file.InputStream;
                //To save file, use SaveAs method
                file.SaveAs(targetPathProfilePic + fileName); //File will be saved in application root
            }
            return Json("Uploaded " + Request.Files.Count + " files");
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