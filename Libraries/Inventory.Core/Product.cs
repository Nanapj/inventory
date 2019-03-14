using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core
{
    public class Product : Entity
    {
        public string Code { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Note { get; set; }
        public string Unit { get; set; }
        public IList<NoteDetail> NoteDetails { get; set; } = new List<NoteDetail>();
        public IList<Unit> Units { get; set; } = new List<Unit>();
    }
}
