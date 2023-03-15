using Dropzonedemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Dropzonedemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string str=null)
        {
            var postedUsername = Request.Form["Username"].ToString();
            foreach (var imageFile in Request.Form.Files)
            {

            }

            return Json(new { status = true, Message = "Account created." });
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadPost()
        {
            var postedUsername = Request.Form["fileName"].ToString();


            //string fName = postedUsername.FileName;
            //if (file != null && file.ContentLength > 0)
            //{ }

            return Json(new { status = true, Message = "file uploaded." });

        }

        public ActionResult FileUpload()
        {
            return View();
        }

        public ActionResult JQueryFileUpload()
        {
            return View();

        }

        public ActionResult MsExport()
        {
            return View();
        }
   
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
