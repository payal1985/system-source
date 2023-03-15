using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExcelExportDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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

        public ActionResult TestExportExcel()
        {
            return View();
        }

        public ActionResult TestExport()
        {
            return View();
        }

        public ActionResult MsExport()
        {
            return View();
        }

        public ActionResult TestOrderExport()
        {
            return View();
        }
        public ActionResult TestOrderExportExcel()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}