using Wallet.Application.Models;

namespace Wallet.Data
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUserListAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByNameAsync(string name);
        Task<bool> Create(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}