using Inventory.Data.Repository;
using Inventory.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private InventoryContext context;
        private DbSet<T> dbSet;

        public Repository(IInventoryContext ct)
        {
            context = ct as InventoryContext;
            dbSet = context.Set<T>();
        }

        public bool Delete(object Id)
        {
            var entity = Find(Id);
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
            return true;
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public T Find(object Id)
        {
            return dbSet.Find(Id);
        }

        public T Insert(T obj)
        {
            dbSet.Add(obj);
            return obj;
        }

        public IQueryable<T> Queryable()
        {
            return dbSet.AsQueryable();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public bool Update(T obj)
        {
            context.Entry(obj).State = EntityState.Modified;
            return true;
        }

    }
}
