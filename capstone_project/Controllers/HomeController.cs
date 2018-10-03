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
    public class HomeController : Controller
    {
        private capstone_mwdEntities db = new capstone_mwdEntities();
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