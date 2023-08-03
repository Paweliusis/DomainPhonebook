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
        private static string DecodingSecureString(SecureString _secureString)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(_secureString);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally { Marshal.ZeroFreeGlobalAllocUnicode(valuePtr); }
        }
        //private static string CreateSalt(int _size)
        //{
        //    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        //    byte[] buff = new byte[_size];
        //    rng.GetBytes(buff);
        //    return Convert.ToBase64String(buff);
        //}
        //private static byte[] GenerateSaltedHash(byte[] _secureString, byte[] _salt)
        //{
        //    HashAlgorithm alg = new SHA256Managed();
        //    byte[] _passwordWithSalt = new byte[_secureString.Length + _salt.Length];
        //    for (int i = 0; i < _secureString.Length; i++) { _passwordWithSalt[i] = _secureString[i]; }
        //    for (int i = 0; i < _salt.Length; i++) { _passwordWithSalt[_secureString.Length + i] = _salt[i]; }
        //    return alg.ComputeHash(_passwordWithSalt);
        //}
        private static string GenerateHash(string password)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            using (SHA512 sha = new SHA512Managed()) { return Convert.ToBase64String(sha.ComputeHash(data)); }
        }
    }
}
