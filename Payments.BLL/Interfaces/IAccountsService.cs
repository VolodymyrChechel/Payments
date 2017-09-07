using System.Collections.Generic;
using Payments.BLL.DTO;

namespace Payments.BLL.Interfaces
{
    public interface IAccountsService
    {
        IEnumerable<DebitAccountDTO> GetAccountsByUserId(string id);
        DebitAccountDTO GetAccount(string id);
        void BlockAccount(string id);
        void UnblockAccountRequest(string id);
        void CreateDebitAccount(DebitAccountDTO account);
    }
}