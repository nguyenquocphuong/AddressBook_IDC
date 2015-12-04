using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System;

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

        public static UserBasicInfoViewModel ToViewModel(User user)
        {
            UserBasicInfoViewModel userBasicInfoViewModel = new UserBasicInfoViewModel();
            ToViewModel(user, userBasicInfoViewModel);
            return userBasicInfoViewModel;
        }

        public static void ToViewModel(User user, UserBasicInfoViewModel userBasicInfoViewModel)
        {
            userBasicInfoViewModel.Email = user.Email;
            userBasicInfoViewModel.FirstName = AES.Decrypt(Convert.FromBase64String(user.FirstName));
            userBasicInfoViewModel.LastName = AES.Decrypt(Convert.FromBase64String(user.LastName));
        }

        public static User ToDomainModel(UserBasicInfoViewModel userBasicInfoViewModel)
        {
            User user = new User();
            ToDomainModel(user, userBasicInfoViewModel);
            return user;
        }

        public static void ToDomainModel(User user, UserBasicInfoViewModel userBasicInfoViewModel)
        {
            user.Email = userBasicInfoViewModel.Email;
            user.FirstName = Convert.ToBase64String(AES.Encrypt(userBasicInfoViewModel.FirstName));
            user.LastName = Convert.ToBase64String(AES.Encrypt(userBasicInfoViewModel.LastName));
        }
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

        public static UserViewModel ToViewModel(User user)
        {
            UserViewModel userViewModel = new UserViewModel();

            UserBasicInfoViewModel.ToViewModel(user, userViewModel);
            userViewModel.Id = user.Id;
            userViewModel.UserName = user.UserName;

            return userViewModel;
        }

        public static List<UserViewModel> ToViewModel(List<User> users)
        {
            List<UserViewModel> userViewModels = new List<UserViewModel>();

            foreach (User user in users)
                userViewModels.Add(UserViewModel.ToViewModel(user));

            return userViewModels;
        }

        public static User ToDomainModel(UserViewModel userViewModel)
        {
            User user = new User();
            ToDomainModel(user, userViewModel);
            return user;
        }

        public static void ToDomainModel(User user, UserViewModel userViewModel)
        {
            UserBasicInfoViewModel.ToDomainModel(user, userViewModel);
            if (!string.IsNullOrWhiteSpace(userViewModel.Id))
                user.Id = userViewModel.Id;
            user.UserName = userViewModel.UserName;
        }
    }

    public class AssignedRole
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Assigned { get; set; }
    }
}