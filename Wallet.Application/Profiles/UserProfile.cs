using AutoMapper;
using Wallet.Application.Models;
using Wallet.Core.Entities;

namespace Wallet.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, User>().ReverseMap();

            CreateMap<UserInfoEntity, UserInfo>().ReverseMap();
        }
    }
}
