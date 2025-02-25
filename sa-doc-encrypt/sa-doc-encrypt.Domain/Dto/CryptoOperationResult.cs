namespace sa_doc_encrypt.Domain.Dto
{
    public class CryptoOperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
