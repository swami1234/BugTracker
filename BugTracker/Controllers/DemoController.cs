using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        public ActionResult LandingPage()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LandingPage(string key)
        //{
        //    var demoEmail = WebConfigurationManager.AppSettings[key];
        //    var demoUserPassword = WebConfigurationManager.AppSettings["DemoUserPassword"]
        //}
            
    }
}