using ms_doc_encrypt.Domain.Contracts;
using ms_doc_encrypt.Infrastructure.Models;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace ms_doc_encrypt.Infrastructure.Services
{
    public class EncryptRepository:IEncryptRepository
    {
        private readonly ILogger<EncryptRepository> _logger;

        public EncryptRepository(ILogger<EncryptRepository> logger) 
        {
            _logger = logger;
        }

        public byte[] EncryptData(FileInfo fileInfo, string keyBase64)
        {
            try
            {
                byte[] data = File.ReadAllBytes(fileInfo.FullName);
                return EncryptData(data, keyBase64);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"EncryptData File");
                return [];
            }
        }

        public byte[] EncryptData(string plainText, string keyBase64)
        {
            try
            {
                byte[] data = ToolKit.GetBytesFromPlainString(plainText);
                return EncryptData(data,keyBase64);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EncryptData String");
                return [];
            }
        }

        public byte[] EncryptData(byte[] data, string keyBase64)
        {
            try
            {
                byte[] cipher;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Convert.FromBase64String(keyBase64);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    aes.GenerateIV();

                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    {
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (var binaryWriter = new BinaryWriter(cryptoStream))
                                {
                                    binaryWriter.Write(data);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "EncryptData Array");
                return [];
            }
        }
    }
}
