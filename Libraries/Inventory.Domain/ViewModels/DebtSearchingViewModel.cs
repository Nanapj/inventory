using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ViewModels
{
    public class DebtSearchingViewModel
    {
        public Guid? WarehouseId { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public bool isOwed { get; set; }
    }
}
