using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using capstone_project.Models;

namespace capstone_project.Controllers
{
    public class DistributorController : Controller
    {
        private capstone_mwdEntities db = new capstone_mwdEntities();
        // GET: Admin
        public ActionResult Index()
        {

            var x = db.tbl_products.ToList();
            return View(x);
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
            ViewBag.Message = "Your contact page.";

            return View();
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

            tbl_products prod = new tbl_products();
            prod.product_name = name;
            prod.product_price = int.Parse(price);
            prod.product_desc = desc;
            prod.product_quantity = int.Parse(quantity);
            prod.prod_img = words[2];
            prod.product_owner = int.Parse(Session["Account_id"].ToString());
            db.tbl_products.Add(prod);
            db.SaveChanges();

            var y = db.tbl_products.OrderByDescending(u => u.product_id).FirstOrDefault().product_id;

            string target = @"C:\Users\rey.norbert.besmonte\Documents\Visual Studio 2017\Projects\capstone_project\capstone_project\images\products\" + y;


            string sourceFile = System.IO.Path.Combine(sourcePath, words[2]);
            string destFile = System.IO.Path.Combine(target, words[2]);


            if (!System.IO.Directory.Exists(target))
            {
                System.IO.Directory.CreateDirectory(target);
            }

            System.IO.File.Copy(sourceFile, destFile, true);
        }
    }
}