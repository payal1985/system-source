using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class ItemTypeOptionLines
    {
        public int ItemTypeOptionLineId { get; set; }
        public int ClientId { get; set; }
        public int ItemTypeOptionId { get; set; }
        public string ItemTypeOptionLineCode { get; set; }
        public string ItemTypeOptionLineName { get; set; }
        public string ItemTypeOptionLineDesc { get; set; }
        public int OrderSequence { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
        public string InventoryUserAcceptanceRulesRequired { get; set; }
        public int? StatusId { get; set; }

        public virtual ItemTypeOptions ItemTypeOption { get; set; }
    }
}
