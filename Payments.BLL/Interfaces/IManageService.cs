using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Payments.BLL.DTO;

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

        
        // cards methods
        void CreateCard(CardDto card);
        IEnumerable<CardDto> GetCardsByProfile(string id);
        void DeleteCard(string number);
    }
}