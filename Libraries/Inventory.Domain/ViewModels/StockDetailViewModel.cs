using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ViewModels
{
    public class StockDetailViewModel
    {
        public string ProductName { get; set; }
        public long Quantity { get; set; }
        public long OpeningStock { get; set; }
        public long ClosingStock { get; set; }
        public long ExportQuantity { get; set; }
        public long ImportQuantity { get; set; }

    }
}
