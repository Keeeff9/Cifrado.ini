using System.Text;
using System.Security.Cryptography;

namespace ms_doc_encrypt.Infrastructure.Models
{
    public static class ToolKit
    {
        public static string GetKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = AesConstants.KEY_SIZE_BITS;
                return Convert.ToBase64String(aes.Key);
            }
        }

        public static byte[] GetBytesFromPlainString(string data)
        {
            if (data == null) { throw new ArgumentException("Cadena de datos nula"); }
            return Encoding.UTF8.GetBytes(data);
        }

        public static byte[] GetBytesFromBase64String(string data)
        {
            if (data == null) { throw new ArgumentException("Cadena base64 nula"); }
            return Convert.FromBase64String(data);
        }

        public static string GetStringFromCipherBytes(byte[] data)
        {
            if (data == null) { throw new ArgumentException("Bytes cifrados nulos"); }
            return Encoding.UTF8.GetString(data);
        }
        public static string GetBase64StringFromCipherBytes(byte[] data)
        {
            if (data == null) { throw new ArgumentException("Bytes cifrados nulos"); }
            return Convert.ToBase64String(data);
        }
    }
}
