namespace sa_mti_crypto.Domain.Contracts
{
    public interface IDecryptRepository
    {
        byte[] DecryptData(byte[] encryptedData, string password);
        byte[] DecryptData(string base64data, string password);
    }
}