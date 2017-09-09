using System.Collections.Generic;
using Payments.BLL.DTO;

namespace Payments.BLL.Interfaces
{
    public interface IRequestsService
    {
        IEnumerable<RequestDto> GetRequestsList();
        void SendDecision();
    }
}