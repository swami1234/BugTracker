
using BugTracker.Helper;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;



namespace BugTracker.Helper

{

    public class TicketHelper : CommonHelper

    {
       



        internal object ListAssignedTickets(object getuserId)

        {

            throw new NotImplementedException();

        }



        public ICollection<Ticket> ListTicketsOnProject(string projectId)

        {

            var project = db.Projects.Find(projectId);



            var list = project.Tickets.ToList();



            return list;

        }



        public ICollection<Ticket> GetTicketsOwned(string userId)

        {



            var list = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();



            return list;

        }



        public string GetAssignedToUserId(int ticketId)

        {

           return db.Tickets.Find(ticketId).AssignedToUserId;
           

        }



        public string GetTicketOwner(int ticketId)

        {

            return db.Tickets.Find(ticketId).OwnerUserId;

        }



        public ICollection<Ticket> ListAssignedTickets(string userId)

        {

            var user = db.Users.Find(userId);



            var tickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();

            return tickets;

        }



        public ICollection<Ticket> ListTicketsOwned(string userId)

        {

            var user = db.Users.Find(userId);



            var tickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();



            return tickets;

        }

        public static int CountListTicketsOwned()

        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();



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
            return myTickets.Count();

        }


        public ICollection<Ticket> ListTickets()

        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();



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
            return myTickets;

        }


        public ICollection<Ticket> GetOpenTickets(int projectId)

        {

            var project = db.Projects.Find(projectId);

            var openTickets = project.Tickets.Where(t => t.TicketStatus.Name == "Open");



            return openTickets.ToList();

        }



        public ICollection<Ticket> GetClosedTickets(int projectId)

        {

            var project = db.Projects.Find(projectId);

            var closedTickets = project.Tickets.Where(t => t.TicketStatus.Name == "Resolved");



            return closedTickets.ToList();

        }



        public ICollection<Ticket> GetUnassignedTickets(int projectId)

        {

            var project = db.Projects.Find(projectId);

            var unassignedTickets = project.Tickets.Where(t => t.TicketStatus.Name == "Unassigned");



            return unassignedTickets.ToList();

        }



        public ICollection<Ticket> GetInfoNeededTickets(int projectId)

        {

            var project = db.Projects.Find(projectId);

            var tickets = project.Tickets.Where(t => t.TicketStatus.Name == "Need More Info");



            return tickets.ToList();

        }



        public ICollection<Ticket> GetUrgentTickets(int projectId)

        {

            var project = db.Projects.Find(projectId);

            var urgentTickets = project.Tickets.Where(t => t.TicketPriority.Name == "Urgent" || t.TicketPriority.Name == "High");



            return urgentTickets.ToList();

        }

        public ICollection<Ticket> GetMyUrgentTickets()

        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();



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

            var urgentTickets = myTickets.Where(t => t.TicketPriority.Name == "Urgent" || t.TicketPriority.Name == "High");

            return urgentTickets.ToList();

        }

        public static int GetMyUrgentTicketsCount()

        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();



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

            var urgentTickets = myTickets.Where(t => t.TicketPriority.Name == "Urgent" || t.TicketPriority.Name == "High");

            return urgentTickets.Count();

        }

        public static int GetMyHighTicketsCount()

        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();



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

            var highTickets = myTickets.Where(t => t.TicketPriority.Name == "High");

            return highTickets.Count();

        }

        public bool IsUserTicketOwner(string userId, int ticketId)

        {

            var ticket = db.Tickets.Find(ticketId);

            if (ticket.OwnerUserId != userId || ticket.OwnerUserId == null)

            {

                return false;

            }

            else

            {

                return true;

            }

        }



        public bool IsUserAssignedToTicket(string userId, int ticketId)

        {

            var ticket = db.Tickets.Find(ticketId);

            if (ticket.AssignedToUserId != userId || ticket.AssignedToUserId == null)

            {

                return false;

            }

            else

            {

                return true;

            }

        }



        //public async Task AssignUserToTicket(string userId, int ticketId)

        //{

        //    if (!IsUserAssignedToTicket(userId, ticketId))

        //    {

        //        var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);

        //        var ticket = db.Tickets.Find(ticketId);

        //        ticket.AssignedToUserId = userId;

        //        db.Entry(ticket).Property(t => t.AssignedToUserId).IsModified = true;

        //        ChangeTicketStatus(ticketId, "Open");

        //        if (!HttpContext.Current.User.IsInRole("Demo"))

        //        {

        //            db.SaveChanges();

        //           // await notifyHelper.Notify(oldTicket, ticket);

        //           // historyHelper.AddHistories(oldTicket, ticket);

        //        };

        //    }

        //}



        public void ChangeTicketStatus(int ticketId, string statusName)

        {

            var ticket = db.Tickets.Find(ticketId);

            ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(s => s.Name == statusName).Id;

            db.Tickets.Attach(ticket);

            db.Entry(ticket).Property(t => t.TicketStatusId).IsModified = true;

            try

            {

                db.SaveChanges();

            }

            catch (DbEntityValidationException e)

            {

                foreach (var eve in e.EntityValidationErrors)

                {

                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",

                        eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors)

                    {

                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",

                            ve.PropertyName, ve.ErrorMessage);

                    }

                }

                throw;

            }

            db.SaveChanges();

        }


       


        //public async Task UnassignUserFromTicket(int ticketId, string userId)

        //{

        //    var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);

        //    var ticket = db.Tickets.Find(ticketId);

        //    ticket.AssignedUserId = null;

        //    db.Entry(ticket).Property(t => t.AssignedUserId).IsModified = true;

        //    ChangeTicketStatus(ticket.Id, "Unassigned");

        //    if (!HttpContext.Current.User.IsInRole("Demo"))

        //    {

        //        await notifyHelper.Notify(oldTicket, ticket);

        //        historyHelper.AddHistories(oldTicket, ticket);

        //        db.SaveChanges();

        //    }



        //}

    }

}



