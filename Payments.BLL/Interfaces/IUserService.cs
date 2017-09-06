using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;

namespace Payments.BLL.Interfaces
{
    // used to interract with DAL
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task SetInitialData(UserDTO adminDto, List<UserDTO> usersDto, List<string> roles);
    }
}