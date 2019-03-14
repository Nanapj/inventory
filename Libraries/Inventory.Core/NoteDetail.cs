using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core
{
    public class NoteDetail : Entity
    {
        public Note Note { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public string Noted { get; set; }
    }
}
