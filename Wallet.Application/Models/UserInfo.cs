namespace Wallet.Application.Models
{
    public class UserInfo
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public double CurrentBalance { get; set; }

        public double AvailableBalance { get; set; }

        public string StoryPoints { get; set; }

        public string NoPaymentDue { get; set; }
    }
}
