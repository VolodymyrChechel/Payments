using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Payments.BLL.DTO;
using Payments.DAL.Entities;

namespace Payments.BLL.Interfaces
{
    // this service is used by admin to manage user and their accounts
    public interface IManageService
    {
        // users methods
        IEnumerable<UserInfoDTO> GetProfiles();
        UserInfoDTO GetProfile(string id);

        // accounts methods
        IEnumerable<DebitAccountDTO> GetDebitAccountsByProfile(string id, bool withoutCard = false);
        void CreateDebitAccount(DebitAccountDTO debitAccount);
        bool IsAccountExist(int? id);
        DebitAccountDTO GetDebitAccount(int? id);
        void UpdateDebitAccount(DebitAccountDTO debitAccount);
        void DeleteAccount(int? id);
        string GetAccountProfileId(int? id);
        
        // cards methods
        void CreateCard(CardDto card);
        bool IsCardExist(string number);
        IEnumerable<CardDto> GetCardsByProfile(string id);
        void DeleteCard(string number);

        // operations refer to payment
        void Replenish(PaymentDTO payment);
        void Withdraw(PaymentDTO payment);
        void Payment(PaymentDTO payment);

        IEnumerable<PaymentDTO> GetPaymentsByProfile(string id, string sortType);


    }
}