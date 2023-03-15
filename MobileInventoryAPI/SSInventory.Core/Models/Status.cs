using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class Status
    {
        public Status()
        {
            InventoryItem = new HashSet<InventoryItem>();
            InventoryItemCondition = new HashSet<InventoryItemCondition>();
        }

        public int StatusId { get; set; }
        public string StatusType { get; set; }
        public string StatusName { get; set; }
        public string StatusDesc { get; set; }
        public int? SortOrder { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int? UpdateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual ICollection<InventoryItem> InventoryItem { get; set; }
        public virtual ICollection<InventoryItemCondition> InventoryItemCondition { get; set; }
    }
}
