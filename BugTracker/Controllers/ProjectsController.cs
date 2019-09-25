using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helper;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    [RequireHttps]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();

        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }


        [Authorize(Roles = "Project Manager, Submitter, Developer")]
        public ActionResult MyProjects()

        {

            var userId = User.Identity.GetUserId();

            var currentUser = db.Users.Find(userId);

            var myProjects = currentUser.Projects.ToList();

            return View(myProjects);



        }



        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var allProjectManagers = roleHelper.UsersInRole("Project Manager");
            var currentProjectManagers = projectHelper.UsersInRoleOnProject(project.Id, "Project Manager");
            ViewBag.ProjectManagers = new MultiSelectList(allProjectManagers, "Id", "FullNameWithEmail", currentProjectManagers);

            var allSubmitters = roleHelper.UsersInRole("Submitter");
            var currentSubmitters = projectHelper.UsersInRoleOnProject(project.Id, "Submitter");
            ViewBag.Submitters = new MultiSelectList(allSubmitters, "Id", "FullNameWithEmail", currentSubmitters);

            var allDevelopers = roleHelper.UsersInRole("Developer");
            var currentDevelopers = projectHelper.UsersInRoleOnProject(project.Id, "Developer");
            ViewBag.Developers = new MultiSelectList(allDevelopers, "Id", "FullNameWithEmail", currentDevelopers);

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(string userId, List<int> UserProject)
        {

            foreach (var project in projectHelper.ListUserProjects(userId))
            {
                projectHelper.RemoveUserFromProject(userId, project.Id);
            }

            if (UserProject != null)
            {
                foreach (var projectId in UserProject)
                {
                    projectHelper.AddUserToProject(userId, projectId);
                }

            }
            return RedirectToAction("UserIndex","Admin");
        }
            //[HttpPost]
            //public ActionResult Details(List<string> users, int projectName)
            //{
            //    if (users != null)
            //    {
            //        //Lets iterate over the incoming list of Users that were selected from the form
            //        foreach (var userId in users)
            //        {
            //            //and remove each of them from whatever role they occupy only to add them back to the selected role
            //            foreach (var project in projectHelper.ListUserProjects(userId))
            //            {
            //                projectHelper.RemoveUserFromProject(userId, project);
            //            }

            //            //Only to add the back to the selected role
            //            if (!string.IsNullOrEmpty(projectName))
            //            {
            //                roleHelper.AddUserToRole(userId, projectName);
            //            }
            //        }
            //    }

            //    return RedirectToAction("ManageRoles");
            //}


            // GET: Projects/Create
            public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Created")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Created")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
