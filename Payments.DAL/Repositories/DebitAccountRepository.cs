using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Payments.DAL.EF;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.DAL.Repositories
{
    // implementation Repository for DebitAccount DbSet
    public class DebitAccountRepository : IRepository<DebitAccount>
    {
        private PaymentsContext db;

        public DebitAccountRepository(PaymentsContext context)
        {
            this.db = context;
        }

        public IQueryable<DebitAccount> GetAll()
        {
            return db.DebitAccounts;
        }

        public DebitAccount Get(object id)
        {
            return db.DebitAccounts.Find(id);
        }

        public IQueryable<DebitAccount> Find(Func<DebitAccount, bool> predicate)
        {
            return db.DebitAccounts.Where(predicate).AsQueryable();
        }

        public void Create(DebitAccount item)
        {
            db.DebitAccounts.Add(item);
        }

        public void Update(DebitAccount item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            DebitAccount debitAcc = db.DebitAccounts.Find(id);

            if (debitAcc != null)
                db.DebitAccounts.Remove(debitAcc);
        }
        
    }
}