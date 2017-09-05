using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Payments.DAL.EF;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.DAL.Repositories
{
    // realization Repository for Transaction DbSet
    public class TransactionRepository : IRepository<Transaction>
    {
        private PaymentsContext db;

        public TransactionRepository(PaymentsContext context)
        {
            this.db = context;
        }

        public IQueryable<Transaction> GetAll()
        {
            return db.Transactions;
        }

        public Transaction Get(object id)
        {
            return db.Transactions.Find(id);
        }

        public IQueryable<Transaction> Find(Func<Transaction, bool> predicate)
        {
            return db.Transactions.Where(predicate).AsQueryable();
        }

        public void Create(Transaction item)
        {
            db.Transactions.Add(item);
        }

        public void Update(Transaction item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            Transaction transaction = db.Transactions.Find(id);

            if (transaction != null)
                db.Transactions.Remove(transaction);
        }
        
    }
}