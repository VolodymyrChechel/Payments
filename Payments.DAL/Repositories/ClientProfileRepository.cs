using Payments.DAL.EF;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Payments.DAL.Repositories
{
    public class ClientProfileRepository : IRepository<ClientProfile>
    {
        private PaymentsContext db;

        public ClientProfileRepository(PaymentsContext context)
        {
            this.db = context;
        }

        public IQueryable<ClientProfile> GetAll()
        {
            return db.ClientProfiles;
        }

        public ClientProfile Get(int id)
        {
            return db.ClientProfiles.Find(id);
        }

        public IQueryable<ClientProfile> Find(Func<ClientProfile, bool> predicate)
        {
            return db.ClientProfiles.Where(predicate).AsQueryable();
        }

        public void Create(ClientProfile item)
        {
            db.ClientProfiles.Add(item);
        }

        public void Update(ClientProfile item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ClientProfile profile = db.ClientProfiles.Find(id);
            if (profile != null)
                db.ClientProfiles.Remove(profile);
        }
    }
}