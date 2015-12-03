using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AddressBook.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;

namespace AddressBook.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Role
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(RoleViewModel.ToViewModels(db.Roles.ToList()));
        }

        // GET: Role/Details/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityRole role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(RoleViewModel.ToViewModel(role));
        }

        // GET: Role/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "Name")] RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = roleViewModel.ToDomainModel();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                var roleresult = roleManager.CreateAsync(role);

                if (roleresult.Result != IdentityResult.Success)
                {
                    ModelState.AddModelError("", roleresult.Result.Errors.First());
                    return View(roleViewModel);
                }
                return RedirectToAction("Index");
            }

            return View(roleViewModel);
        }

        // GET: Role/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityRole role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(RoleViewModel.ToViewModel(role));
        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "Id,Name")] RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(role).State = EntityState.Modified;
                //db.SaveChanges();

                var role = roleViewModel.ToDomainModel();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                var roleresult = roleManager.UpdateAsync(role);

                if (roleresult.Result != IdentityResult.Success)
                {
                    ModelState.AddModelError("", roleresult.Result.Errors.First());
                    return View(roleViewModel);
                }

                return RedirectToAction("Index");
            }
            return View(roleViewModel);
        }

        // GET: Role/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityRole role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(RoleViewModel.ToViewModel(role));
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(RoleViewModel roleViewModel)
        {
            IdentityRole role = db.Roles.Find(roleViewModel.Id);
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var roleresult = roleManager.DeleteAsync(role);

            if (roleresult.Result != IdentityResult.Success)
            {
                ModelState.AddModelError("", roleresult.Result.Errors.First());
                return View();
            }

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
