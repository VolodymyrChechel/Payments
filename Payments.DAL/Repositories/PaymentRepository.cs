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
    // realization Repository for Payment DbSet
    public class PaymentRepository : IRepository<Payment>
    {
        private PaymentsContext db;

        public PaymentRepository(PaymentsContext context)
        {
            NLog.LogInfo(this.GetType(), "Constructor PaymentRepository execution");

            this.db = context;
        }

        public IQueryable<Payment> GetAll()
        {
            NLog.LogInfo(this.GetType(), "Method GetAll execution");

            return db.Payments;
        }

        public Payment Get(object id)
        {
            NLog.LogInfo(this.GetType(), "Method Get execution");

            return db.Payments.Find(Convert.ToInt32(id));
        }

        public IQueryable<Payment> Find(Func<Payment, bool> predicate)
        {
            NLog.LogInfo(this.GetType(), "Method Find execution");

            return db.Payments.Where(predicate).AsQueryable();
        }

        public void Create(Payment item)
        {
            NLog.LogInfo(this.GetType(), "Method Create execution");

            db.Payments.Add(item);
        }

        public void Update(Payment item)
        {
            NLog.LogInfo(this.GetType(), "Method Update execution");

            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            NLog.LogInfo(this.GetType(), "Method Delete execution");

            Payment payment = db.Payments.Find(id);

            if (payment != null)
                db.Payments.Remove(payment);
        }
        
    }
}