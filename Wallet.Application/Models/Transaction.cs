using System;
using System.Drawing;
using Wallet.Application.Enum;

namespace Wallet.Application.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public PaymentType Type { get; set; }
        public double SummaryCash { get; set; }
        public string TransactionName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Pending { get; set; }
        public string? AuthorizedUser { get; set; }
        public Icon Icon { get; set; }
    }
}
