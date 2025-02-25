using Microsoft.Extensions.Logging;
using sa_doc_encrypt.Domain.Contracts;

namespace sa_doc_encrypt.Infraestructure.Services
{
    public class CryptoApiClient : ICryptoApiClient
    {
        private readonly ILogger<CryptoApiClient> _logger;
        private readonly HttpClient _httpClient;

        public CryptoApiClient(ILogger<CryptoApiClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<FileInfo?> GetFile(string url)
        {
            try
            {
                FileInfo file = BuildFileInfo(url);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                using (FileStream fs = new FileStream(file.FullName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await response.Content.CopyToAsync(fs);
                }

                return file;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFile, url: {@Url}", url);
                return null;
            }
        }

        private static FileInfo BuildFileInfo(string url)
        {
            string workdir = Path.Combine(AppContext.BaseDirectory, "downloads");
            if(!Directory.Exists(workdir)) { Directory.CreateDirectory(workdir); }

            return new FileInfo(Path.Combine(workdir, Path.GetFileName(url)));
        }
    }
}
