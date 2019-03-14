using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ViewModels
{
    public class StockGroupViewModel
    {
        public string Warehouse { get; set; }
        public List<StockDetailViewModel> StockList { get; set; } = new List<StockDetailViewModel>();
    }
}
