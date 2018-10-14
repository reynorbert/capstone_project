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

namespace capstone_project.Controllers
{
    public class HomeController : Controller
    {
        private capstone_mwdEntities db = new capstone_mwdEntities();

        public ActionResult Index()
        {
            string input = Request.Form["search-input"];
            var x = input == null || input == "" ? db.tbl_products.ToList() : db.tbl_products.Where(i => i.product_name.Contains(input) || i.product_desc.Contains(input)).ToList();
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

      

    }
}