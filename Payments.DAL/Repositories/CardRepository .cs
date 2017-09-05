using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Payments.DAL.EF;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.DAL.Repositories
{
    // realization Repository for Card DbSet
    public class CardRepository : IRepository<Card>
    {
        private PaymentsContext db;

        public CardRepository(PaymentsContext context)
        {
            this.db = context;
        }

        public IQueryable<Card> GetAll()
        {
            return db.Cards;
        }

        public Card Get(object id)
        {
            return db.Cards.Find(id);
        }

        public IQueryable<Card> Find(Func<Card, bool> predicate)
        {
            return db.Cards.Where(predicate).AsQueryable();
        }

        public void Create(Card item)
        {
            db.Cards.Add(item);
        }

        public void Update(Card item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            Card card = db.Cards.Find(id);
            if (card != null)
                db.Cards.Remove(card);
        }
        
    }
}