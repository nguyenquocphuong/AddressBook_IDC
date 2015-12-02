using Microsoft.AspNet.Identity;
using System.Web.Configuration;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System;

namespace AddressBook.Models
{
    public class SHA256PasswordHasher: IPasswordHasher
    {
        public string HashPassword(string password)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] hashValue = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashValue);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            string testHash = this.HashPassword(providedPassword);
            return hashedPassword.Equals(testHash) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}