using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helper;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Admin, Developer, Project Manager, Submitter")]
    [RequireHttps]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        // GET: Tickets
        public ActionResult Index()
        {

            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        [Authorize(Roles = "Project Manager,Developer,Submitter")] //MyIndex wants to fill some view with my                                                              tickets only

        public ActionResult MyIndex()
        {
            
            var userId = User.Identity.GetUserId();//First Get the Id of the logged in user

            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault(); //then get the roles they occupy
                                                                            //firstordefault Returns the first                   element of a sequence, or a default value if no element is found

            var myTickets = new List<Ticket>();
            
            switch (myRole)//then based on the role name - push different data into the view
            {
                case "Developer":
                    myTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                    break;

                case "Submitter":
                    myTickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    break;

                case "Project Manager":
                    myTickets = db.Users.Find(userId).Projects.SelectMany(t => t.Tickets).ToList();
                    break;

            }
            return View(myTickets);
        }



    
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult AssignTicket(int? id)
        {
            UserRolesHelper helper = new UserRolesHelper();
            var ticket = db.Tickets.Find(id);
            var users = helper.UsersInRole("Developer").ToList();
            ViewBag.AssignedToUserId = new SelectList(users, "Id", "FullName");
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignTicket(Ticket model)
        {
            var ticket = db.Tickets.Find(model.Id);

            ticket.AssignedToUserId = model.AssignedToUserId;
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
            db.SaveChanges();

            NotificationHelper.CreateAssignmentNotification(oldTicket, ticket);
            
            HistoryHelper.CreateHistoryRecord(oldTicket, ticket);

            var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id },
                protocol: Request.Url.Scheme);
            try
            {
                EmailService ems = new EmailService();
                IdentityMessage msg = new IdentityMessage();
                ApplicationUser user = db.Users.Find(model.AssignedToUserId);
                msg.Body = "You have been assigned to a new Ticket." + Environment.NewLine +
                    "please click the following link to view details" +
                    "<a href=\"" + callbackUrl + "\">NEW TICKET</a>";
                msg.Destination = user.Email;
                msg.Subject = "Invite to Household";
                await ems.SendAsync(msg);
            }
            catch (Exception )
            {
                await Task.FromResult(0);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Dashboard(int Id)
        {
            var ticket = db.Tickets.ToList();
            return View(ticket);
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var project = ticket.Project;
            //ViewBag.TeamPMs = projectHelper.UsersInRoleOnProject(project.Id, SystemRole.ProjectManager).ToList();
            //ViewBag.TeamSubs = projectHelper.UsersInRoleOnProject(project.Id, SystemRole.Submitter).ToList();
            //ViewBag.TeamDevs = projectHelper.UsersInRoleOnProject(project.Id, SystemRole.Developer).ToList();
            ViewBag.History = ticket.TicketHistories;
            ViewBag.Notification = ticket.TicketNotifications;
            //ViewBag.CurrentUser = User.Identity.GetUserId();
            //ViewBag.DevProjects = projectHelper.ListUserProjects(ticket.AssignedToUserId);
            return View(ticket);
        }
        
        // GET: Tickets/Create
        [Authorize(Roles ="Submitter")]
        public ActionResult Create()
        {
            // This code is used to pop a sweet alert if anyone other than submitter tries to create a ticket. We are just omitting this code as we are using it in a different way and just not showing the link "create new" if its anyone other than submitter. 
            //if (!TicketDecisionHelper.TicketIsCreatedByUser())
            //{
            //    TempData["Message"] = "You are not authorized to create ticket based on your assigned role";

            //    return RedirectToAction("Index", "Tickets");
            //}

            var myProjects = projectHelper.ListUserProjects(User.Identity.GetUserId());
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");
            //var ticketstatus = db.TicketStatuses.Where(t => t.Name == " UnAssigned");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");

            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,TicketTypeId,TicketPriorityId,Title,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTime.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.TicketStatusId = db.TicketStatuses.AsNoTracking().FirstOrDefault(t => t.Name == "New UnAssigned").Id;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }




            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");

            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles ="Developer,Submitter,Project Manager,Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

          

            if (!TicketDecisionHelper.TicketIsEditableByUser(ticket))
            {
                TempData["Message"] = "You are not authorized to edit TicketId" + ticket.Id + "based on your assigned role";

                    return RedirectToAction("Index", "Tickets");
            }



            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
         
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,OwnerUserId,AssignedToUserId,Title,Description,Created,Updated")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //Go out to the database and get a copy of the ticket before it is changed
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                db.Entry(ticket).State = EntityState.Modified;
                ticket.Updated = DateTime.Now;
                db.SaveChanges();

                //Call the NotificationHelper to determine if a Notification needs to be created

                NotificationHelper.CreateAssignmentNotification(oldTicket, ticket);
                NotificationHelper.CreateChangeNotification(oldTicket, ticket);
                // NotificationHelper.ManageNotifications(oldTicket, ticket);

                //HistoryHelper
                HistoryHelper.CreateHistoryRecord(oldTicket, ticket);

                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Developer,Submitter,Project Manager,Admin")]
        public ActionResult Delete(int? id)
        {
           
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
           Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            if (!TicketDecisionHelper.TicketIsdeletableByUser())
            {
                TempData["Message"] = "You are not authorized to delete TicketId" + ticket.Id + "based on your assigned role";

                return RedirectToAction("Index", "Tickets");
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
