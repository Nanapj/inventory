using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core
{
    public class Unit : Entity
    {
        public string UnitName { get; set; }
        public Product Product { get; set; }
    }
}
