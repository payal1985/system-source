using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class InventoryBuildings
    {
        public InventoryBuildings()
        {
            InventoryItem = new HashSet<InventoryItem>();
        }

        public int InventoryBuildingId { get; set; }
        public int? ClientId { get; set; }
        public string InventoryBuildingCode { get; set; }
        public string InventoryBuildingName { get; set; }
        public string InventoryBuildingDesc { get; set; }
        public int StatusId { get; set; }
        public int OrderSequence { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int? UpdateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual ICollection<InventoryItem> InventoryItem { get; set; }
    }
}
