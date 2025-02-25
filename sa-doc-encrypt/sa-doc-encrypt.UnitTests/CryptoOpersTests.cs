using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using sa_doc_encrypt.Domain.Contracts;
using sa_doc_encrypt.Infraestructure;
using sa_doc_encrypt.Infraestructure.Services;
using sa_exporte_entregables.UnitTests;
using Xunit.Abstractions;

namespace sa_doc_encrypt.UnitTests
{
    public class CryptoOpersTests
    {
        private readonly ITestOutputHelper _output;

        public CryptoOpersTests(ITestOutputHelper output) { _output = output; }

        private static void BuildRequiredObjects(out ICryptoOpers opers)
        {
            var builder = new ConfigurationBuilder();
            UTToolkit.BuildConfig(builder);
            IConfiguration config = builder.Build();

            string ambiente = config.GetValue<string>("ambiente") ?? "";
            string uri = config.GetSection("uri").GetValue<string>(ambiente) ?? "";

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services
                        .AddDocEncryptClient()
                        .ConfigureHttpClient(client => client.BaseAddress = new Uri(uri));

                    services.AddScoped<ICryptoOpers, CryptoOpers>();
                })
                .Build();

            opers = host.Services.GetRequiredService<ICryptoOpers>();
        }

        [Fact]
        public async Task MustCryptFileOk()
        {
            BuildRequiredObjects(out ICryptoOpers opers);
            FileInfo file = new FileInfo(Path.Combine(AppContext.BaseDirectory, "inlet-files", "SICAD_config_plano.ini"));

            var result = await opers.EncryptFile(file);

            Assert.NotNull(result);
            Assert.True(result.Success);

            _output.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [Fact]
        public async Task MustDecryptFileOk()
        {
            BuildRequiredObjects(out ICryptoOpers opers);
            FileInfo file = new FileInfo(Path.Combine(AppContext.BaseDirectory, "inlet-files", "SICAD_config_cifrado.ini"));

            var result = await opers.DecryptFile(file);

            Assert.NotNull(result);
            Assert.True(result.Success);

            _output.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }
}
