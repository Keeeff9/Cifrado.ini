using Microsoft.Extensions.Logging;
using sa_doc_encrypt.Domain.Contracts;
using sa_doc_encrypt.Domain.Dto;

namespace sa_doc_encrypt.Infraestructure.Services
{
    public class CryptoOpers : ICryptoOpers
    {
        private readonly ILogger<CryptoOpers> _logger;
        private readonly IDocEncryptClient _client;

        public CryptoOpers(ILogger<CryptoOpers> logger, IDocEncryptClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<CryptoOperationResult> DecryptFile(FileInfo file)
        {
            try
            {
                DecryptFileInput decryptFileInput = new() { File = new StrawberryShake.Upload(file.OpenRead(), file.Name)};
                var decryptResult = await _client.DecryptFile.ExecuteAsync(decryptFileInput);
                var decryptData = decryptResult.Data!.DecryptFile.ResultModelOfString;

                return new CryptoOperationResult
                {
                    Message = decryptData!.Message,
                    Success = decryptData!.Error != "true",
                    Url = decryptData!.Custom2
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DecryptFile");
                return new CryptoOperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<CryptoOperationResult> EncryptFile(FileInfo file)
        {
            try
            {
                EncryptFileInput encryptFileInput = new() { File = new StrawberryShake.Upload(file.OpenRead(), file.Name) };
                var encryptResult = await _client.EncryptFile.ExecuteAsync(encryptFileInput);
                var encryptData = encryptResult.Data!.EncryptFile.ResultModelOfString;

                return new CryptoOperationResult
                {
                    Message = encryptData!.Message,
                    Success = encryptData!.Error != "true",
                    Url = encryptData!.Custom2
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EncryptFile");
                return new CryptoOperationResult { Success = false, Message = ex.Message };
            }
        }
    }
}
