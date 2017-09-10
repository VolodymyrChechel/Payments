using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Payments.Common.NLog;
using Payments.DAL.EF;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.DAL.Repositories
{
    // implementation Repository for Account DbSet
    public class AccountRepository : IRepository<Account>
    {
        private PaymentsContext db;

        public AccountRepository(PaymentsContext context)
        {
            NLog.LogInfo(this.GetType(), "Constructor AccountRepository execution");

            this.db = context;
        }

        public IQueryable<Account> GetAll()
        {
            NLog.LogInfo(this.GetType(), "Method GetAll execution");

            return db.Accounts;
        }

        public Account Get(object id)
        {
            NLog.LogInfo(this.GetType(), "Method Get execution");

            return db.Accounts.Find(Convert.ToInt32(id));
        }

        public IQueryable<Account> Find(Func<Account, bool> predicate)
        {
            NLog.LogInfo(this.GetType(), "Method Find execution");

            return db.Accounts.Where(predicate).AsQueryable();
        }

        public void Create(Account item)
        {
            NLog.LogInfo(this.GetType(), "Method Create execution");

            db.Accounts.Add(item);
        }

        public void Update(Account item)
        {
            NLog.LogInfo(this.GetType(), "Method Update execution");

            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            NLog.LogInfo(this.GetType(), "Method Delete execution");

            var account = db.Accounts.Find(id);

            // when account has existing relation data to deny deleting
            if (account != null)
                if (account.Cards.Any() || account.Payments.Any() ||
                    account.UnblockAccountRequests.Any())
                    throw new Exception("Account has related data");
            
            db.Accounts.Remove(account);
        }
        
    }
}