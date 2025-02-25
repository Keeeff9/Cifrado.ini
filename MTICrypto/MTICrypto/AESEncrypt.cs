using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTICrypto
{
    public class AESEncrypt : MTIAes
    {
        public AESEncrypt(byte[] data, string keyBase64) : base(data, keyBase64) { }

        public AESEncrypt(string plainString, string keyBase64) : base(Toolkit.GetBytesFromPlainString(plainString), keyBase64) { }

        public byte[] Encrypt()
        {
            byte[] cipher;

            using (Aes aes = Aes.Create())
            {
                aes.Key = this.key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.GenerateIV();

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var crytpoStream = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var binaryWriter = new BinaryWriter(crytpoStream))
                            {
                                binaryWriter.Write(this.data);
                            }
                        }
                        cipher = msEncrypt.ToArray();
                    }

                }

                using (var msEncrypt = new MemoryStream())
                {
                    using (var binaryWriter = new BinaryWriter(msEncrypt))
                    {
                        binaryWriter.Write(aes.IV);
                        binaryWriter.Write(cipher);
                        binaryWriter.Flush();
                    }
                    return msEncrypt.ToArray();
                }
            }
        }
    }
}
