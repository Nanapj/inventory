using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ViewModels
{
    public class DebtStockGroupViewModel
    {
        public string Warehouse { get; set;}
        public double Sum { get; set; }
        public List<DebtStockViewModel> StockList { get; set; } = new List<DebtStockViewModel>();
    }
}
