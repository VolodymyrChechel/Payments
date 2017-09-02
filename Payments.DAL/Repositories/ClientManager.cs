using System;
using System.Data.Entity;
using System.Linq;
using Payments.DAL.EF;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.DAL.Repositories
{
    public class ClientManager : IClientManager
    {
        private PaymentsContext db;

        public ClientManager(PaymentsContext context)
        {
            db = context;
        }

        public void Create(ClientProfile profile)
        {
            db.ClientProfiles.Add(profile);
            db.SaveChanges();
        }

        public IQueryable<ClientProfile> GetAll()
        {
            return db.ClientProfiles;
        }

        public ClientProfile Get(string id)
        {
            return db.ClientProfiles.Find(id);
        }

        public IQueryable<ClientProfile> Find(Func<ClientProfile, bool> predicate)
        {
            return db.ClientProfiles.Where(predicate).AsQueryable();
        }

        public void Update(ClientProfile item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(string id)
        {
            ClientProfile profile = db.ClientProfiles.Find(id);
            if (profile != null)
                db.ClientProfiles.Remove(profile);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}