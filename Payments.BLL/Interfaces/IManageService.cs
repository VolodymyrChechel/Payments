using System.Collections.Generic;
using System.Linq;
using Payments.BLL.DTO;

namespace Payments.BLL.Interfaces
{
    public interface IManageService
    {
        IEnumerable<UserInfoDTO> GetProfiles();
        UserInfoDTO GetProfile(string id);

        //void CreateNewAccount(AccountDTO accountDto)
        //{

        //}
    }
}