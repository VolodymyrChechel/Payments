using AutoMapper;
using Payments.BLL.DTO;
using Payments.WEB.Models;

namespace Payments.WEB.Util
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterModel, UserDTO>();
            //CreateMap<UserDTO, UserInfoDTO>();
        }
    }
}