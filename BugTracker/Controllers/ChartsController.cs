using BugTracker.ChartViewModel;
using BugTracker.Helper;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        // GET: Charts
        public JsonResult GetRealChartData()
        {

            var data = new ChartJsBarData();

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

            foreach (var status in db.TicketStatuses)
            {
                data.Labels.Add(status.Name);
                data.Values.Add(myTickets.Where(t => t.TicketStatusId == status.Id).Count());
            }
            //var data = new ChartJsBarData();

            //foreach(var ticketStatus in db.TicketStatuses.ToList())
            //{
            //    var value = db.TicketStatuses.Find(ticketStatus.Id).Tickets.Count();

            //    data.Labels.Add(ticketStatus.Name);
            //    data.Values.Add(value);
            //}
            return Json(data);
        }

        public JsonResult GetRealChartData12()
        {
            var data = new ChartJsBarData();

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

            foreach (var priority in db.TicketPriorities)
            {
                data.Labels.Add(priority.Name);
                data.Values.Add(myTickets.Where(t => t.TicketPriorityId == priority.Id).Count());
            }

            //foreach (var ticket in myTickets)
            //{
            //    var value = db.TicketPriorities.Where(t => t.Name)

            //    data.Labels.Add(ticket.TicketPriority.Name);

            //    data.Values.Add(value);
            //}
            return Json(data);


        }
            //}

            //public JsonResult GetRealChartData12()
            //{
            //    var data = new ChartJsBarData();

            //    foreach (var ticketpriority in db.TicketPriorities.ToList())
            //    {
            //        var value = db.TicketPriorities.Find(ticketpriority.Id).Tickets.Count();

            //        data.Labels.Add(ticketpriority.Name);
            //        data.Values.Add(value);
            //    }
            //    return Json(data);
            //}


            public JsonResult GetRealChartData1()
        {
            var data = new ChartJsBarData();

            foreach (var project in db.Projects.ToList())
            {
                var value = db.Projects.Find(project.Id).Tickets.Count();

                data.Labels.Add(project.Name);
                data.Values.Add(value);
            }
            return Json(data);
        }
        
        public JsonResult GetRealChartDataMyProjects()
        {
            var data = new ChartJsBarData();

            var userId = User.Identity.GetUserId();

            var currentUser = db.Users.Find(userId);

            var myProjects = currentUser.Projects.ToList();

            foreach (var project in myProjects)
            {
                var value = myProjects.Count();

                data.Labels.Add(project.Name);
                data.Values.Add(value);
            }


            return Json(data);
        }

        public JsonResult GetRealChartDataMyTickets1()
        {
            var data = new ChartJsBarData();

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

            foreach (var type in db.TicketTypes)
            {
                data.Labels.Add(type.Name);
                data.Values.Add(myTickets.Where(t => t.TicketTypeId == type.Id).Count());
            }
            return Json(data);

        }

    }
}



          


