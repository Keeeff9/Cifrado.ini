using Microsoft.Extensions.Logging;
using sa_mti_crypto.Domain.Contracts;
using sa_mti_crypto.Infrastructure.Models;
using System.Security.Cryptography;
using System.Text;

namespace sa_mti_crypto.Infrastructure.Services
{
    public sealed class KeyManager : IKeyManager
    {
        private const int Iterations = 600_000;
        private readonly ILogger<KeyManager>? _logger;

        public KeyManager(ILogger<KeyManager>? logger = null)
        {
            _logger = logger;
        }

        public byte[] DeriveKeyFromPassword(string password, byte[] salt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password no puede estar vacía", nameof(password));

            if (salt == null || salt.Length != 32)
                throw new ArgumentException("Salt debe ser de 32 bytes", nameof(salt));

            byte[] passwordBytes = Array.Empty<byte>();
            try
            {
                passwordBytes = Encoding.UTF8.GetBytes(password);
                using var pbkdf2 = new Rfc2898DeriveBytes(
                    passwordBytes,
                    salt,
                    Iterations,
                    HashAlgorithmName.SHA512
                );
                return pbkdf2.GetBytes(AesConstants.KEY_SIZE_BYTES);
            }
            catch (Exception ex)
            {
                _logger?.LogError("Error derivando clave: {Message}", ex.Message);
                throw new CryptographicException("Error derivando clave", ex);
            }
            finally
            {
                Array.Clear(passwordBytes, 0, passwordBytes.Length);
            }
        }

        public byte[] GenerateSalt()
        {
            try
            {
                byte[] salt = new byte[32];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(salt);
                return salt;
            }
            catch (Exception ex)
            {
                _logger?.LogError("Error generando salt: {Message}", ex.Message);
                throw new CryptographicException("Error generando salt", ex);
            }
        }
    }
}