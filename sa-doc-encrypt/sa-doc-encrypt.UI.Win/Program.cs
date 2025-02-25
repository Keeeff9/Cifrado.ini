using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using sa_doc_encrypt.Domain.Contracts;
using sa_doc_encrypt.Infraestructure.Services;
using Serilog;

namespace sa_doc_encrypt.UI.Win
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            string logFilePath = Path.Combine(AppContext.BaseDirectory, "logs", "sa-doc-encrypt.log");

            var logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Async(f => f.File(logFilePath, rollingInterval: RollingInterval.Day))
                    .CreateLogger();

            var builder = new ConfigurationBuilder();
            BuildConfig(builder);
            IConfiguration config = builder.Build();

            string ambiente = config.GetValue<string>("ambiente") ?? "";
            string uri = config.GetSection("uri").GetValue<string>(ambiente) ?? "";

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services
                        .AddDocEncryptClient()
                        .ConfigureHttpClient(client => client.BaseAddress = new Uri(uri));

                    services.AddScoped<ICryptoOpers, CryptoOpers>();

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

                    services.AddLogging(builder => {
                        builder.ClearProviders();
                        builder.AddSerilog(logger);
                    });
                })
                .Build();

            FrmMain frmMain = ActivatorUtilities.CreateInstance<FrmMain>(host.Services);
            Application.Run(frmMain);
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }

    }
}