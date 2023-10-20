using AutoMapper;
using Wallet.Application.Models;
using Wallet.Core.Entities;
using Wallet.Core;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Wallet.Data.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly WalletContext _dbContext;

        private readonly IMapper _mapper;

        public TransactionRepository(WalletContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionListAsync()
        {
            var transactions = await _dbContext.Transactions.ToListAsync();

            if (transactions != null)
            {
                var result = new List<Transaction>();

                foreach (var infoEntity in transactions)
                {
                    result.Add(_mapper.Map<Transaction>(infoEntity));
                }

                return result;
            }

            return null;
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            var transaction = await _dbContext.Transactions.FirstOrDefaultAsync(x => x.Id == id);

            if (transaction != null)
            {
                var result = _mapper.Map<Transaction>(transaction);

                return result;
            }

            return null;
        }

        public async Task<List<Transaction>> GetTransactionByUserIdAsync(int userid)
        {
            var list = await GetTransactionListAsync();

            var result = new List<Transaction>();

            var transactions = from transaction in list
                               where transaction.UserId == userid
                               select transaction;

            if (transactions != null && transactions.Any())
            {
                foreach (var transaction in transactions)
                {
                    var model = _mapper.Map<Transaction>(transaction);
                    result.Add(model);
                }

                return result;
            }

            return null;
        }

        public async Task<bool> Create(Transaction transaction)
        {
            if (transaction != null)
            {
                var icon_stream = new FileStream("icon.ico", FileMode.Open);

                transaction.Icon = new Icon(icon_stream);
                transaction.CreatedDate = DateTime.Now;

                var result = _mapper.Map<TransactionEntity>(transaction);
                await _dbContext.Transactions.AddAsync(result);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            var original = await _dbContext.Transactions.FindAsync(transaction.Id);

            var icon_stream = new FileStream("icon.ico", FileMode.Open);

            transaction.Icon = original.Icon;
            transaction.CreatedDate = original.CreatedDate;

            if (original != null)
            {
                _dbContext.Entry(original).CurrentValues.SetValues(transaction);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = await _dbContext.Transactions.FindAsync(id);

            if (transaction != null)
                await Task.Run(() => _dbContext.Transactions.Remove(transaction));

            await _dbContext.SaveChangesAsync();
        }
    }
}
