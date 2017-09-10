using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.Common.Enums;
using Payments.Common.NLog;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.BLL.Services
{
    // implementation of IRequestsService
    // allows to block or sent unblock request by admin
    public class RequestsService : IRequestsService
    {
        private IUnitOfWork Database { get; set; }

        public RequestsService(IUnitOfWork uow)
        {
            NLog.LogInfo(this.GetType(), "Constructor RequestsService execution");

            Database = uow;
        }
        
        public IEnumerable<UnblockAccountRequestDTO> GetRequestsList()
        {
            NLog.LogInfo(this.GetType(), "Method GetRequestsList execution");

            var requestsList = Database.UnblockAccountRequests.GetAll().
                Where(request => request.Status == UnblockRequestStatus.Prepared).
                ToList();

            var requestsDtoList = Mapper.Map<IEnumerable<UnblockAccountRequest>,
                IEnumerable<UnblockAccountRequestDTO>>(requestsList);

            return requestsDtoList;
        }

        public void AcceptRequest(string id)
        {
            NLog.LogInfo(this.GetType(), "Method AcceptRequest execution");

            if(id == null)
                throw new ValidationException("Id was not passed", "");

            var request = Database.UnblockAccountRequests.Get(id);

            if(request == null)
                throw new ValidationException("Request was not found", "");

            request.Status = UnblockRequestStatus.Proceeded;

            var account = Database.Accounts.Get(request.AccountAccountNumber);
            account.IsBlocked = false;

            Database.Accounts.Update(account);
            Database.UnblockAccountRequests.Update(request);
            Database.Save();
        }

        public void RejectRequest(string id)
        {
            NLog.LogInfo(this.GetType(), "Method RejectRequest execution");

            if (id == null)
                throw new ValidationException("Id was not passed", "");

            var request = Database.UnblockAccountRequests.Get(id);

            if (request == null)
                throw new ValidationException("Request was not found", "");

            request.Status = UnblockRequestStatus.Cancelled;

            Database.UnblockAccountRequests.Update(request);
            Database.Save();
        }
    }
}