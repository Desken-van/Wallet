using AutoMapper;
using Wallet.Application.Enum;
using Wallet.Data;
using Wallet.Models;
using Wallet.Models.Response;
using Wallet.Services.Contract;

namespace Wallet.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly ITransactionRepository _transactionRepository;
        private IMapper _mapper;

        public TransactionService(IUserRepository userRepository, IUserInfoRepository userInfoRepository, 
            ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _userInfoRepository = userInfoRepository;
            _mapper = mapper;
        }

        public async Task<TransactionListModel> GetTransactionListModel(string name)
        {
            var user = await _userRepository.GetUserByNameAsync(name);

            var result = new TransactionListModel();

            if (user != null)
            {
                var userInfo = await _userInfoRepository.GetUserInfoByUserIdAsync(user.Id);

                var transactions = await _transactionRepository.GetTransactionByUserIdAsync(user.Id);

                var latestTransaction = new List<TransactionModel>();

                foreach (var transaction in transactions)
                {
                    var model = _mapper.Map<TransactionResponse>(transaction);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        transaction.Icon.Save(stream);
                        model.Icon = stream.ToArray();
                    }

                    latestTransaction.Add(GetTransactionModel(model));
                }

                result = new TransactionListModel
                {
                    CardBalance = new CardBalance()
                    {
                        CurrentBalance = userInfo.CurrentBalance,
                        AvailableBalance = userInfo.AvailableBalance,
                    },

                    StoryPoints = userInfo.StoryPoints,
                    NoPaymentDue = userInfo.NoPaymentDue,
                    LatestTransaction = latestTransaction
                };
            }

            return result;
        }

        private TransactionModel GetTransactionModel(TransactionResponse model)
        {
            var transactionModel = new TransactionModel()
            {
                TransactionName = model.TransactionName,
                SummaryCash = $"${model.SummaryCash}",
                Description = model.Description,
            };


            if (model.AuthorizedUser != null && model.AuthorizedUser != "")
            {
                transactionModel.Date = model.AuthorizedUser + " - ";
            }

            if (model.CreatedDate.Day < DateTime.Now.Day
                && model.CreatedDate.Month == DateTime.Now.Month
                && model.CreatedDate.Year == DateTime.Now.Year
                && (DateTime.Now.Day - model.CreatedDate.Day) <= 7
                && (DateTime.Now.Day - model.CreatedDate.Day) >= 1)
            {
                DayOfWeek wk = model.CreatedDate.DayOfWeek;

                transactionModel.Date += wk.ToString();
            }
            else
            {
                transactionModel.Date += model.CreatedDate.Date.ToString();
            }

            if (model.Type == PaymentType.Payment)
            {
                transactionModel.SummaryCash = $"+${model.SummaryCash}";
            }

            transactionModel.TransactionDetail = new TransactionDetail()
            {
                TransactionId = model.Id,

                TransactionName = transactionModel.TransactionName,

                Description = model.Description,
                CreatedDate = model.CreatedDate,
                Status = "Aproved"
            };

            if (model.Pending)
            {
                transactionModel.Description = "Pending -" + model.Description;
                transactionModel.TransactionDetail.Status = "Pending";
            }

            transactionModel.Icon = model.Icon;

            return transactionModel;
        }
    }
}
