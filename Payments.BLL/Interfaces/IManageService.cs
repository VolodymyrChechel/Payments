using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Payments.BLL.DTO;

namespace Payments.BLL.Interfaces
{
    // this service is ised by admin to manage user and their accounts
    public interface IManageService
    {
        IEnumerable<UserInfoDTO> GetProfiles();
        UserInfoDTO GetProfile(string id);

        IEnumerable<DebitAccountDTO> GetDebitAccountsByProfile(string id, bool withoutCard = false);

        void CreateDebitAccount(DebitAccountDTO debitAccount);

        bool IsAccountExist(int? id);

        DebitAccountDTO GetDebitAccount(int? id);
        void UpdateDebitAccount(DebitAccountDTO debitAccount);

        void DeleteAccount(int? id);

        void CreateCard(DepositCardDTO card);
    }
}