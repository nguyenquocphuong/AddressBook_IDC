using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AddressBook.Models;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System;

namespace AddressBook.Controllers
{
    public class UsersController : Controller
    {
        public const string DEFAULT_PASSWORD = "P@ssw0rd";
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    _userManager.PasswordHasher = new SHA256PasswordHasher();
                }
                return _userManager;
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: ApplicationUsers
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: ApplicationUsers/Details/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userIQueryable = from u in db.Users.Include("Roles")
                                 where u.Id == id
                                 select u;

            if (!userIQueryable.Any())
            {
                return HttpNotFound("User not found.");
            }

            var user = userIQueryable.First();
            var userViewModel = user.ToViewModel();

            return View(userViewModel);
        }

        // GET: ApplicationUsers/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            var userViewModel = new UserViewModel { Roles = PopulateRole() };
            return View(userViewModel);
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = userViewModel.ToDomainModel();

                var result = await UserManager.CreateAsync(user, user.Email + UsersController.DEFAULT_PASSWORD);
                if (result.Succeeded)
                {
                //    foreach (AssignedRole assignedRole in userViewModel.Roles)
                //        if(assignedRole.Assigned)
                //            await UserManager.AddToRoleAsync(user.Id, assignedRole.RoleName);
                    return RedirectToAction("Index");
                }
            }

            return View(userViewModel);
        }

        // GET: ApplicationUsers/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name");


            // Get all courses
            var allDbRoles = db.Roles.ToList();

            // Get the user we are editing and include the courses already subscribed to
            var user = db.Users.Include("Roles").FirstOrDefault(x => x.Id == id);
            var userViewModel = user.ToViewModel(allDbRoles);

            return View(userViewModel);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var originalUser = db.Users.Find(userViewModel.Id);
                AddOrUpdateKeepExistingRoles(originalUser, userViewModel.Roles);
                db.Entry(originalUser).CurrentValues.SetValues(userViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userViewModel);
        }

        // GET: ApplicationUsers/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userIQueryable = from u in db.Users.Include("Roles")
                                 where u.Id == id
                                 select u;

            if (!userIQueryable.Any())
            {
                return HttpNotFound("User not found.");
            }

            var user = userIQueryable.First();
            var userViewModel = user.ToViewModel();

            return View(userViewModel);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(string id)
        {
            User applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        private void AddOrUpdateKeepExistingRoles(User user, IEnumerable<AssignedRole> assignedRoles)
        {
            var webRoleAssignedIds = assignedRoles.Where(c => c.Assigned).Select(webRole => webRole.RoleId);
            var dbRoleIds = user.Roles.Select(dbRole => dbRole.RoleId);
            var RoleIds = dbRoleIds as string[] ?? dbRoleIds.ToArray();
            var roleToDeleteIDs = RoleIds.Where(id => !webRoleAssignedIds.Contains(id)).ToList();

            // Delete removed courses
            foreach (var id in roleToDeleteIDs)
            {
                user.Roles.Remove(db.Roles.Find(id).Users.First());
            }

            // Add courses that user doesn't already have
            foreach (var id in webRoleAssignedIds)
            {
                if (!RoleIds.Contains(id))
                {
                    user.Roles.Add(new IdentityUserRole { RoleId = id, UserId = user.Id });
                }
            }
        }

        private ICollection<AssignedRole> PopulateRole()
        {
            var roles = db.Roles;
            var assignedRoles = new List<AssignedRole>();

            foreach (var item in roles)
            {
                assignedRoles.Add(new AssignedRole
                {
                    RoleId = item.Id,
                    RoleName = item.Name,
                    Assigned = false
                });
            }

            return assignedRoles;
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
