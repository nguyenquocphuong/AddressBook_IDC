using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models
{
    public class UserBasicInfoViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }

    public class UserViewModel : UserBasicInfoViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string UserName { get; set; }

        public virtual ICollection<AssignedRole> Roles { get; set; }

        public UserViewModel()
        {
            Roles = new Collection<AssignedRole>();
        }

        public User ToDomainModel()
        {
            User user = new User();

            if (!string.IsNullOrWhiteSpace(this.Id))
                user.Id = this.Id;
            user.UserName = this.UserName;
            user.Email = this.Email;
            user.FirstName = this.FirstName;
            user.LastName = this.LastName;

            return user;
        }
    }

    public class AssignedRole
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Assigned { get; set; }
    }
}