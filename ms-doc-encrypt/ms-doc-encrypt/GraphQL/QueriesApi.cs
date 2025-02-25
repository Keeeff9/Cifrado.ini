using ms_doc_encrypt.Domain.Dto;

namespace ms_doc_encrypt.GraphQL
{
    public class QueriesApi
    {
        public OperationResult GetVersion()
        {
            return new OperationResult { Message = "v1.0.0", Success = true };
        }
    }
}
