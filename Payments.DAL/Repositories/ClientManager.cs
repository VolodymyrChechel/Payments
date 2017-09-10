using System;
using System.Data.Entity;
using System.Linq;
using Payments.Common.NLog;
using Payments.DAL.EF;
using Payments.DAL.Entities;
using Payments.DAL.Interfaces;

namespace Payments.DAL.Repositories
{
    // implementation Repository for ClientProfiles DbSet
    public class ClientManager : IClientManager
    {
        private PaymentsContext db;

        public ClientManager(PaymentsContext context)
        {
            NLog.LogInfo(this.GetType(), "Constructor ClientManager execution");

            db = context;
        }

        public void Create(ClientProfile profile)
        {
            NLog.LogInfo(this.GetType(), "Method Create execution");
            db.ClientProfiles.Add(profile);
            db.SaveChanges();
        }

        public IQueryable<ClientProfile> GetAll()
        {
            NLog.LogInfo(this.GetType(), "Method GetAll execution");

            return db.ClientProfiles;
        }

        public ClientProfile Get(string id)
        {
            NLog.LogInfo(this.GetType(), "Method Get execution");

            return db.ClientProfiles.Find(id);
        }

        public IQueryable<ClientProfile> Find(Func<ClientProfile, bool> predicate)
        {
            NLog.LogInfo(this.GetType(), "Method Find execution");

            return db.ClientProfiles.Where(predicate).AsQueryable();
        }

        public void Update(ClientProfile item)
        {
            NLog.LogInfo(this.GetType(), "Method Update execution");

            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(string id)
        {
            NLog.LogInfo(this.GetType(), "Method Delete execution");

            ClientProfile profile = db.ClientProfiles.Find(id);
            if (profile != null)
                db.ClientProfiles.Remove(profile);
        }

        public void Dispose()
        {
            NLog.LogInfo(this.GetType(), "Method Dispose execution");

            db.Dispose();
        }
    }
}