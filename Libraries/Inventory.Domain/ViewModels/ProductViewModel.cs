using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ViewModels
{
    public class ProductViewModel
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Unit { get; set; }
    }
}
