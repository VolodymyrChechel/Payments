using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Payments.BLL.DTO;
using Payments.BLL.Interfaces;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.BLL.Services
{
    public class ManageService : IManageService
    {
        private IUnitOfWork Database { get; set; }

        public ManageService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<UserInfoDTO> GetProfiles()
        {
            var clients = Database.ClientManager.GetAll().Include(profile => profile.ApplicationUser);

            return Mapper.Map<IQueryable<ClientProfile>, IEnumerable<UserInfoDTO>>(clients); ;
        }

        public UserInfoDTO GetProfile(string id)
        {
            var client = Database.ClientManager.Get(id);

            return Mapper.Map<ClientProfile, UserInfoDTO>(client);
        }

        public 
    }
}