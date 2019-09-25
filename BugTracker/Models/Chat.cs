using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string OwnerUserId { get; set; }
        public string AssignedToUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

       
        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }

        public virtual ICollection<TicketNotification> ChatNotifications { get; set; }

        public Chat()
        {
        
            ChatNotifications = new HashSet<TicketNotification>();
        }
    }
}