using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class ItemTypes
    {
        public ItemTypes()
        {
            Inventory = new HashSet<Inventory>();
            ItemTypeOptions = new HashSet<ItemTypeOptions>();
        }

        public int ItemTypeId { get; set; }
        public int ClientId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemTypeDesc { get; set; }
        public int OrderSequence { get; set; }
        public bool IsNewType { get; set; }
        public int ItemTypeAttributeId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }

        public virtual ItemTypeAttribute ItemTypeAttribute { get; set; }
        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<ItemTypeOptions> ItemTypeOptions { get; set; }
    }
}
