using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helper
{
    public class Utilities
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static string MakeReadable(string property, string value)
        {
            switch (property)
            {
                case "TicketStatutsId":
                    return db.TicketStatuses.Find(Convert.ToInt32(value)).Name;
                case "TicketPriorityId":
                    return db.TicketPriorities.Find(Convert.ToInt32(value)).Name;
                case "TicketTypeId":
                    return db.TicketTypes.Find(Convert.ToInt32(value)).Name;
                case "AssignedToUserId":
                    return !string.IsNullOrEmpty(value) ? db.Users.Find(value).FullName : "--UnAssigned--";
                default:
                    return value;



            }
        }
            


    }
}