using Microsoft.Extensions.Configuration;

namespace ms_doc_encrypt.Domain.Helpers
{
    public class ConfigurationValues
    {
        private readonly IConfiguration _configuration;
        public string HostRest { get; set; }
        public string CipherKey { get; set; }
        public ConfigurationValues(IConfiguration configuration) 
        {
            HostRest = string.Empty;
            CipherKey = string.Empty;
            _configuration = configuration;
            SetValues();    
        }

        private void SetValues()
        {
            if (Environment.GetEnvironmentVariable("CIPHER_KEY") != null)
            {
                CipherKey = Environment.GetEnvironmentVariable("CIPHER_KEY")!;
            }
            else 
            {
                CipherKey = _configuration.GetValue<string>("CIPHER_KEY")!; 
            }
            HostRest = _configuration.GetValue<string>("HOST_REST")!;
        }
    }
}
