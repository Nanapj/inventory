using Inventory.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core
{
    public class Entity
    {
        public Entity()
        {
            Id = GuidComb.GenerateComb();
        }
        public Guid Id { get; set; }
        public bool Delete { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public ApplicationUser User { get; set; }
        public long? Index { get; set; }
        public long? MonthIndex { get; set; }
    }
}
