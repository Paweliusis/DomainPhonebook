using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows;
using System.Security;
using System.Runtime.InteropServices;

namespace PhoneBook.Classes
{
    class Encoding
    {
        public static string CreateHashFromSecureString(SecureString password)
        {
            return GenerateHash(DecodingSecureString(password));
        }
        // switch to public
        public static string DecodingSecureString(SecureString _secureString)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(_secureString);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally { Marshal.ZeroFreeGlobalAllocUnicode(valuePtr); }
        }

        // make public for test
        public static string GenerateHash(string password)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            using (SHA512 sha = new SHA512Managed()) { return Convert.ToBase64String(sha.ComputeHash(data)); }
        }
        // test salt and hash
        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[1024];

            rng.GetBytes(buffer);
            string salt = BitConverter.ToString(buffer);
            return salt;
        }
        public static string HashAndSaltPassword(string password, string salt)
        {
            var saltedPassword = password + salt;
            return GenerateHash(saltedPassword);
        }

    }
}
