using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AddressBook.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }

        public string TempPassword { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public UserViewModel ToViewModel()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<IdentityRole> Roles = db.Roles.ToList();

                var userViewModel = new UserViewModel
                {
                    Id = this.Id,
                    UserName = this.UserName,
                    Email = this.Email,
                    FirstName = this.FirstName,
                    LastName = this.LastName
                };

                foreach (var userRole in this.Roles)
                {
                    IdentityRole role = Roles.Find(r => r.Id == userRole.RoleId);
                    userViewModel.Roles.Add(new AssignedRole
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        Assigned = true
                    });
                }

                return userViewModel;
            }
        }

        public UserViewModel ToViewModel(ICollection<IdentityRole> allDbRoles)
        {
            var userViewModel = new UserViewModel
            {
                Id = this.Id,
                UserName = this.UserName,
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName
            };

            // Collection for full list of courses with user's already assigned courses included
            ICollection<AssignedRole> allRoles = new List<AssignedRole>();

            foreach (var r in allDbRoles)
            {
                // Create new AssignedCourseData for each course and set Assigned = true if user already has course
                var assignedRole = new AssignedRole
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    Assigned = this.Roles.FirstOrDefault(x => x.RoleId == r.Id) != null
                };

                allRoles.Add(assignedRole);
            }

            userViewModel.Roles = allRoles;

            return userViewModel;
        }
    }
}