using System.Collections.Generic;
using System.Linq;
using Payments.BLL.DTO;

namespace Payments.BLL.Interfaces
{
    public interface IManageService
    {
        IEnumerable<UserInfoDTO> GetProfiles();
        UserInfoDTO GetProfile(string id);


        // account managing
        IEnumerable<DebitAccountDTO> GetDebitAccountsByProfile(string id);

        void CreateDebitAccount(DebitAccountDTO debitAccount);

        
    }
}