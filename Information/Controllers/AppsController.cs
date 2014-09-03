using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.UI;
using Information.Models; 

namespace Information.Controllers
{
    public class AppsController : Controller
    {
        public string GetQuoteServerLocation()
        {   // API currently in the same project , otherwise make it a config option or something 
            string s = Request.Url.AbsoluteUri.ToLower();
            string[] parts = s.Split(new string[] { "apps" }, StringSplitOptions.RemoveEmptyEntries);
            string result = parts[0] + @"api/";
            return result; 
        }
        public ActionResult Quote()
        {            
            ViewBag.QuoteServerLocation = GetQuoteServerLocation(); 
            ViewBag.Timeout = 10000;
            return View(); 
        }
        public ActionResult Demo()
        {
            return View();
        }
        public ActionResult QuoteA()
        {
            ViewBag.QuoteServerLocation = GetQuoteServerLocation();
            ViewBag.Timeout = 10000;
            return View();
        }
        public ActionResult QuoteB()
        {
            var serverlocation = GetQuoteServerLocation();
            ViewBag.QuoteServerLocation = serverlocation + "ManageQuotes"; 
            ViewBag.Timeout = 10000;
            return View();
        }
        public ActionResult GeolocationFromGoogle()
        {
            return View();
        }
        public ActionResult GeolocationFromMethod1()
        {
            return View();
        }
        public ActionResult MovieReviewDemo()
        {
            return View();
        }
        public ActionResult LongLat()
        {
            return View();
        }
        public ActionResult Admin()
        {
            ViewBag.QuoteServerLocation = GetQuoteServerLocation();
            ViewBag.Timeout = 10000;
            return View();
        }
    }
}
