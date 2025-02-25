using Microsoft.Extensions.Logging;
using ms_doc_encrypt.Domain.Contracts;
using ms_doc_encrypt.Infrastructure.Models;
using System.Security.Cryptography;

namespace ms_doc_encrypt.Infrastructure.Services
{
    public class DecryptRepository : IDecryptRepository
    {
        private readonly ILogger<DecryptRepository> _logger;

        public DecryptRepository(ILogger<DecryptRepository> logger)
        {
            _logger = logger;
        }

        public byte[] DecryptData(byte[] data, string keyBase64)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Convert.FromBase64String(keyBase64);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    byte[] cryptoIV = new byte[AesConstants.IV_SIZE_BYTES];
                    Array.Copy(data, cryptoIV, AesConstants.IV_SIZE_BYTES);
                    aes.IV = cryptoIV;

                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        using (var msDecrypt = new MemoryStream())
                        {
                            using (var decryptoStream = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                            {
                                using (var binaryWriter = new BinaryWriter(decryptoStream))
                                {
                                    binaryWriter.Write(data, AesConstants.IV_SIZE_BYTES, data.Length - AesConstants.IV_SIZE_BYTES);
                                }
                            }
                            return msDecrypt.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DecryptData Array");
                return [];
            }
        }

        public byte[] DecryptData(string base64data, string keyBase64)
        {
            try
            {
                byte[] data = ToolKit.GetBytesFromBase64String(base64data);
                return DecryptData(data, keyBase64);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DecryptData String");
                return [];
            }
        }
    }
}
