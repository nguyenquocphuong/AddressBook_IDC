using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }

        public static RoleViewModel ToViewModel(IdentityRole Role)
        {
            var roleViewModel = new RoleViewModel()
            {
                Id = Role.Id,
                Name = Role.Name
            };
            return roleViewModel;
        }

        public static List<RoleViewModel> ToViewModels(List<IdentityRole> Roles)
        {
            List<RoleViewModel> roleViewModels = new List<RoleViewModel>();
            foreach (IdentityRole role in Roles)
                roleViewModels.Add(ToViewModel(role));
            return roleViewModels;
        }

        public IdentityRole ToDomainModel()
        {
            IdentityRole role = new IdentityRole();
            if (!string.IsNullOrWhiteSpace(this.Id))
                role.Id = this.Id;
            role.Name = this.Name;

            return role;
        }
    }
}