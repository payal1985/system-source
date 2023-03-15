using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class InventoryFloors
    {
        public InventoryFloors()
        {
            InventoryItem = new HashSet<InventoryItem>();
        }

        public int InventoryFloorId { get; set; }
        public int? ClientId { get; set; }
        public string InventoryFloorCode { get; set; }
        public string InventoryFloorName { get; set; }
        public string InventoryFloorDesc { get; set; }
        public int StatusId { get; set; }
        public int OrderSequence { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int? UpdateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual ICollection<InventoryItem> InventoryItem { get; set; }
    }
}
