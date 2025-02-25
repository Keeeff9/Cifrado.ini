using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTICrypto
{
    public class AESDecrypt : MTIAes
    {
        public AESDecrypt(byte[] data, string keyBase64) : base(data, keyBase64) { }

        public AESDecrypt(string base64String, string keyBase64) : base(Toolkit.GetBytesFromBase64String(base64String), keyBase64) { }

        public byte[] Decrypt()
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = this.key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] cryptoIV = new byte[Constants.IV_SIZE_BYTES];
                Array.Copy(this.data, cryptoIV, Constants.IV_SIZE_BYTES);
                aes.IV = cryptoIV;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    using (var msDecrypt = new MemoryStream())
                    {
                        using (var decryptoStream = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                        {
                            using (var binaryWriter = new BinaryWriter(decryptoStream))
                            {
                                binaryWriter.Write(this.data, Constants.IV_SIZE_BYTES, this.data.Length - Constants.IV_SIZE_BYTES);
                            }
                        }
                        return msDecrypt.ToArray();
                    }
                }
            }
        }
    }
}
