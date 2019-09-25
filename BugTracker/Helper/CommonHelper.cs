using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helper
{
    public abstract class CommonHelper
    {
        protected static ApplicationDbContext db = new ApplicationDbContext();
        protected static UserRolesHelper roleHelper = new UserRolesHelper();
        protected static ProjectsHelper projectHelper = new ProjectsHelper();
        protected static TicketHelper ticketHelper = new TicketHelper();

        protected ApplicationUser CurrentUser = null;
        protected string CurrentRole = "";

        protected CommonHelper()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            CurrentUser = db.Users.Find(userId);
            CurrentRole = roleHelper.ListUserRoles(CurrentUser.Id).FirstOrDefault();
        }
    }
}