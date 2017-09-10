using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Payments.Common.NLog;
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
            NLog.LogInfo(this.GetType(), "Constructor DebitAccountRepository execution");

            this.db = context;
        }

        public IQueryable<DebitAccount> GetAll()
        {
            NLog.LogInfo(this.GetType(), "Method GetAll execution");

            return db.DebitAccounts;
        }

        public DebitAccount Get(object id)
        {
            NLog.LogInfo(this.GetType(), "Method Get execution");

            return db.DebitAccounts.Find(id);
        }

        public IQueryable<DebitAccount> Find(Func<DebitAccount, bool> predicate)
        {
            NLog.LogInfo(this.GetType(), "Method Find execution");

            return db.DebitAccounts.Where(predicate).AsQueryable();
        }

        public void Create(DebitAccount item)
        {
            NLog.LogInfo(this.GetType(), "Method Create execution");

            db.DebitAccounts.Add(item);
        }

        public void Update(DebitAccount item)
        {
            NLog.LogInfo(this.GetType(), "Method Update execution");

            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            NLog.LogInfo(this.GetType(), "Method Delete execution");

            DebitAccount debitAcc = db.DebitAccounts.Find(id);

            if (debitAcc != null)
                db.DebitAccounts.Remove(debitAcc);
        }
        
    }
}