using Inventory.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core
{
    public class Note : Entity
    {
        public Note()
        {
            Details = new List<NoteDetail>();
        }
        public string Code { get; set; }
        public Warehouse Source { get; set; }
        public Warehouse Destination { get; set; }
        public double Total { get; set; }
        public string Noted { get; set; }
        public List<NoteDetail> Details { get; set; }
        public ProductType ProductType { get; set; }
        public void TotalCal()
        {
            Total = Details.Sum(x => x.Quantity * x.Price);
        }
    }
}
