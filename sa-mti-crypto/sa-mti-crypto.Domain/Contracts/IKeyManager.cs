namespace sa_mti_crypto.Domain.Contracts
{
    public interface IKeyManager
    {
        byte[] DeriveKeyFromPassword(string password, byte[] salt);
        byte[] GenerateSalt();
    }
}