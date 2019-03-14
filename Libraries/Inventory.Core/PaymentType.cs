using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core
{
    public class PaymentType : Entity
    {
        public string Name { get; set; }
        public IList<DebtNote> DebtNote { get; set; } = new List<DebtNote>();
    }
}
