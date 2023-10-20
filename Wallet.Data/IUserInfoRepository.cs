using Wallet.Application.Models;

namespace Wallet.Data
{
    public interface IUserInfoRepository
    {
        Task<IEnumerable<UserInfo>> GetUserInfoListAsync();
        Task<UserInfo> GetUserInfoByIdAsync(int id);
        Task<UserInfo> GetUserInfoByUserIdAsync(int userid);
        Task<bool> Create(UserInfo userInfo);
        Task UpdateAsync(UserInfo userInfo);
        Task DeleteAsync(int id);
    }
}
