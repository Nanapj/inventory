using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core
{
    public class DebtNote : Entity
    {
        public string Code { get; set; }
        public Warehouse PayWarehouse { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool IsOwed { get; set; }
        public double Total { get; set; }
        public string Noted { get; set; }
    }
}
