using System;
using System.Collections;
using System.Linq;

namespace Payments.DAL.Interfaces
{
    // interface for Repository pattern
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T Get(int id);
        IQueryable<T> Find(Func<T, bool> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}