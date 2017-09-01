using System;
using Payments.DAL.Entities;

namespace Payments.DAL.Interfaces
{
    // interface is used to create new client profile
    public interface IClientManager : IDisposable
    {
        void Create(ClientProfile item);
    }
}