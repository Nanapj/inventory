using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ViewModels
{
    public class DebtNoteViewModel
    {
        public Guid? Id { get; set; }
        public Guid? PaymentTypeId { get; set; }
        public Guid? WarehouseId { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string CreatedUser { get; set; }
        public double Total { get; set; }
        public string PayMethod { get; set; }
        public string PayWarehouse { get; set; }
        public bool isOwed { get; set; }
        public string Unit { get; set; }
        public string Noted { get; set; }
    }
}
