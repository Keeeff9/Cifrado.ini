using sa_mti_crypto.Infrastructure.Services;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Security.Cryptography;

namespace sa_mti_crypto.UnitTests
{
    public class DecryptRepositoryTests
    {
        private readonly DecryptRepository _decryptor;
        private readonly EncryptRepository _encryptor;
        private readonly Mock<ILogger<DecryptRepository>> _loggerMock = new();

        public DecryptRepositoryTests()
        {
            var keyManager = new KeyManager();
            _decryptor = new DecryptRepository(keyManager, _loggerMock.Object);
            _encryptor = new EncryptRepository(keyManager, Mock.Of<ILogger<EncryptRepository>>());
        }

        [Fact]
        public void DecryptData_ValidCipher_ReturnsOriginalData()
        {
            // Arrange
            var original = Encoding.UTF8.GetBytes("Datos secretos");
            var encrypted = _encryptor.EncryptData(original, "password");

            // Act
            var decrypted = _decryptor.DecryptData(encrypted, "password");

            // Assert
            Assert.Equal(original, decrypted);
        }

        [Fact]
        public void DecryptData_InvalidCipher_ThrowsException()
        {
            // Arrange
            var invalidData = new byte[10]; // Demasiado corto

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _decryptor.DecryptData(invalidData, "password"));
        }

        [Fact]
        public void DecryptData_WrongPassword_LogsError()
        {
            // Arrange
            var original = Encoding.UTF8.GetBytes("Test");
            var encrypted = _encryptor.EncryptData(original, "right_password");

            // Act
            var result = _decryptor.DecryptData("fake_path.enc", "wrong_password", "output_dir");

            // Assert
            Assert.False(result.Success);
            _loggerMock.Verify(log => log.LogError(
                It.IsAny<Exception>(),
                "Error descifrando archivo"),
                Times.Once
            );
        }
    }
}