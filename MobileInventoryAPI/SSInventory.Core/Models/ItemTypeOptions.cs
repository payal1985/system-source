using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class ItemTypeOptions
    {
        public ItemTypeOptions()
        {
            ItemTypeOptionLines = new HashSet<ItemTypeOptionLines>();
        }

        public int ItemTypeOptionId { get; set; }
        public int ClientId { get; set; }
        public int ItemTypeId { get; set; }
        public string ItemTypeOptionCode { get; set; }
        public string ItemTypeOptionName { get; set; }
        public string ItemTypeOptionDesc { get; set; }
        public int OrderSequence { get; set; }
        public int ValType { get; set; }
        public int StatusId { get; set; }
        public int LimitMin { get; set; }
        public int LimitMax { get; set; }
        public bool IsHide { get; set; }
        public bool IsRequired { get; set; }
        public string FieldType { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
        public int ItemTypeSupportFileId { get; set; }
        public int ItemTypeAttributeId { get; set; }

        public virtual ItemTypes ItemType { get; set; }
        public virtual ICollection<ItemTypeOptionLines> ItemTypeOptionLines { get; set; }
    }
}
