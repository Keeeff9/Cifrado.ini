namespace sa_mti_crypto.Domain.Dto
{
    public sealed class OperationResult
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
        public string? OutputFilePath { get; init; }
    }
}