using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Payments.BLL.DTO;
using Payments.BLL.Interfaces;
using Payments.DAL.Interfaces;

namespace Payments.BLL.Services
{
    public class RequestsService : IRequestsService
    {
        private IUnitOfWork Database { get; set; }

        public RequestsService(IUnitOfWork uow)
        {
            Database = uow;
        }
        
        public IEnumerable<UnblockAccountRequestDTO> GetRequestsList()
        {
            var requests = Database.UnblockAccountRequests.GetAll().ToList();



        }

        public void SendDecision();
    }
}