using sa_mti_crypto.Domain.Contracts;
using sa_mti_crypto.Domain.Dto;
using sa_mti_crypto.Infrastructure.Models;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace sa_mti_crypto.Infrastructure.Services
{
    public class DecryptRepository : IDecryptRepository
    {
        private readonly IKeyManager _keyManager;
        private readonly ILogger<DecryptRepository> _logger;

        public DecryptRepository(
            IKeyManager keyManager,
            ILogger<DecryptRepository> logger
        )
        {
            _keyManager = keyManager ?? throw new ArgumentNullException(nameof(keyManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public OperationResult DecryptData(string filePath, string password, string outputDirectory)
        {
            try
            {
                _logger.LogInformation("Descifrando archivo: {FilePath}", Path.GetFileName(filePath));

                byte[] data = ToolKit.ReadFile(filePath);
                byte[] decrypted = DecryptData(data, password);
                string outputPath = ToolKit.GenerateOutputPath(filePath, encrypt: false, outputDirectory);

                ToolKit.SaveFile(decrypted, outputPath);
                ToolKit.WipeData(decrypted);

                return new OperationResult
                {
                    Success = true,
                    OutputFilePath = outputPath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error descifrando archivo");
                return new OperationResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public byte[] DecryptData(byte[] encryptedData, string password)
        {
            if (encryptedData == null || encryptedData.Length < 48)
                throw new ArgumentException("Datos cifrados inválidos", nameof(encryptedData));

            byte[] salt = new byte[32];
            byte[] iv = new byte[AesConstants.IV_SIZE_BYTES];
            Buffer.BlockCopy(encryptedData, 0, salt, 0, 32);
            Buffer.BlockCopy(encryptedData, 32, iv, 0, AesConstants.IV_SIZE_BYTES);

            byte[] key = _keyManager.DeriveKeyFromPassword(password, salt);
            byte[] cipherText = new byte[encryptedData.Length - 48];
            Buffer.BlockCopy(encryptedData, 48, cipherText, 0, cipherText.Length);

            try
            {
                using var aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using var memoryStream = new MemoryStream();
                using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(cipherText, 0, cipherText.Length);
                }

                return memoryStream.ToArray();
            }
            finally
            {
                Array.Clear(key, 0, key.Length);
            }
        }

        public byte[] DecryptData(string base64data, string password)
        {
            throw new NotImplementedException();
        }
    }
}