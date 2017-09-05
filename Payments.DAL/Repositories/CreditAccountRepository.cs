using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Payments.DAL.EF;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.DAL.Repositories
{
    public class CreditAccountRepository : IRepository<CreditAccount>
    {
        private PaymentsContext db;

        public CreditAccountRepository(PaymentsContext context)
        {
            this.db = context;
        }

        public IQueryable<CreditAccount> GetAll()
        {
            return db.CreditAccounts;
        }

        public CreditAccount Get(object id)
        {
            return db.CreditAccounts.Find(id);
        }

        public IQueryable<CreditAccount> Find(Func<CreditAccount, bool> predicate)
        {
            return db.CreditAccounts.Where(predicate).AsQueryable();
        }

        public void Create(CreditAccount item)
        {
            db.CreditAccounts.Add(item);
        }

        public void Update(CreditAccount item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            CreditAccount creditAcc = db.CreditAccounts.Find(id);

            if (creditAcc != null)
                db.CreditAccounts.Remove(creditAcc);
        }
        
    }
}