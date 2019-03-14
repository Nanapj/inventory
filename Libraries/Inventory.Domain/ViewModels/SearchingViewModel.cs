using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Core;
namespace Inventory.Domain.ViewModels
{
    public class SearchingViewModel
    {
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public Guid? DestinationId { get; set; }
        public Guid? SourceId { get; set; }
        public Guid? Id { get; set; }
        public string Code { get; set; }
    }
}
