using System;
using System.Collections.Generic;

namespace SSInventory.Share.Models.Dto.ItemTypeAttributes
{
    public class ItemTypeAttributeModel
    {
        public int ItemTypeAttributeId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
        public virtual List<ItemTypeModel> ItemTypes2 { get; set; }
    }
}
