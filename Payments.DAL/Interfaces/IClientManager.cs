using System;
using System.Data.Entity;
using System.Linq;
using Payments.DAL.EF;
using Payments.DAL.Entities;

namespace Payments.DAL.Interfaces
{
    // interface is used to create new client profile
    public interface IClientManager : IDisposable
    {
        void Create(ClientProfile profile);

        IQueryable<ClientProfile> GetAll();

        ClientProfile Get(string id);

        IQueryable<ClientProfile> Find(Func<ClientProfile, bool> predicate);

        void Update(ClientProfile item);

        void Delete(string id);
    }
}