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
    // implementation Repository for UnblockAccountRequests DbSet
    public class UnblockAccountRequestRepository : IRepository<UnblockAccountRequest>
    {
        private PaymentsContext db;

        public UnblockAccountRequestRepository(PaymentsContext context)
        {
            NLog.LogInfo(this.GetType(), "Constructor UnblockAccountRequestRepository execution");

            this.db = context;
        }

        public IQueryable<UnblockAccountRequest> GetAll()
        {
            NLog.LogInfo(this.GetType(), "Method GetAll execution");

            return db.UnblockAccountRequests;
        }

        public UnblockAccountRequest Get(object id)
        {
            NLog.LogInfo(this.GetType(), "Method Get execution");

            return db.UnblockAccountRequests.Find(Convert.ToInt32(id));
        }

        public IQueryable<UnblockAccountRequest> Find(Func<UnblockAccountRequest, bool> predicate)
        {
            NLog.LogInfo(this.GetType(), "Method Find execution");

            return db.UnblockAccountRequests.Where(predicate).AsQueryable();
        }

        public void Create(UnblockAccountRequest item)
        {
            NLog.LogInfo(this.GetType(), "Method Create execution");

            db.UnblockAccountRequests.Add(item);
        }

        public void Update(UnblockAccountRequest item)
        {
            NLog.LogInfo(this.GetType(), "Method Update execution");

            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            NLog.LogInfo(this.GetType(), "Method Update execution");

            UnblockAccountRequest request = db.UnblockAccountRequests.Find(id);

            if (request != null)
                db.UnblockAccountRequests.Remove(request);
        }
        
    }
}