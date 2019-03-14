using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ViewModels
{
    public class SearchingStockViewModel
    {
        public Guid? WarehouseId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}
