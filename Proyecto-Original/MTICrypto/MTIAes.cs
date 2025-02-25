using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTICrypto
{
    public abstract class MTIAes
    {
        protected byte[]? data;
        protected byte[]? key;

        private MTIAes() { }

        public MTIAes(byte[] data, string keyBase64)
        {
            ValidateArguments(data, keyBase64);
        }

        private void ValidateArguments(byte[] data, string keyBase64)
        {
            if (data == null || keyBase64 == null) { throw new ArgumentNullException(); }
            ValidateKey(keyBase64);
            this.data = data;
        }

        private void ValidateKey(string keyBase64)
        {
            this.key = Convert.FromBase64String(keyBase64);
            if (this.key.Length != Constants.KEY_SIZE_BYTES) { throw new ArgumentException("Tamaño de llave no válido"); }
        }
    }
}
