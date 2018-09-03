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
            return View();
        }
    }
}