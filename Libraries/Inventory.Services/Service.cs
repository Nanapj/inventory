using Inventory.Data;
using Inventory.Data.Repository;
using Inventory.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _res;

        public IQueryable<T> Queryable()
        {
            return _res.Queryable();
        }

        public Service(IRepository<T> res)
        {
            _res = res as Repository<T>;
        }

        public bool Delete(Guid Id)
        {
            _res.Delete(Id);
            return true;
        }

        public IEnumerable<T> GetAll()
        {
            return _res.GetAll();
        }

        public T Find(Guid Id)
        {
            return _res.Find(Id);
        }

        public T Insert(T obj)
        {
            _res.Insert(obj);
            return obj;
        }

        public void SaveChanges()
        {
            _res.SaveChanges();
        }

        public bool Update(T obj)
        {
            _res.Update(obj);
            return true;
        }
    }
}
