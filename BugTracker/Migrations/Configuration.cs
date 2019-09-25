namespace BugTracker.Migrations
{
    using BugTracker.Helper;
    using BugTracker.Helpers;
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
       
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {


            


        #region Ticket Priorities, Ticket Statutes, ticket Type

             context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                new TicketPriority { Name = "Urgent", Description = "Highest Priority, requiring immediate attention" },
                new TicketPriority { Name = "High", Description = " second Highest Priority, requiring hasty attention" },
                new TicketPriority { Name = "Medium", Description = "Normal Priority, requiring moderate attention, below urgent and high" },
                new TicketPriority { Name = "Low", Description = "Little action is needed" },
                new TicketPriority { Name = "None", Description = "Not a Priority" },
                new TicketPriority { Name = "Unassigned", Description = "unassigned" }
                );

            context.TicketStatuses.AddOrUpdate(
               t => t.Name,
               new TicketStatus { Name = "New UnAssigned", Description = "Unassigned to Developer" },
               new TicketStatus { Name = "Assigned", Description = "Assigned to Developer" },
               new TicketStatus { Name = "In progress", Description = "Being worked on by Developer" },
               new TicketStatus { Name = "Reassigned", Description = "Has been reassigned to a different Developer" },
               new TicketStatus { Name = "Unassigned", Description = "Has no Developer assigned to it" },
               new TicketStatus { Name = "Completed", Description = "Completed!" }
               );

            context.TicketTypes.AddOrUpdate(
               t => t.Name,
               new TicketType { Name = "Defective", Description = "Something is broken making the entire program to malfunction" },
               new TicketType { Name = "Functionality Bug", Description = "A bug affecting the functionality of the website" },
               new TicketType { Name = "Functionality Request", Description = "A request to change or add functionaliy to the program" },
               new TicketType { Name = "Visual Bug", Description = "Non-Aligned visuals, or meshing of Z-index, etc" },
               new TicketType { Name = "Visual Request", Description = "A request to add new visual features to the program" },
               new TicketType { Name = "Low Prority", Description = "Issues that do not require immediate attention" }
               );

            context.SaveChanges();

            #endregion

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            #region User & Role Creation
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "sarveshpatel123@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sarveshpatel123@gmail.com",
                    Email = "sarveshpatel123@gmail.com",
                    FirstName = "sarvesh",
                    LastName = "patel",
                    DisplayName = "Sarvesh Patel"
                }, "Abc#123321");
            }
            if (!context.Users.Any(u => u.Email == "Admin@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Admin@mailinator.com",
                    Email = "Admin@mailinator.com",
                    FirstName = "Admin",
                    LastName = "patel",
                    DisplayName = "Admin Patel"
                }, WebConfigurationManager.AppSettings["DemoUserPassword"]);
            }

            var DemoAdmin = userManager.FindByEmail("Admin@mailinator.com").Id;
            userManager.AddToRole(DemoAdmin, "Admin");

            var adId = userManager.FindByEmail("sarveshpatel123@gmail.com").Id;
            userManager.AddToRole(adId, "Admin");

            //
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "sarveshpatel1234@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sarveshpatel1234@gmail.com",
                    Email = "sarveshpatel1234@gmail.com",
                    FirstName = "Jack",
                    LastName = "Dorsie",
                    DisplayName = "Jack Dorsie"
                }, "Abc#123321");
            }
            if (!context.Users.Any(u => u.Email == "Submitter@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Submitter@mailinator.com",
                    Email = "Submitter@mailinator.com",
                    FirstName = "Submitter",
                    LastName = "patel",
                    DisplayName = "Submitter Patel"
                }, WebConfigurationManager.AppSettings["DemoUserPassword"]);
            }

            var sbId = userManager.FindByEmail("sarveshpatel1234@gmail.com").Id;
            userManager.AddToRole(sbId, "Submitter");

            var DemoSubmitter = userManager.FindByEmail("Submitter@mailinator.com").Id;
            userManager.AddToRole(DemoSubmitter, "Submitter");
            //

            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "sarveshpatel1235@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sarveshpatel1235@gmail.com",
                    Email = "sarveshpatel1235@gmail.com",
                    FirstName = "Nancy",
                    LastName = "Patel",
                    DisplayName = "Nancy Patel"
                }, "Abc#123321");
            }
            if (!context.Users.Any(u => u.Email == "ProjectManager@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ProjectManager@mailinator.com",
                    Email = "ProjectManager@mailinator.com",
                    FirstName = "ProjectManager",
                    LastName = "patel",
                    DisplayName = "ProjectManager Patel"
                }, WebConfigurationManager.AppSettings["DemoUserPassword"]);
            }

            var  pmId = userManager.FindByEmail("sarveshpatel1235@gmail.com").Id;
            userManager.AddToRole(pmId, "Project Manager");

            var DemoProjectManager = userManager.FindByEmail("ProjectManager@mailinator.com").Id;
            userManager.AddToRole(DemoProjectManager, "Project Manager");
            //

            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "sarveshpatel1236@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sarveshpatel1236@gmail.com",
                    Email = "sarveshpatel1236@gmail.com",
                    FirstName = "Jason",
                    LastName = "Smith",
                    DisplayName = "Jason Smith"
                }, "Abc#123321");
            }
            if (!context.Users.Any(u => u.Email == "Developer@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Developer@mailinator.com",
                    Email = "Developer@mailinator.com",
                    FirstName = "Developer",
                    LastName = "patel",
                    DisplayName = "Developer Patel"
                }, WebConfigurationManager.AppSettings["DemoUserPassword"]);
            }

            var dvId = userManager.FindByEmail("sarveshpatel1236@gmail.com").Id;
            userManager.AddToRole(dvId, "Developer");

            var DemoDeveloper = userManager.FindByEmail("Developer@mailinator.com").Id;
            userManager.AddToRole(DemoDeveloper, "Developer");



            #endregion

            #region Projection Creation
            context.Projects.AddOrUpdate(
           p => p.Name,
               new Project { Name = "SP Blog", Description = "This is the SP Blog project that is online.", Created = DateTime.Now },
               new Project { Name = "Portfolio", Description = "This is the Portfolio project that is online.", Created = DateTime.Now },
                new Project { Name = "CMS1", Description = "This is the CMS1 project that is online.", Created = DateTime.Now },
               new Project { Name = "Bug Tracker", Description = "This is the BugTracker project that is online.", Created = DateTime.Now }
           );

            context.SaveChanges();

            #endregion

            #region Project assignment
            var blogProjectId = context.Projects.FirstOrDefault(p => p.Name == "SP Blog").Id;
            var bugTrackerId = context.Projects.FirstOrDefault(p => p.Name == "Bug Tracker").Id;
            var cmsId = context.Projects.FirstOrDefault(p => p.Name == "CMS1").Id;

            var projectHelper = new ProjectsHelper();

            projectHelper.AddUserToProject(pmId, blogProjectId);
            projectHelper.AddUserToProject(dvId, blogProjectId);
            projectHelper.AddUserToProject(sbId, blogProjectId);
            projectHelper.AddUserToProject(adId, blogProjectId);

            projectHelper.AddUserToProject(DemoAdmin, blogProjectId);
            projectHelper.AddUserToProject(DemoSubmitter, blogProjectId);
            projectHelper.AddUserToProject(DemoProjectManager, blogProjectId);
            projectHelper.AddUserToProject(DemoDeveloper, blogProjectId);

            projectHelper.AddUserToProject(pmId, bugTrackerId);
            projectHelper.AddUserToProject(dvId, bugTrackerId);
            projectHelper.AddUserToProject(sbId, bugTrackerId);
            projectHelper.AddUserToProject(adId, bugTrackerId);

            projectHelper.AddUserToProject(DemoAdmin, bugTrackerId);
            projectHelper.AddUserToProject(DemoSubmitter, bugTrackerId);
            projectHelper.AddUserToProject(DemoProjectManager, bugTrackerId);
            projectHelper.AddUserToProject(DemoDeveloper, bugTrackerId);


            projectHelper.AddUserToProject(pmId, cmsId);
            projectHelper.AddUserToProject(dvId, cmsId);
            projectHelper.AddUserToProject(sbId, cmsId);
            projectHelper.AddUserToProject(adId, cmsId);

            projectHelper.AddUserToProject(DemoAdmin, cmsId);
            projectHelper.AddUserToProject(DemoSubmitter, cmsId);
            projectHelper.AddUserToProject(DemoProjectManager, cmsId);
            projectHelper.AddUserToProject(DemoDeveloper, cmsId);

            #endregion


            #region Ticket Creation

            //  var ticketHelper = new TicketHelper();

            context.Tickets.AddOrUpdate(
                p => p.Title,

                new Ticket
                {
                    ProjectId = blogProjectId,
                    OwnerUserId = sbId,
                    Title = "Seeded Ticket #1",
                    Description = "Testing a seeded Ticket",
                    Created = DateTime.Now,
                    TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                    TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "New UnAssigned").Id,
                    TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Visual Bug").Id,

                },

                new Ticket
                {
                    ProjectId = blogProjectId,
                    OwnerUserId = sbId,
                    AssignedToUserId = dvId,
                    Title = "Seeded Ticket #2",
                    Description = "Testing a seeded Ticket",
                    Created = DateTime.Now,
                    TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                    TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "Assigned").Id,
                    TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Defective").Id,
                },

                 new Ticket
                 {
                     ProjectId = bugTrackerId,
                     OwnerUserId = sbId,
                     Title = "Seeded Ticket #3",
                     Description = "Testing a seeded Ticket",
                     Created = DateTime.Now,
                     TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                     TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "New UnAssigned").Id,
                     TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Visual Bug").Id,

                 },

                    new Ticket
                    {
                        ProjectId = bugTrackerId,
                        OwnerUserId = sbId,
                        AssignedToUserId = dvId,
                        Title = "Seeded Ticket #4",
                        Description = "Testing a seeded Ticket",
                        Created = DateTime.Now,
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "Assigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Defective").Id,
                    },

                      new Ticket
                      {
                          ProjectId = cmsId,
                          OwnerUserId = sbId,
                          Title = "Seeded Ticket #5",
                          Description = "Testing a seeded Ticket",
                          Created = DateTime.Now,
                          TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                          TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "New UnAssigned").Id,
                          TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Visual Bug").Id,

                      },

                    new Ticket
                    {
                        ProjectId = cmsId,
                        OwnerUserId = sbId,
                        AssignedToUserId = dvId,
                        Title = "Seeded Ticket #6",
                        Description = "Testing a seeded Ticket",
                        Created = DateTime.Now,
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "Assigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Defective").Id,
                    });

            #endregion

        }
    }
}
