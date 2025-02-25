namespace ms_doc_encrypt.Infrastructure.Models
{
    public abstract class MtiAes
    {
        protected byte[]? data;
        protected byte[]? key;

        private MtiAes() { }

        protected MtiAes(byte[] data, string keyBase64)
        {
            ValidateArguments(data, keyBase64);
        }

        private void ValidateArguments(byte[] data, string keybase64)
        {
            if (data == null || keybase64 == null)
            {
                throw new ArgumentException("Datos o llave nulos");
            }

            this.data = data;
            Validatekey(keybase64);
        }

        private void Validatekey(string keyBase64)
        {
            this.key = Convert.FromBase64String(keyBase64);

            if (this.key.Length != AesConstants.KEY_SIZE_BYTES)
            {
                throw new ArgumentException("Tamaño de llave no valido");
            }
        }
    }
}
