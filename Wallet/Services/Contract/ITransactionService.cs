using Wallet.Models;

namespace Wallet.Services.Contract
{
    public interface ITransactionService
    {
        Task<TransactionListModel> GetTransactionListModel(string name);
    }
}
