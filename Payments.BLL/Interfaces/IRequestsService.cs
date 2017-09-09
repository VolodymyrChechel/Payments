using System.Collections.Generic;
using Payments.BLL.DTO;

namespace Payments.BLL.Interfaces
{
    // sevice for admin, used to work with unblock requests
    public interface IRequestsService
    {
        IEnumerable<UnblockAccountRequestDTO> GetRequestsList();

        void AcceptRequest(string id);
        void RejectRequest(string id);
    }
}