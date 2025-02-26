using sa_mti_crypto.Infrastructure.Services;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System.Text;

namespace sa_mti_crypto.UnitTests
{
    public class EncryptRepositoryTests
    {
        private readonly EncryptRepository _encryptor;
        private readonly Mock<ILogger<EncryptRepository>> _loggerMock = new();

        public EncryptRepositoryTests()
        {
            _encryptor = new EncryptRepository(
                new KeyManager(),
                _loggerMock.Object
            );
        }

        [Fact]
        public void EncryptData_SameInput_DifferentOutputDueToIV()
        {
            // Arrange
            var data = Encoding.UTF8.GetBytes("Test Data");
            var password = "P@ssw0rd";

            // Act
            var encrypted1 = _encryptor.EncryptData(data, password);
            var encrypted2 = _encryptor.EncryptData(data, password);

            // Assert
            Assert.NotEqual(encrypted1, encrypted2);
        }

        [Fact]
        public void EncryptData_InvalidFile_ReturnsFailedOperation()
        {
            // Act
            var result = _encryptor.EncryptData("invalid_path.txt", "password", "output_dir");

            // Assert
            Assert.False(result.Success);
            _loggerMock.Verify(log => log.LogError(
                It.IsAny<Exception>(),
                "Error cifrando archivo"),
                Times.Once
            );
        }
    }
}