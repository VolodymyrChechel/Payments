using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
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
            this.db = context;
        }

        public IQueryable<Payment> GetAll()
        {
            return db.Payments;
        }

        public Payment Get(object id)
        {
            return db.Payments.Find(id);
        }

        public IQueryable<Payment> Find(Func<Payment, bool> predicate)
        {
            return db.Payments.Where(predicate).AsQueryable();
        }

        public void Create(Payment item)
        {
            db.Payments.Add(item);
        }

        public void Update(Payment item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            Payment payment = db.Payments.Find(id);

            if (payment != null)
                db.Payments.Remove(payment);
        }
        
    }
}