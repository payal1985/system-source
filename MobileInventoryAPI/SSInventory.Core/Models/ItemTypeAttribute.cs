using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class ItemTypeAttribute
    {
        public ItemTypeAttribute()
        {
            ItemTypes = new HashSet<ItemTypes>();
        }

        public int ItemTypeAttributeId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }

        public virtual ICollection<ItemTypes> ItemTypes { get; set; }
    }
}
