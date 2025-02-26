using sa_mti_crypto.Domain.Contracts;
using sa_mti_crypto.Domain.Dto;
using sa_mti_crypto.Infrastructure.Models;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using System.Text;

namespace sa_mti_crypto.Infrastructure.Services
{
    public class EncryptRepository : IEncryptRepository
    {
        private readonly IKeyManager _keyManager;
        private readonly ILogger<EncryptRepository> _logger;

        public EncryptRepository(
            IKeyManager keyManager,
            ILogger<EncryptRepository> logger
        )
        {
            _keyManager = keyManager ?? throw new ArgumentNullException(nameof(keyManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public OperationResult EncryptData(string filePath, string password, string outputDirectory)
        {
            try
            {
                _logger.LogInformation("Cifrando archivo: {FilePath}", Path.GetFileName(filePath));

                byte[] data = ToolKit.ReadFile(filePath);
                byte[] encrypted = EncryptData(data, password);
                string outputPath = ToolKit.GenerateOutputPath(filePath, encrypt: true, outputDirectory);

                ToolKit.SaveFile(encrypted, outputPath);
                ToolKit.WipeData(encrypted);

                return new OperationResult
                {
                    Success = true,
                    OutputFilePath = outputPath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cifrando archivo");
                return new OperationResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public byte[] EncryptData(byte[] data, string password)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("Datos inválidos", nameof(data));

            using var aes = Aes.Create();
            byte[] salt = _keyManager.GenerateSalt();
            byte[] key = _keyManager.DeriveKeyFromPassword(password, salt);

            try
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateIV();

                using var memoryStream = new MemoryStream();
                memoryStream.Write(salt, 0, 32);
                memoryStream.Write(aes.IV, 0, aes.IV.Length);

                using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                }

                return memoryStream.ToArray();
            }
            finally
            {
                Array.Clear(key, 0, key.Length);
            }
        }

        public byte[] EncryptData(FileInfo fileInfo, string password)
        {
            throw new NotImplementedException();
        }

        public byte[] EncryptData(string plainText, string password)
        {
            throw new NotImplementedException();
        }
    }
}