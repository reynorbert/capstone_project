using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using capstone_project.Models;
using System.IO;

namespace capstone_project.Controllers
{
    public class VendorController : Controller
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
            ViewBag.Message = "Your contact page.";

            return View();
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
    }
}