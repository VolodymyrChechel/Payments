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
using Payments.Common.NLog;
using Payments.DAL.Entities;
using Payments.DAL.Identity;
using Payments.DAL.Interfaces;

namespace Payments.BLL.Services
{
    // service for creating new users in identity
    public class UserService : IUserService
    {
        private IUnitOfWork db { get; set; }

        public UserService(IUnitOfWork uow)
        {
            NLog.LogInfo(this.GetType(), "Constructor UserService execution");

            db = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            NLog.LogInfo(this.GetType(), "Method Create execution");

            ApplicationUser user = await db.UserManager.FindByEmailAsync(userDto.Email);

            if (user == null)
            {
                user = new ApplicationUser {Email = userDto.Email, UserName = userDto.Email};

                var result = await db.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Any())
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
            NLog.LogInfo(this.GetType(), "Method Authenticate execution");

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
        public async Task SetInitialData(UserDTO adminDto, UserDTO employeeDto, List<UserDTO> usersDto, List<string> roles)
        {
            NLog.LogInfo(this.GetType(), "Method SetInitialData execution");
            
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

            // adding employee
            await Create(employeeDto);
            
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