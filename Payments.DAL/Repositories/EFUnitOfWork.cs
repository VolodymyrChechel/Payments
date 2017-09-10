using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Payments.Common.NLog;
using Payments.DAL.EF;
using Payments.DAL.Entities;
using Payments.DAL.Identity;
using Payments.DAL.Interfaces;

namespace Payments.DAL.Repositories
{
    // realization unit of work repository
    public class EFUnitOfWork : IUnitOfWork
    {
        private PaymentsContext db;
        
        private CardRepository cardRepository;
        private AccountRepository accountRepository;
        private CreditAccountRepository creditAccountRepository;
        private DebitAccountRepository debitAccountRepository;
        private PaymentRepository paymentRepository;
        private UnblockAccountRequestRepository unblockAccountRequestRepository;

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private IClientManager clientManager;

        public EFUnitOfWork(string connectionString)
        {
            NLog.LogInfo(this.GetType(), "Constructor EFUnitOfWork execution");

            db = new PaymentsContext(connectionString);
        }

        public IRepository<Card> Cards
        {
            get
            {
                if(cardRepository == null)
                    cardRepository = new CardRepository(db);

                return cardRepository;
            }
        }

        public IRepository<CreditAccount> CreditAccounts
        {
            get
            {
                if (creditAccountRepository == null)
                    creditAccountRepository = new CreditAccountRepository(db);

                return creditAccountRepository;
            }
        }

         public IRepository<Account> Accounts
        {
            get
            {
                if (accountRepository == null)
                    accountRepository = new AccountRepository(db);

                return accountRepository;
            }
        }

        public IRepository<DebitAccount> DebitAccounts
        {
            get
            {
                if(debitAccountRepository == null)
                    debitAccountRepository = new DebitAccountRepository(db);

                return debitAccountRepository;
            }
        }

        public IRepository<Payment> Payments
        {
            get
            {
                if(paymentRepository == null)
                    paymentRepository = new PaymentRepository(db);

                return paymentRepository;
            }
        }

        public IRepository<UnblockAccountRequest> UnblockAccountRequests
        {
            get
            {
                if(unblockAccountRequestRepository == null)
                    unblockAccountRequestRepository = new UnblockAccountRequestRepository(db);

                return unblockAccountRequestRepository;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                if(userManager == null)
                    userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

                return userManager;
            }
        }

        public IClientManager ClientManager
        {
            get
            {
                if(clientManager == null)
                    clientManager = new ClientManager(db);

                return clientManager;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                if(roleManager == null)
                    roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));

                return roleManager;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}   