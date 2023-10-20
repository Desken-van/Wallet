using Wallet.Models.Response;

namespace Wallet.Models
{
    public class TransactionListModel
    {
        public CardBalance CardBalance {  get; set; }

        public string StoryPoints { get; set; }

        public string NoPaymentDue { get; set; }

        public List<TransactionModel> LatestTransaction {  get; set; } 
    }

    public class TransactionModel
    {
        public string TransactionName { get; set; }

        public string Description { get; set; }

        public string SummaryCash { get; set; }

        public string Date { get; set; }

        public TransactionDetail TransactionDetail { get; set; }

        public byte[] Icon { get; set; }
    }

    public class CardBalance 
    {
        public double CurrentBalance { get; set; }

        public double AvailableBalance { get; set; }
    }

    public class TransactionDetail
    {
        public int TransactionId { get; set; }

        public string TransactionName { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
