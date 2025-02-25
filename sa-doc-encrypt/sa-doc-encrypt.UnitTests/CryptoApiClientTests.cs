using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sa_doc_encrypt.Domain.Contracts;
using sa_doc_encrypt.Infraestructure.Services;
using sa_exporte_entregables.UnitTests;
using Xunit.Abstractions;

namespace sa_doc_encrypt.UnitTests
{
    public class CryptoApiClientTests
    {
        private readonly ITestOutputHelper _output;

        public CryptoApiClientTests(ITestOutputHelper output) { _output = output; }

        private static void BuildRequiredObjects(out ICryptoApiClient client)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.AddHttpClient<ICryptoApiClient, CryptoApiClient>()
                    .ConfigurePrimaryHttpMessageHandler(() => {
                        return new SocketsHttpHandler
                        {
                            PooledConnectionLifetime = TimeSpan.FromMinutes(1),
                            SslOptions = new System.Net.Security.SslClientAuthenticationOptions
                            {
                                RemoteCertificateValidationCallback = delegate { return true; }
                            }
                        };
                    })
                    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
                })
                .Build();

            client = host.Services.GetRequiredService<ICryptoApiClient>();
        }

        [Fact]
        public async Task MustDownloadApiFileOK()
        {
            BuildRequiredObjects(out ICryptoApiClient client);
            string url = "https://localhost:7086/api/doc-encrypt/GetImage/decrypt_0ea66749-5954-4757-9451-fe6ffaea1ddf_encrypt_configuracion%20SICAD%20CSC_sin_Encriptar_PROD.ini";

            var file = await client.GetFile(url);

            Assert.NotNull(file);
            Assert.True(file.Exists);

            _output.WriteLine($"path: {file.FullName}");
        }
    }
}
