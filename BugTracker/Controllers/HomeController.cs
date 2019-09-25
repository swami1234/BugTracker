using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult Register(string returnUrl)
        {

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult DemoDashboard()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();
            ViewBag.Message = "Your Contact Page";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel email)
        {


            var from = $"{email.FromEmail}<{WebConfigurationManager.AppSettings["emailfrom"]}>"; // string interpolation
            var emailMessage = new MailMessage(from, WebConfigurationManager.AppSettings["emailto"])
            {
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = true
            };

            var svc = new PersonalEmail();
            await svc.SendAsync(emailMessage);


            return RedirectToAction("ThankYou");
        }

        public ActionResult ThankYou()
        {

            return View();
        }

    }
}