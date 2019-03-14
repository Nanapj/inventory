using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ViewModels
{
    public class NoteDetailViewModel
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? NoteId { get; set; }
        public string ProductName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public string Noted { get; set; }
    }
}
