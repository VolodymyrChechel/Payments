using AutoMapper;
using Payments.BLL.DTO;
using ViewModels = Payments.WEB.Models;
using AdminViewModels = Payments.WEB.Areas.Admin.Models;

namespace Payments.WEB.Util
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ViewModels.RegisterModel, UserDTO>();
            CreateMap<DebitAccountDTO, AdminViewModels.DebitAccountViewModel>();
            CreateMap<AdminViewModels.DebitAccountViewModel, DebitAccountDTO>();
            CreateMap<AdminViewModels.CardViewModel, CardDto>();
            CreateMap<AdminViewModels.PaymentViewModel, PaymentDTO>();
            CreateMap<PaymentDTO, AdminViewModels.PaymentViewModel>();
            CreateMap<DebitAccountDTO, ViewModels.DebitAccountViewModel>();
            CreateMap<ViewModels.DebitAccountViewModel, DebitAccountDTO>();
            CreateMap<ViewModels.PaymentViewModel, PaymentDTO>();
            CreateMap<ViewModels.CardViewModel, CardDto>();
            CreateMap<CardDto, ViewModels.CardViewModel>();
            //CreateMap<UserDTO, UserInfoDTO>();
        }
    }
}