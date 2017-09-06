using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.DAL.Entities;
using Payments.DAL.Identity;
using Payments.DAL.Interfaces;

namespace Payments.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork db { get; set; }

        public UserService(IUnitOfWork uow)
        {
            db = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await db.UserManager.FindByEmailAsync(userDto.Email);

            if (user == null)
            {
                user = new ApplicationUser {Email = userDto.Email, UserName = userDto.Email};

                var result = await db.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                // adding the user to role
                await db.UserManager.AddToRoleAsync(user.Id, userDto.Role);

                // creating a client profile
                ClientProfile clientProfile = new ClientProfile
                {
                    Id = user.Id,
                    FirstName = userDto.FirstName,
                    SecondName = userDto.SecondName,
                    Patronymic = userDto.Patronymic,
                    Birthday = userDto.Birthday,
                    PhoneNumber = userDto.PhoneNumber,
                    VAT = userDto.VAT
                };

                db.ClientManager.Create(clientProfile);
                await db.SaveAsync();

                return new OperationDetails(true, "Registration successful", "");
            }
            else
            {
                return new OperationDetails(false, "User with such login already redister", "Email");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;

            // search user
            ApplicationUser user = await db.UserManager.FindAsync(userDto.Email, userDto.Password);

            // authorisate and return claim
            if (user != null)
                claim = await db.UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);

            return claim;
        }

        // initialization data
        public async Task SetInitialData(UserDTO adminDto, List<UserDTO> usersDto, List<string> roles)
        {
            // adding initial roles
            foreach (var roleName in roles)
            {
                var role = await db.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await db.RoleManager.CreateAsync(role);
                }
            }

            // adding admin
            await Create(adminDto);
           
            foreach (var userDto in usersDto)
            {
                await Create(userDto);
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}