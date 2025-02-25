namespace ms_doc_encrypt.Domain.Contracts
{
    public interface IEncryptRepository
    {
        byte[] EncryptData(FileInfo fileInfo, string keyBase64);

        byte[] EncryptData(string plainText, string keyBase64);

        byte[] EncryptData(byte[] data, string keyBase64);
    }
}
