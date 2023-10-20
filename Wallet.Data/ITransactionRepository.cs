using Wallet.Application.Models;

namespace Wallet.Data
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetTransactionListAsync();
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task<List<Transaction>> GetTransactionByUserIdAsync(int userid);
        Task<bool> Create(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id);
    }
}
