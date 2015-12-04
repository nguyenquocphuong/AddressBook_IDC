using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressBook.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Street Name")]
        public string StreetName { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Province")]
        public string Province { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        public void EncryptPII()
        {
            this.FirstName = Convert.ToBase64String(myAES.AES.Encrypt(this.FirstName));
            this.LastName = Convert.ToBase64String(myAES.AES.Encrypt(this.LastName));
            this.PhoneNumber = Convert.ToBase64String(myAES.AES.Encrypt(this.PhoneNumber));
        }

        public void DecryptPII()
        {
            try
            {
                this.FirstName = myAES.AES.Decrypt(Convert.FromBase64String(this.FirstName));
                this.LastName = myAES.AES.Decrypt(Convert.FromBase64String(this.LastName));
                this.PhoneNumber = myAES.AES.Decrypt(Convert.FromBase64String(this.PhoneNumber));
            }
            catch { }
        }
    }
}