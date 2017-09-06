using AutoMapper;
using Payments.BLL.DTO;
using Payments.WEB.Areas.Admin.Models;
using Payments.WEB.Models;

namespace Payments.WEB.Util
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterModel, UserDTO>();
            CreateMap<DebitAccountDTO, DebitAccountViewModel>();
            CreateMap<DebitAccountViewModel, DebitAccountDTO>();
            CreateMap<CardViewModel, CardDto>();
            CreateMap<PaymentViewModel, PaymentDTO>();
            CreateMap<PaymentDTO, PaymentViewModel>();

            //CreateMap<UserDTO, UserInfoDTO>();
        }
    }
}