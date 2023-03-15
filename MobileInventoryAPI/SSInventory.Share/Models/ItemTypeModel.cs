using SSInventory.Share.Models.Dto.ItemTypeOptionLines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SSInventory.Share.Models
{
    public class ItemTypeModel
    {
        public ItemTypeModel()
        {
            ItemTypeOptions = new List<ItemTypeOptionModel>();
        }
        public int ItemTypeId { get; set; }
        public int ClientId { get; set; }
        [MaxLength(100)]
        public string ItemTypeCode { get; set; }
        [MaxLength(100)]
        public string ItemTypeName { get; set; }
        [MaxLength(255)]
        public string ItemDesc { get; set; }
        public int OrderSequence { get; set; }
        public bool IsNewType { get; set; }
        public int ItemTypeAttributeId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
        public string MainImage { get; set; }
        public int StatusId { get; set; }
        public string Condition { get; set; }
        public string NoteForItem { get; set; }

        // For edit
        public int? InventoryId { get; set; }
        public int? InventoryItemId { get; set; }
        public string QRcode { get; set; }
        public int WarrantyYears { get; set; }
        public int? GlobalProductCatalogID { get; set; }
        public bool AddedToCartItem { get; set; }

        public ItemTypeAdditionalOptionsModel ItemTypeAdditionalOption{ get; set; }
        public List<ItemTypeOptionModel> ItemTypeOptions { get; set; }
    }
}
