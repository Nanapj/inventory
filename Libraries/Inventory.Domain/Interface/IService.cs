using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Interface
{
    public interface IService<T> where T : class
    {
        IEnumerable<T> GetAll();
        IQueryable<T> Queryable();
        T Find(Guid Id);
        T Insert(T obj);
        bool Delete(Guid Id);
        bool Update(T obj);
        void SaveChanges();
    }
}
