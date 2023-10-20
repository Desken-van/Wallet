using Wallet.Application.Enum;

namespace Wallet.Models.Requests
{
    public class TransactionRequest
    {
        public int UserId { get; set; }
        public PaymentType Type { get; set; }
        public double SummaryCash { get; set; }
        public string TransactionName { get; set; }
        public string Description { get; set; }
        public bool Pending { get; set; }
        public string? AuthorizedUser { get; set; }
    }

    public class TransactionUpdateRequest : TransactionRequest
    {
        public int Id { get; set; }
    }
}
