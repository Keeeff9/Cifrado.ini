using sa_mti_crypto.Infrastructure.Services;
using Xunit;
using Xunit.Abstractions;
using System.Security.Cryptography;

namespace sa_mti_crypto.UnitTests
{
    public class KeyManagerTests
    {
        private readonly KeyManager _keyManager = new();

        [Fact]
        public void DeriveKeyFromPassword_SameInputs_ProducesSameKey()
        {
            // Arrange
            var password = "SecurePassword123";
            var salt = new byte[32];
            RandomNumberGenerator.Fill(salt);

            // Act
            var key1 = _keyManager.DeriveKeyFromPassword(password, salt);
            var key2 = _keyManager.DeriveKeyFromPassword(password, salt);

            // Assert
            Assert.Equal(key1, key2);
        }

        [Fact]
        public void GenerateSalt_Always_ProducesUniqueSalts()
        {
            // Act
            var salt1 = _keyManager.GenerateSalt();
            var salt2 = _keyManager.GenerateSalt();

            // Assert
            Assert.NotEqual(salt1, salt2);
        }

        [Fact]
        public void DeriveKeyFromPassword_InvalidSalt_ThrowsException()
        {
            // Arrange
            var invalidSalt = new byte[16]; // Deberían ser 32 bytes

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _keyManager.DeriveKeyFromPassword("password", invalidSalt));
        }
    }
}