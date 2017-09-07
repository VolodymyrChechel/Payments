using AutoMapper;
using Payments.BLL.DTO;
using Payments.WEB.Areas.Admin.Models;
using Payments.WEB.Models;
using DebitAccountViewModel = Payments.WEB.Models.DebitAccountViewModel;

namespace Payments.WEB.Util
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterModel, UserDTO>();
            CreateMap<DebitAccountDTO, Payments.WEB.Areas.Admin.Models.DebitAccountViewModel>();
            CreateMap<Payments.WEB.Areas.Admin.Models.DebitAccountViewModel, DebitAccountDTO>();
            CreateMap<CardViewModel, CardDto>();
            CreateMap<PaymentViewModel, PaymentDTO>();
            CreateMap<PaymentDTO, PaymentViewModel>();
            CreateMap<DebitAccountDTO, DebitAccountViewModel>();
            CreateMap<DebitAccountViewModel, DebitAccountDTO>();
            //CreateMap<UserDTO, UserInfoDTO>();
        }
    }
}