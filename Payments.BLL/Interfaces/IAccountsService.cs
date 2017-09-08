using System.Collections.Generic;
using Payments.BLL.DTO;

namespace Payments.BLL.Interfaces
{
    public interface IAccountsService
    {
        IEnumerable<DebitAccountDTO> GetAccountsByUserId(string id, string ordering = null);
        DebitAccountDTO GetAccount(string id);
        void BlockAccount(string id);
        void UnblockAccountRequest(string id);
        void CreateDebitAccount(DebitAccountDTO account);
        void EditAccountName(DebitAccountDTO account);

        void Replenish(PaymentDTO payment);
        void Withdraw(PaymentDTO payment);
        void Payment(PaymentDTO payment);
        IEnumerable<PaymentDTO> GetPaymentsByProfile(string id, string sortType);
    }
}