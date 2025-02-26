namespace sa_mti_crypto.Infrastructure.Models
{
    public static class AesConstants
    {
        public const int KEY_SIZE_BITS = 256;
        public const int KEY_SIZE_BYTES = KEY_SIZE_BITS / 8;
        public const int IV_SIZE_BYTES = 16;
    }
}