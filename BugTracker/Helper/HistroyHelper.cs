using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Helper
{
    public class HistoryHelper : CommonHelper
    {
        public static void CreateHistoryRecord(Ticket oldTicket, Ticket newTicket)
        {
            foreach (var property in newTicket.GetType().GetProperties())
            {

                var trackedProperties = WebConfigurationManager.AppSettings["TrackedHistoryProperties"].Split(',').ToList();
                if (!trackedProperties.Contains(property.Name))
                    continue;


                var value = (property.GetValue(newTicket, null)??"").ToString();

                var oldValue = (property.GetValue(oldTicket,null)??"").ToString();

               

                if (value != oldValue)
                {
                    var newHistory = new TicketHistory
                    {
                        UserId = HttpContext.Current.User.Identity.GetUserId(),
                        Updated = newTicket.Updated.GetValueOrDefault(),
                        PropertyName = property.Name,
                        OldValue = Utilities.MakeReadable(property.Name, oldValue),
                        NewValue = Utilities.MakeReadable(property.Name, value),
                        TicketId = newTicket.Id
                        
                    };
                    db.TicketHistories.Add(newHistory);

                }

            }
            db.SaveChanges();
        }

    }
}