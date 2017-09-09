using System.Collections.Generic;
using Payments.BLL.DTO;

namespace Payments.BLL.Interfaces
{
    public interface IRequestsService
    {
        IEnumerable<UnblockAccountRequestDTO> GetRequestsList();

        void AcceptRequest(string id);
        void RejectRequest(string id);
    }
}