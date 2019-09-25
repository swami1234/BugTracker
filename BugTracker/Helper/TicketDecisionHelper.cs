using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Controllers;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Helper
{
    public class TicketDecisionHelper : CommonHelper
    {
        public static bool TicketIsCreatedByUser()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Developer":
                    return false;

                case "Admin":
                    return false;

                case "Project Manager":
                    return false;

                case "Submitter":
                    return true;

                default:
                    return false;

            }
        }

       

        public static bool TicketIsEditableByUser(Ticket ticket)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Developer":
                    return ticket.AssignedToUserId == userId;

                case "Submitter":
                    return ticket.OwnerUserId == userId;

                case "Project Manager":
                    return db.Users.Find(userId).Projects.SelectMany(t => t.Tickets).Select(t => t.Id).Contains(ticket.Id);

                case "Admin":
                    return true;

                default:
                    return false;

            }
        }

        public static bool TicketIsdeletableByUser()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Developer":
                    return false;

                case "Admin":
                    return false;

                case "Project Manager":
                    return false;

                case "Submitter":
                    return true;

                default:
                    return false;

            }
        }
       
    }
}