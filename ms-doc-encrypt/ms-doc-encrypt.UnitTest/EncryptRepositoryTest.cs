using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ms_doc_encrypt.Domain.Contracts;
using ms_doc_encrypt.Infrastructure.Models;
using ms_doc_encrypt.Infrastructure.Services;
using Xunit.Abstractions;

namespace ms_doc_encrypt.UnitTest
{
    public class EncryptRepositoryTest
    {
        private readonly ITestOutputHelper _outputHelper;

        public EncryptRepositoryTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        private static void BuildRequiredObjects(out IEncryptRepository repository) 
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => { 
                    services.AddScoped<IEncryptRepository,EncryptRepository>();
                })
                .Build();

            repository = host.Services.GetRequiredService<IEncryptRepository>();
        }

        [Fact]
        public void MustCipherEncryptOk()
        {
            string textoPlano = "Test de encriptación";
            BuildRequiredObjects(out var repository);
            string cipherKey = ToolKit.GetKey();
            byte[]data1 = repository.EncryptData(textoPlano,cipherKey);
            byte[]data2 = repository.EncryptData(textoPlano,cipherKey);

            Assert.NotNull(data1);
            Assert.NotNull(data2);
            Assert.NotEmpty(data1);
            Assert.NotEmpty(data2);
            Assert.NotEqual(data1, data2);

            _outputHelper.WriteLine($"Data1 = {ToolKit.GetBase64StringFromCipherBytes(data1)}");
            _outputHelper.WriteLine($"Data2 = {ToolKit.GetBase64StringFromCipherBytes(data2)}");
        }
    }
}