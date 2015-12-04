using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using myAES;

namespace AES
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                string original = "Here is some data to encrypt!";

                // Create a new instance of the AesManaged
                // class.  This generates a new key and initialization 
                // vector (IV).
                using (AesManaged myAes = new AesManaged())
                {

                    // Encrypt the string to an array of bytes.
                    byte[] encrypted = myAES.AES.Encrypt(original);

                    // Decrypt the bytes to a string.
                    string roundtrip = myAES.AES.Decrypt(encrypted);

                    //Display the original data and the decrypted data.
                    Console.WriteLine("Original:   {0}", original);
                    Console.WriteLine("Round Trip: {0}", roundtrip);
                    Console.ReadKey();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
    }
}
