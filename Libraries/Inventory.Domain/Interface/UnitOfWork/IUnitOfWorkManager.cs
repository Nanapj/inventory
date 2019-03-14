using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Interface.UnitOfWork
{
    public partial interface IUnitOfWorkManager : IDisposable
    {
        IUnitOfWork NewUnitOfWork();
    }
}
