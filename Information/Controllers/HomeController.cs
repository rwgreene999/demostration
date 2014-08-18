using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Information.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to my home page.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Robert's Demo.";
            var date = System.IO.File.GetCreationTime(System.Reflection.Assembly.GetExecutingAssembly().Location);

            ViewBag.BuildDate = date.ToString(); 

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
