using AutoMapper;
using Payments.BLL.DTO;
using Payments.DAL.Entities;

namespace Payments.BLL.Util
{
    public class BLLUserProfile : Profile
    {
        public BLLUserProfile()
        {
            CreateMap<ClientProfile, UserInfoDTO>().ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.ApplicationUser.Email));
            CreateMap<DebitAccount, DebitAccountDTO>();
            CreateMap<DebitAccountDTO, DebitAccount>();
            //CreateMap<UserDTO, UserInfoDTO>();
        }
    }
}