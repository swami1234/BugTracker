﻿using BugTracker.Helper;
using BugTracker.Models;
using BugTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize(Roles = "Project Manager, Admin")]
    [RequireHttps]
    public class AdminController : Controller
    {

      private ApplicationDbContext db = new ApplicationDbContext();
      private UserRolesHelper roleHelper = new UserRolesHelper();
      private ProjectsHelper projectHelper = new ProjectsHelper();

        // GET: Admin
        public ActionResult UserIndex()
        {
            var allUsers = db.Users.ToList();

            var users = allUsers.Select(u => new UserProfileViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName,
                AvatarUrl = u.AvatarUrl,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email
            }).ToList();


            return View(users);
        }

      

        [HttpGet]
        public ActionResult ManageUserRoles(string userId)
        {

            var currentRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            ViewBag.UserId = userId;

            ViewBag.UserRole = new SelectList(db.Roles.ToList(), "Name", "Name", currentRole);


          

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserRoles(string userId, string userRole)
        {

            foreach (var role in roleHelper.ListUserRoles(userId))
            {
                roleHelper.RemoveUserFromRole(userId, role);
            }

            if(! string.IsNullOrEmpty(userRole))
            {
                roleHelper.AddUserToRole(userId, userRole);
            }

           return RedirectToAction("UserIndex");
        }

        [HttpGet]
        public ActionResult ManageProjects(string userId)
        {
            var currentProject = projectHelper.ListUserProjects(userId).FirstOrDefault();

            ViewBag.UserId = userId;


            ViewBag.UserProject = new SelectList(db.Projects.ToList(), "Id", "Name", currentProject.Id);


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjects(string userId, int Userproject)
        {

            foreach (var project in projectHelper.ListUserProjects(userId))
            {
                projectHelper.RemoveUserFromProject(userId, project.Id);
            }

            if (Userproject > 0 )
            {
                
                
                    projectHelper.AddUserToProject(userId, Userproject);
                


            }




            return RedirectToAction("UserIndex");
        }


        public ActionResult ManageRoles()
        {
            var users = db.Users.Select(u => new UserProfileViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName,
                AvatarUrl = u.AvatarUrl,
                Email = u.Email
            }).ToList();

            ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Id", "FullNameWithEmail");
            ViewBag.RoleName = new SelectList(db.Roles.ToList(), "Name", "Name");

            return View(users);
        }

        [HttpPost]
        public ActionResult ManageRoles(List<string> users, string roleName)
        {
            if (users != null)
            {
                //Lets iterate over the incoming list of Users that were selected from the form
                foreach (var userId in users)
                {
                    //and remove each of them from whatever role they occupy only to add them back to the selected role
                    foreach (var role in roleHelper.ListUserRoles(userId))
                    {
                        roleHelper.RemoveUserFromRole(userId, role);
                    }

                    //Only to add the back to the selected role
                    if (!string.IsNullOrEmpty(roleName))
                    {
                        roleHelper.AddUserToRole(userId, roleName);
                    }
                }
            }

            return RedirectToAction("ManageRoles");
        }

        public ActionResult ManageUserProjects(string userId)
        {
            var myProjects = projectHelper.ListUserProjects(userId).Select(p => p.Id);

            ViewBag.Projects = new MultiSelectList(db.Projects.ToList(), "Id", "Name", myProjects);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserProjects(List<int> projects, string userId)
        {
            foreach (var project in projectHelper.ListUserProjects(userId).ToList())
            {
                projectHelper.RemoveUserFromProject(userId, project.Id);
            }

            if (projects != null)
            {
                foreach (var projectId in projects)
                {
                    projectHelper.AddUserToProject(userId, projectId);
                }
            }

            return RedirectToAction("UserIndex");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectUsers(int projectId, List<string> ProjectManagers, List<string> Developers, List<string> Submitters)
        {
            //Step 1: Remove all users from the project
            foreach (var user in projectHelper.UsersOnProject(projectId).ToList())
            {
                projectHelper.RemoveUserFromProject(user.Id, projectId);
            }

            //Step 2: Adds back all the selected PM's
            if (ProjectManagers != null)
            {
                foreach (var projectManagerId in ProjectManagers)
                {
                    projectHelper.AddUserToProject(projectManagerId, projectId);
                }
            }

            //Step 3: Adds back all the selected Developers
            if (Developers != null)
            {
                foreach (var developerId in Developers)
                {
                    projectHelper.AddUserToProject(developerId, projectId);
                }
            }

            //Step 4: Adds back all the selected Submitters
            if (Submitters != null)
            {
                foreach (var submitterId in Submitters)
                {
                    projectHelper.AddUserToProject(submitterId, projectId);
                }
            }

            //Step 4: Redirect the user somewhere
            return RedirectToAction("Details", "Projects", new { id = projectId });
        }
    }
}

       
  