using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Payments.DAL.EF;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.DAL.Repositories
{
    public class AccountRepository : IRepository<Account>
    {
        private PaymentsContext db;

        public AccountRepository(PaymentsContext context)
        {
            this.db = context;
        }

        public IQueryable<Account> GetAll()
        {
            return db.Accounts;
        }

        public Account Get(object id)
        {
            return db.Accounts.Find(Convert.ToInt32(id));
        }

        public IQueryable<Account> Find(Func<Account, bool> predicate)
        {
            return db.Accounts.Where(predicate).AsQueryable();
        }

        public void Create(Account item)
        {
            db.Accounts.Add(item);
        }

        public void Update(Account item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            var account = db.Accounts.Find(id);

            if (account != null)
                if (account.Cards.Any() || account.Payments.Any() ||
                    account.UnblockAccountRequests.Any())
                    throw new Exception("Account has related data");
            
            db.Accounts.Remove(account);
        }
        
    }
}