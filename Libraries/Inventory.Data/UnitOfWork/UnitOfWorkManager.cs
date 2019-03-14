using Inventory.Domain.Interface;
using Inventory.Domain.Interface.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.UnitOfWork
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly InventoryContext _context;

        public UnitOfWorkManager(IInventoryContext context)
        {
            _context = context as InventoryContext;
        }

        public IUnitOfWork NewUnitOfWork()
        {
            return new UnitOfWork(_context);
        }

        public void Dispose()
        {
        }
    }
}
