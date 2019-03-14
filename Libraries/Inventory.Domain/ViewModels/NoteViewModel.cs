using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ViewModels
{
    public class NoteViewModel
    {
        public Guid? Id { get; set; }
        public Guid? SourceId { get; set; }
        public Guid? DestinationId { get; set; }
        public WarehouseViewModel Source { get; set; }
        public WarehouseViewModel Destination { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string SourceWarehouse { get; set; }
        public Inventory.Core.Enum.ProductType ProductType { get; set; }
        public string DestinationWarehouse { get; set; }
        public string Code { get; set; }
        public double Total { get; set; }
        public string Noted { get; set; }
        public string UserName { get; set; }
        public List<NoteDetailViewModel> Details { get; set; } = new List<NoteDetailViewModel>();
    }
}
