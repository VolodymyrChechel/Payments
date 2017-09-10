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
    // implementation Repository for Card DbSet
    public class CardRepository : IRepository<Card>
    {
        private PaymentsContext db;

        public CardRepository(PaymentsContext context)
        {
            NLog.LogInfo(this.GetType(), "Constructor CardRepository execution");

            this.db = context;
        }

        public IQueryable<Card> GetAll()
        {
            NLog.LogInfo(this.GetType(), "Method GetAll execution");

            return db.Cards;
        }

        public Card Get(object id)
        {
            NLog.LogInfo(this.GetType(), "Method Get execution");

            return db.Cards.Find(id);
        }

        public IQueryable<Card> Find(Func<Card, bool> predicate)
        {
            NLog.LogInfo(this.GetType(), "Method Find execution");

            return db.Cards.Where(predicate).AsQueryable();
        }

        public void Create(Card item)
        {
            NLog.LogInfo(this.GetType(), "Method Create execution");

            db.Cards.Add(item);
        }

        public void Update(Card item)
        {
            NLog.LogInfo(this.GetType(), "Method Update execution");

            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            NLog.LogInfo(this.GetType(), "Method Delete execution");

            Card card = db.Cards.Find(id);
            if (card != null)
                db.Cards.Remove(card);
        }
        
    }
}