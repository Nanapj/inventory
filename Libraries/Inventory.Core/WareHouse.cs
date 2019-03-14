using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core
{
    public class Warehouse : Entity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string WarehouseCode { get; set; }
        public IList<Note> SourceNote { get; set; } = new List<Note>();
        public IList<Note> DestinationNote { get; set; } = new List<Note>();
        public IList<DebtNote> DebtNote { get; set; } = new List<DebtNote>();
    }
}
