using sa_doc_encrypt.Domain.Dto;

namespace sa_doc_encrypt.Domain.Contracts
{
    public interface ICryptoOpers
    {
        Task<CryptoOperationResult> EncryptFile(FileInfo file);
        Task<CryptoOperationResult> DecryptFile(FileInfo file);
    }
}
