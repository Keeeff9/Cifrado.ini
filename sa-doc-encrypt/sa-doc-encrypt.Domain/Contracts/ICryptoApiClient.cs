namespace sa_doc_encrypt.Domain.Contracts
{
    public interface ICryptoApiClient
    {
        Task<FileInfo?> GetFile(string url);
    }
}
