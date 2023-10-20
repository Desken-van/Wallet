using AutoMapper;
using Wallet.Application.Models;
using Wallet.Core.Entities;
using Wallet.Core;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Wallet.Data.Helpers;
using Wallet.Data.Factory.Base;

namespace Wallet.Data.Repository
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly WalletContext _dbContext;

        private readonly IMapper _mapper;

        public UserInfoRepository(WalletContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserInfo>> GetUserInfoListAsync()
        {
            var infoList = await _dbContext.UserInfos.ToListAsync();

            if (infoList != null)
            {
                var result = new List<UserInfo>();

                foreach (var infoEntity in infoList)
                {
                    result.Add(_mapper.Map<UserInfo>(infoEntity));
                }

                return result;
            }

            return null;
        }

        public async Task<UserInfo> GetUserInfoByIdAsync(int id)
        {
            var userInfo = await _dbContext.UserInfos.FirstOrDefaultAsync(x => x.Id == id);

            if (userInfo != null)
            {
                var result = _mapper.Map<UserInfo>(userInfo);

                return result;
            }

            return null;
        }

        public async Task<UserInfo> GetUserInfoByUserIdAsync(int userid)
        {
            var userInfo = await _dbContext.UserInfos.FirstOrDefaultAsync(x => x.UserId == userid);

            if (userInfo != null)
            {
                var result = _mapper.Map<UserInfo>(userInfo);

                return result;
            }

            return null;
        }

        public async Task<bool> Create(UserInfo userInfo)
        {
            if (userInfo != null)
            {
                var list = await GetUserInfoListAsync();

                var checkCopy = list.Any(d => d.UserId == userInfo.UserId);

                if (!checkCopy)
                {
                    var month = GetMonth();

                    userInfo.StoryPoints = GetStoryPoint();
                    userInfo.NoPaymentDue = $"You’ve paid your {month} balance";
                    userInfo.AvailableBalance = 1500.00 - userInfo.CurrentBalance;

                    var result = _mapper.Map<UserInfoEntity>(userInfo);
                    await _dbContext.UserInfos.AddAsync(result);
                    await _dbContext.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }

        public async Task UpdateAsync(UserInfo userInfo)
        {
            var original = await _dbContext.UserInfos.FindAsync(userInfo.Id);

            if (original != null)
            {
                var month = GetMonth();

                userInfo.StoryPoints = GetStoryPoint();
                userInfo.NoPaymentDue = $"You’ve paid your {month} balance";
                userInfo.AvailableBalance = 1500.00 - userInfo.CurrentBalance;

                _dbContext.Entry(original).CurrentValues.SetValues(userInfo);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var userInfo = await _dbContext.UserInfos.FindAsync(id);

            if (userInfo != null)
                await Task.Run(() => _dbContext.UserInfos.Remove(userInfo));

            await _dbContext.SaveChangesAsync();
        }

        private string GetStoryPoint()
        {
            var myDt = DateTime.UtcNow;

            var yearTable = new YearTable(myDt);

            var season = yearTable.CheckDate(myDt);

            var calculator = StoryPointFactory.CreateStoryPoint(myDt, season);

            var result = calculator.Calculate(myDt, season);

            return result;
        }

        private string GetMonth()
        {
            var ci = new CultureInfo("en-US");

            var dtfi = ci.DateTimeFormat;

            var myDt = DateTime.UtcNow;

            foreach (var name in dtfi.MonthGenitiveNames)
            {
                if (dtfi.MonthGenitiveNames.ToList().IndexOf(name) == myDt.Month - 1)
                {
                    return name;
                }
            }

            return string.Empty;
        }
    }
}
