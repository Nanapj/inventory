using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IQueryable<T> Queryable();
        T Find(object Id);
        T Insert(T obj);
        bool Delete(object Id);
        bool Update(T obj);
        void SaveChanges();
    }
}
