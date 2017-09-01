using System.Data.Entity;
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

        public void Dispose()
        {
            db.Dispose();
        }
    }
}