using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ViewModels
{
    public class DebtStockViewModel
    {
        public string PayMethod { get; set; }
        public List<DebtStockDetailViewModel> Details { get; set; } = new List<DebtStockDetailViewModel>();
    }
}
