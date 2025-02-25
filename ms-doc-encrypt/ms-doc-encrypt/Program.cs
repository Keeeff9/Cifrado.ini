using ms_doc_encrypt.Domain.Helpers;
using ms_doc_encrypt.GraphQL;
using ms_doc_encrypt.Infrastructure.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
});

string logFilePath = System.IO.Path.Combine(AppContext.BaseDirectory, "logs", "ms-doc-encrypt.log");

var logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
        .WriteTo.Async(a => { a.File(logFilePath, rollingInterval: RollingInterval.Day); })
        .WriteTo.Async(c => c.Console())
        .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddSingleton<ConfigurationValues>();
builder.Services.AddScoped<EncryptRepository>();
builder.Services.AddScoped<DecryptRepository>();

builder.Services
    .AddGraphQLServer()
    .AddType<UploadType>()
    .AddQueryType<QueriesApi>()
    .AddMutationType<MutationsApi>()
    .AddMutationConventions()
    .ModifyRequestOptions(o => {
        o.ExecutionTimeout = TimeSpan.FromSeconds(300);
    });

builder.Services.AddControllers();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.MapGraphQL("/api/doc-encrypt");
await app.RunAsync();
