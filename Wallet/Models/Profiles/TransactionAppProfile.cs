using AutoMapper;
using Wallet.Application.Models;
using Wallet.Models.Requests;
using Wallet.Models.Response;

namespace Wallet.Models.Profiles
{
    public class TransactionAppProfile : Profile
    {
        public TransactionAppProfile()
        {
            CreateMap<TransactionRequest, Transaction>().ReverseMap()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.SummaryCash, opt => opt.MapFrom(src => src.SummaryCash))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.TransactionName, opt => opt.MapFrom(src => src.TransactionName))
                .ForMember(dest => dest.Pending, opt => opt.MapFrom(src => src.Pending))
                .ForMember(dest => dest.AuthorizedUser, opt => opt.MapFrom(src => src.AuthorizedUser))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<TransactionUpdateRequest, Transaction>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.SummaryCash, opt => opt.MapFrom(src => src.SummaryCash))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.TransactionName, opt => opt.MapFrom(src => src.TransactionName))
                .ForMember(dest => dest.Pending, opt => opt.MapFrom(src => src.Pending))
                .ForMember(dest => dest.AuthorizedUser, opt => opt.MapFrom(src => src.AuthorizedUser))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Transaction, TransactionResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.SummaryCash, opt => opt.MapFrom(src => src.SummaryCash))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.TransactionName, opt => opt.MapFrom(src => src.TransactionName))
                .ForMember(dest => dest.Pending, opt => opt.MapFrom(src => src.Pending))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.AuthorizedUser, opt => opt.MapFrom(src => src.AuthorizedUser))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom<IconToByteArrayResolver>());
        }

    }

    public class IconToByteArrayResolver : IValueResolver<Transaction, TransactionResponse, byte[]>
    {
        public byte[] Resolve(Transaction source, TransactionResponse destination, byte[] destMember, ResolutionContext context)
        {
            if (source.Icon == null)
            {
                return null;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                source.Icon.Save(stream);
                return stream.ToArray();
            }
        }
    }
}
