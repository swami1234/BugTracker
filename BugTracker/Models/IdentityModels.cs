using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Models
{
  
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }

        }

        [NotMapped]
        public string FullNameWithEmail
        {
            get
            {
                return $"{LastName}, {FirstName} - {Email}";
            }

        }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }

        public ApplicationUser()
        {
            Projects = new HashSet<Project>();
            Chats = new HashSet<Chat>();
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketComments = new HashSet<TicketComment>();
            TicketHistories = new HashSet<TicketHistory>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

     public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
     {
        public ApplicationDbContext()
             : base("BugTracker", throwIfV1Schema: false) { }
        

        

            public static ApplicationDbContext Create()
            {
                return new ApplicationDbContext();
            }


        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets  { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<TicketAttachment> TicketAttachments  { get; set; }
        public DbSet<TicketComment> TicketComments  { get; set; }
        public DbSet<TicketHistory> TicketHistories  { get; set; }
        public DbSet<TicketNotification> TicketNotifications  { get; set; }
        public DbSet<ChatNotification> ChatNotifications { get; set; }
        public DbSet<TicketPriority> TicketPriorities  { get; set; }
        public DbSet<TicketStatus>TicketStatuses  { get; set; }
        public DbSet<TicketType> TicketTypes  { get; set; }

        //public System.Data.Entity.DbSet<BugTracker.Models.ChatNotification> ChatNotifications { get; set; }

        //public System.Data.Entity.DbSet<BugTracker.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}