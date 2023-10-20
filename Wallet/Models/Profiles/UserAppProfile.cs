using AutoMapper;
using Wallet.Application.Models;
using Wallet.Models.Requests;

namespace Wallet.Models.Profiles
{
    public class UserAppProfile : Profile
    {
        public UserAppProfile()
        {
            CreateMap<UserAuthRequest, User>().ReverseMap()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.HashCode));

            CreateMap<UserRequest, User>().ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.HashCode));

            CreateMap<UserUpdateRequest, User>().ReverseMap()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.HashCode));

            CreateMap<UserInfoRequest, UserInfo>().ReverseMap()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CurrentBalance, opt => opt.MapFrom(src => src.CurrentBalance));

            CreateMap<UserInfoUpdateRequest, UserInfo>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CurrentBalance, opt => opt.MapFrom(src => src.CurrentBalance));
        }
    }
}
