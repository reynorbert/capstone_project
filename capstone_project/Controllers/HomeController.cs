using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}