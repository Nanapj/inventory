using Inventory.Domain.Interface;
using Inventory.Domain.Interface.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InventoryContext _context;
        private readonly ObjectContext _objectContext;

        public UnitOfWork(IInventoryContext context)
        {
            _context = context as InventoryContext;
            _objectContext = ((IObjectContextAdapter)_context).ObjectContext;

            if (_objectContext.Connection.State != ConnectionState.Open)
            {
                _objectContext.Connection.Open();
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_objectContext.Connection.State == ConnectionState.Open)
            {
                _objectContext.Connection.Close();
            }
        }
    }
}
