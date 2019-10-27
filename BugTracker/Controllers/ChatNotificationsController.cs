using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helper;
using BugTracker.Models;
using BugTracker.Signalr.hubs;
using Microsoft.AspNet.SignalR;

namespace BugTracker.Controllers
{
    public class ChatNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //private IHubContext<ChatHub> hubContext;
        public ApplicationDbContext _context { get; }

        private IHubContext<ChatHub> _hubContext;

        public ChatNotificationsController(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        
        // GET: ChatNotifications
        public ActionResult Index()
        {
            var chatNotifications = db.ChatNotifications.Include(c => c.Recipient).Include(c => c.Sender);
            return View(chatNotifications.ToList());
        }

        // GET: ChatNotifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatNotification chatNotification = db.ChatNotifications.Find(id);
            if (chatNotification == null)
            {
                return HttpNotFound();
            }
            return View(chatNotification);
        }

        // GET: ChatNotifications/Create
        public ActionResult Create()
        {
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.SenderId = new SelectList(db.Users, "Id", "FirstName");

           
            return View();
        }

        // POST: ChatNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ChatId,RecipientId,SenderId,Created,Subject,NotificationBody,Read")] ChatNotification chatNotification)
        {
            if (ModelState.IsValid)
            {
                db.ChatNotifications.Add(chatNotification);
                db.SaveChanges();

                
                //ChatNotificationHelper.ChatNotify(chatNotification);

                return RedirectToAction("Index");
            }

            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", chatNotification.RecipientId);
            ViewBag.SenderId = new SelectList(db.Users, "Id", "FirstName", chatNotification.SenderId);
            return View(chatNotification);
        }

        // GET: ChatNotifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatNotification chatNotification = db.ChatNotifications.Find(id);
            if (chatNotification == null)
            {
                return HttpNotFound();
            }
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", chatNotification.RecipientId);
            ViewBag.SenderId = new SelectList(db.Users, "Id", "FirstName", chatNotification.SenderId);
            return View(chatNotification);
        }

        // POST: ChatNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ChatId,RecipientId,SenderId,Created,Subject,NotificationBody,Read")] ChatNotification chatNotification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chatNotification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", chatNotification.RecipientId);
            ViewBag.SenderId = new SelectList(db.Users, "Id", "FirstName", chatNotification.SenderId);
            return View(chatNotification);
        }

        // GET: ChatNotifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatNotification chatNotification = db.ChatNotifications.Find(id);
            if (chatNotification == null)
            {
                return HttpNotFound();
            }
            return View(chatNotification);
        }

        // POST: ChatNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChatNotification chatNotification = db.ChatNotifications.Find(id);
            db.ChatNotifications.Remove(chatNotification);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
