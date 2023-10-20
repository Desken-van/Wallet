using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.Models;
using Wallet.Core;
using Wallet.Core.Entities;

namespace Wallet.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly WalletContext _dbContext;

        private readonly IMapper _mapper;

        public UserRepository(WalletContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetUserListAsync()
        {
            var userList = await _dbContext.Users.ToListAsync();

            if (userList != null)
            {
                var result = new List<User>();

                foreach (var device in userList)
                {
                    result.Add(_mapper.Map<User>(device));
                }

                return result;
            }

            return null;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                var result = _mapper.Map<User>(user);

                return result;
            }

            return null;
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Name == name);

            if (user != null)
            {
                var result = _mapper.Map<User>(user);

                return result;
            }

            return null;
        }

        public async Task<bool> Create(User user)
        {
            if (user != null)
            {
                var list = await GetUserListAsync();

                var checkCopy = list.Any(d => d.Name == user.Name);

                if (!checkCopy)
                {
                    var result = _mapper.Map<UserEntity>(user);
                    await _dbContext.Users.AddAsync(result);
                    await _dbContext.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }

        public async Task UpdateAsync(User user)
        {
            var original = await _dbContext.Users.FindAsync(user.Id);

            if (original != null)
            {
                user.CreatedDate = original.CreatedDate;

                _dbContext.Entry(original).CurrentValues.SetValues(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if (user != null)
                await Task.Run(() => _dbContext.Users.Remove(user));

            await _dbContext.SaveChangesAsync();
        }
    }
}