using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTICrypto
{
    public static class Toolkit
    {
        public static string GetKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = Constants.KEY_SIZE_BITS;
                return Convert.ToBase64String(aes.Key);
            }
        }

        public static byte[] GetBytesFromPlainString(string data)
        {
            if (data == null) { throw new ArgumentNullException(); }
            return Encoding.UTF8.GetBytes(data);
        }

        public static byte[] GetBytesFromBase64String(string data)
        {
            if (data == null) { throw new ArgumentNullException(); }
            return Convert.FromBase64String(data);
        }

        public static string GetStringFromCipherBytes(byte[] data)
        {
            if (data == null) { throw new ArgumentNullException(); }
            return Encoding.UTF8.GetString(data);
        }
    }
}
