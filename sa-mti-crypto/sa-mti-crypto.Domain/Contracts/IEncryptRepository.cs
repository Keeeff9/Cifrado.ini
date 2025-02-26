namespace sa_mti_crypto.Domain.Contracts
{
    public interface IEncryptRepository
    {
        byte[] EncryptData(FileInfo fileInfo, string password);
        byte[] EncryptData(string plainText, string password);
        byte[] EncryptData(byte[] data, string password);
    }
}