using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AddressBook.Models;

namespace AddressBook.Controllers
{
    public class ContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ContactsController() { db.Configuration.ValidateOnSaveEnabled = false; }

        // GET: Contacts
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login","Account");
            var contacts = db.Contacts.Where(c => c.User.UserName == User.Identity.Name);
            List<Contact> list = contacts.ToList();
            foreach (Contact contact in list)
                contact.DecryptPII();
            return View(contacts.ToList());
        }

        // GET: Contacts/Details/5
        [Authorize(Roles = "User")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Where(c => c.User.UserName == User.Identity.Name && c.Id == id).FirstOrDefault();
            if (contact == null)
            {
                return HttpNotFound();
            }
            contact.DecryptPII();
            contact.Notes = HttpContext.Server.HtmlDecode(contact.Notes);
            return View(contact);
        }

        // GET: Contacts/Create
        [Authorize(Roles = "User")]
        public ActionResult Create()
        {
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public ActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.Where(u => u.UserName == User.Identity.Name).First();
                contact.UserId = user.Id;
                contact.EncryptPII();
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.UserId = new SelectList(db.Users, "Id", "Email", contact.UserId);
            contact.Notes = HttpContext.Server.HtmlDecode(contact.Notes);
            return View(contact);
        }

        // GET: Contacts/Edit/5
        [Authorize(Roles = "User")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Where(c => c.User.UserName == User.Identity.Name && c.Id == id).FirstOrDefault();
            if (contact == null)
            {
                return HttpNotFound();
            }
            contact.DecryptPII();
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Email", contact.UserId);
            contact.Notes = HttpContext.Server.HtmlDecode(contact.Notes);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public ActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.EncryptPII();
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Email", contact.UserId);
            contact.Notes = HttpContext.Server.HtmlDecode(contact.Notes);
            return View(contact);
        }

        // GET: Contacts/Delete/5
        [Authorize(Roles = "User")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Where(c => c.User.UserName == User.Identity.Name && c.Id == id).FirstOrDefault();
            if (contact == null)
            {
                return HttpNotFound();
            }
            contact.DecryptPII();
            contact.Notes = HttpContext.Server.HtmlDecode(contact.Notes);
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
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
