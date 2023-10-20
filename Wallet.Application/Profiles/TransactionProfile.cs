using AutoMapper;
using Wallet.Application.Models;
using Wallet.Core.Entities;

namespace Wallet.Application.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<TransactionEntity, Transaction>().ReverseMap();
        }
    }
}
