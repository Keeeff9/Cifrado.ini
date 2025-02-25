namespace ms_doc_encrypt.Domain.Contracts
{
    public interface IDecryptRepository
    {
        byte[] DecryptData(byte[] data, string keyBase64);
        byte[] DecryptData(string base64data, string keyBase64);
    }
}
