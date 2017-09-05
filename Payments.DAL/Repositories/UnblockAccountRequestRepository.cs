using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Payments.DAL.EF;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.DAL.Repositories
{
    public class UnblockAccountRequestRepository : IRepository<UnblockAccountRequest>
    {
        private PaymentsContext db;

        public UnblockAccountRequestRepository(PaymentsContext context)
        {
            this.db = context;
        }

        public IQueryable<UnblockAccountRequest> GetAll()
        {
            return db.UnblockAccountRequests;
        }

        public UnblockAccountRequest Get(object id)
        {
            return db.UnblockAccountRequests.Find(id);
        }

        public IQueryable<UnblockAccountRequest> Find(Func<UnblockAccountRequest, bool> predicate)
        {
            return db.UnblockAccountRequests.Where(predicate).AsQueryable();
        }

        public void Create(UnblockAccountRequest item)
        {
            db.UnblockAccountRequests.Add(item);
        }

        public void Update(UnblockAccountRequest item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            UnblockAccountRequest request = db.UnblockAccountRequests.Find(id);

            if (request != null)
                db.UnblockAccountRequests.Remove(request);
        }
        
    }
}