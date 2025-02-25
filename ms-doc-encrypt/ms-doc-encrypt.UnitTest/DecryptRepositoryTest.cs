using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ms_doc_encrypt.Domain.Contracts;
using ms_doc_encrypt.Infrastructure.Models;
using ms_doc_encrypt.Infrastructure.Services;
using Xunit.Abstractions;

namespace ms_doc_encrypt.UnitTest
{
    public class DecryptRepositoryTest
    {
        private readonly ITestOutputHelper _output;

        public DecryptRepositoryTest(ITestOutputHelper output)
        {
            _output = output;
        }

        private static void BuildRequiredObjects(out IEncryptRepository encryptRepository, out IDecryptRepository decryptRepository) 
        {
            var host = Host.CreateDefaultBuilder()
               .ConfigureServices((context, services) => {
                   services.AddScoped<IEncryptRepository, EncryptRepository>();
                   services.AddScoped<IDecryptRepository, DecryptRepository>();
               })
               .Build();

            encryptRepository = host.Services.GetRequiredService<IEncryptRepository>();
            decryptRepository = host.Services.GetRequiredService<IDecryptRepository>();
        }

        [Fact]
        public void MustCipherDecryptOk() 
        {
            string plainText = "Test Desencriptación";
            string cipherKey = ToolKit.GetKey();
            _output.WriteLine($"key = {cipherKey}");
            BuildRequiredObjects(out IEncryptRepository encryptRepository, out IDecryptRepository decryptRepository);

            var data1 = encryptRepository.EncryptData(plainText, cipherKey);
            var data2 = encryptRepository.EncryptData(plainText,cipherKey);
            var plain1 = decryptRepository.DecryptData(data1, cipherKey);
            var plain2 = decryptRepository.DecryptData(data2, cipherKey);

            Assert.NotNull(plain1);
            Assert.NotNull(plain2);
            Assert.NotEmpty(plain1);
            Assert.NotEmpty(plain2);
            Assert.Equal(plain1, plain2);

            _output.WriteLine($"Data1 = {ToolKit.GetBase64StringFromCipherBytes(data1)}");
            _output.WriteLine($"Data2 = {ToolKit.GetBase64StringFromCipherBytes(data2)}");
            _output.WriteLine($"Plain1 = {ToolKit.GetStringFromCipherBytes(plain1)}");
            _output.WriteLine($"Plain2 = {ToolKit.GetStringFromCipherBytes(plain2)}");
        }
    }
}
