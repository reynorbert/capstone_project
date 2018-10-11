using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using capstone_project.Models;

namespace capstone_project.Controllers
{
    public class AdminController : Controller
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

        public ActionResult accounts()
        {
            //string input = Request.Form["search-input"];
            //var x = input == null || input == "" ? db.tbl_products.Where(y => y.product_owner.ToString() != account).ToList() : db.tbl_products.Where(i => i.product_name.Contains(input) || i.product_desc.Contains(input)).Where(y => y.product_owner.ToString() != account).ToList();
            int account = int.Parse(Session["Account_id"].ToString());
            var x = db.tbl_accounts.Where(a => a.account_id != account).ToList();
            return View(x);
        }

        public ActionResult account_management(int? id)
        {
            var account = db.tbl_accounts.Where(x => x.account_id == id).ToList();
            ViewBag.account = db.tbl_accounts.Where(x => x.account_id == id).ToList();
            return View(account);
        }
        



        public ActionResult account_request()
        {
        
            var tbl_personal = db.tbl_personalInformations.Where(x => x.tbl_accounts.account_status == 0);
            return View(tbl_personal.ToList());

        }

        [Route("accept_user")]
        [HttpPost]
        public void accept_user(string id)
        {

            tbl_accounts tbl_accounts = db.tbl_accounts.Find(int.Parse(id));
            tbl_accounts.account_status = 1;
            db.SaveChanges();

        }

        [Route("reject_user")]
        [HttpPost]
        public void reject_user(string id)
        {

            tbl_accounts tbl_accounts = db.tbl_accounts.Find(int.Parse(id));
            tbl_accounts.account_status = 2;
            db.SaveChanges();

        }

        public ActionResult reports()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ads()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("create_ads")]
        [HttpPost]
        public void create_ads(string name, string start, string end, string url, string img)
        {
            string image;
            if (img == "")
            {
                image = @"\\upload.png";
            }
            else
            {
                image = img;
            }
            string[] words = image.Split('\\');
            string sourcePath = @"C:\imgs";

          

            tbl_ads ads = new tbl_ads();
            ads.ad_StartDate = start;
            ads.ad_EndDate = end;
            ads.ad_name = name;
            ads.ad_page = url;
            ads.ad_image = words[2];
            db.tbl_ads.Add(ads);
            db.SaveChanges();

            var y = db.tbl_ads.OrderByDescending(u => u.ad_id).FirstOrDefault().ad_id;

            string target = @"C:\capstone_project\capstone_project\images\accounts\ads\" + y;
   

            string sourceFile = System.IO.Path.Combine(sourcePath, words[2]);
            string destFile = System.IO.Path.Combine(target, words[2]);


            if (!System.IO.Directory.Exists(target))
            {
                System.IO.Directory.CreateDirectory(target);
            }

            System.IO.File.Copy(sourceFile, destFile, true);
        }

        [Route("get_ads")]
        [HttpPost]
        public object get_ads()
        {
            var ads = db.tbl_ads;
            return ads.ToList();
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