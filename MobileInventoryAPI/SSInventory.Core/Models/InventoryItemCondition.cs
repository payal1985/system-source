using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class InventoryItemCondition
    {
        public int InventoryItemConditionId { get; set; }
        public string ConditionName { get; set; }
        public int StatusId { get; set; }
        public int OrderSequence { get; set; }
        public bool IsMobileCondition { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }

        public virtual Status Status { get; set; }
    }
}
