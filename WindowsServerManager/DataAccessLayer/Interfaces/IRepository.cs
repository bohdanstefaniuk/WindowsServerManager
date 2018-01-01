using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Guid id);
        T GetByCode(string Code);
        void Create(T item);
        void Update(T item);
        void Delete(Guid id);
    }
}