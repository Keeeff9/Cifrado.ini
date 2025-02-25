using ms_doc_encrypt.Domain.Dto;
using ms_doc_encrypt.Domain.Helpers;
using ms_doc_encrypt.Infrastructure.Services;
using Path = System.IO.Path;

namespace ms_doc_encrypt.GraphQL
{
    public class MutationsApi
    {
        public async Task<CipherOperationResult> EncryptFile(
            [Service] EncryptRepository encryptRepository, [Service] ILogger<MutationsApi> logger,
            [Service] ConfigurationValues configuration, IFile file)
        {
			try
			{
                string workPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                using (var stream = File.Create(workPath))
                {
                    await file.CopyToAsync(stream);
                }

                byte[] data = await File.ReadAllBytesAsync(workPath);
                File.Delete(workPath);
                byte[] cipherData = encryptRepository.EncryptData(data,configuration.CipherKey);

                if (cipherData.Length == 0) 
                {
                    throw new ArgumentException("Falla en cifrado");
                }

                string workdir = Path.Combine(AppContext.BaseDirectory, "downloads");
                if (!Directory.Exists(workdir)) { Directory.CreateDirectory(workdir); }
                await File.WriteAllBytesAsync(Path.Combine(workdir, file.Name), cipherData);
                string url = $"{configuration.HostRest}{file.Name}".Replace(" ", "%20");
                return new CipherOperationResult { Success = true, UrlLink = url };
            }
			catch (Exception ex)
			{
				logger.LogError(ex, "EncryptFile");
				return new CipherOperationResult { Message = ex.Message, Success = false };
			}            
        }

        public async Task<CipherOperationResult> DecryptFile(
           [Service] DecryptRepository decryptRepository, [Service] ILogger<MutationsApi> logger,
           [Service] ConfigurationValues configuration, IFile file)
        {
            try
            {
                string workPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                using (var stream = File.Create(workPath))
                {
                    await file.CopyToAsync(stream);
                }

                byte[] data = await File.ReadAllBytesAsync(workPath);
                File.Delete(workPath);
                byte[] plainData = decryptRepository.DecryptData(data, configuration.CipherKey);

                if (plainData.Length == 0)
                {
                    throw new ArgumentException("Falla en descifrado");
                }

                string workdir = Path.Combine(AppContext.BaseDirectory, "downloads");
                if (!Directory.Exists(workdir)) { Directory.CreateDirectory(workdir); }
                await File.WriteAllBytesAsync(Path.Combine(workdir, file.Name), plainData);
                string url = $"{configuration.HostRest}{file.Name}".Replace(" ", "%20");
                return new CipherOperationResult { Success = true, UrlLink = url };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DecryptFile");
                return new CipherOperationResult { Message = ex.Message, Success = false };
            }
        }
    }
}
