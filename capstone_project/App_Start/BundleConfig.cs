﻿using System.Web;
using System.Web.Optimization;

namespace capstone_project
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                   
                      "~/Scripts/jquery-3.3.1.min.js",
                     
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/bootstrap.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/solid").Include(
              
                      "~/Scripts/solid/solid.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap4.css",
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/jquery.mCustomScrollbar.min.css",
                      "~/Content/main.css",
                      "~/Content/util.css",
                      "~/Content/custom.css",
                      "~/Content/Raleway.css"
                      ));
        }
    }
}
