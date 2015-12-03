using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class UserRole: IdentityUserRole
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}