using System;
using System.Threading.Tasks;
using Payments.DAL.Entities;
using Payments.DAL.Identity;

namespace Payments.DAL.Interfaces
{
    // interface for UnitOfWork pattern
    public interface IUnitOfWork : IDisposable
    {
        // repositories for data store
        IRepository<Card> Cards { get; }
        IRepository<Account> Accounts { get; }
        IRepository<CreditAccount> CreditAccounts { get; }
        IRepository<DebitAccount> DebitAccounts { get; }
        IRepository<Transaction> Transactions { get; }
        IRepository<UnblockAccountRequest> UnblockAccountRequests { get; }
        void Save();

        // repositories for identity
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
    }
}