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
            if (string.IsNullOrWhiteSpace(password)) return null;
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] hashValue = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashValue);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword) || string.IsNullOrWhiteSpace(providedPassword)) return PasswordVerificationResult.Failed;
            string testHash = this.HashPassword(providedPassword);
            return hashedPassword.Equals(testHash) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}