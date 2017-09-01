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
            var clients = Database.ClientProfiles.GetAll().Include(profile => profile.ApplicationUser);
            //clients.FirstOrDefault().ApplicationUser.Email
            var testAfterMapper = Mapper.Map<IQueryable<ClientProfile>, IEnumerable<UserInfoDTO>>(clients);

            return testAfterMapper;
        }
    }
}