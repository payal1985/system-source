using Newtonsoft.Json;
using SSInventory.Share.Models.Dto.InventoryImage;
using SSInventory.Share.Models.Dto.InventoryItem;
using System;
using System.Collections.Generic;

namespace SSInventory.Share.Models.Dto.Inventory
{
    public class InventoryModel
    {
        public InventoryModel()
        {
            InventoryItems = new List<InventoryItemModel>();
        }

        public int? InventoryId { get; set; }
        public int ClientId { get; set; }
        public int? ItemTypeId { get; set; }
        public int? ItemRowId { get; set; }
        public string ItemCode { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string Fabric { get; set; }
        public string Finish { get; set; }
        public string Fabric2 { get; set; }
        public string Finish2 { get; set; }
        public string Size { get; set; }
        public string Unit { get; set; }
        public string Rfidcode { get; set; }
        public string Barcode { get; set; }
        public string Ownership { get; set; }
        public string PartNumber { get; set; }
        public decimal Diameter { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Depth { get; set; }
        public string Top { get; set; }
        public string Edge { get; set; }
        public string Base { get; set; }
        public string Frame { get; set; }
        public string Seat { get; set; }
        public string Back { get; set; }
        public string SeatHeight { get; set; }
        public string Createby { get; set; }
        public string Updateby { get; set; }
        public DateTime? DeviceDate { get; set; }
        public int? SubmissionId { get; set; }
        public string Modular { get; set; }
        public string MainImage { get; set; }
        public DateTime? Createdate { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
        public string QRcode { get; set; }

        [JsonIgnore]
        public List<InventoryItemModel> InventoryItems { get; set; }
        public List<InventoryImageModel> InventoryImageModels { get; set; } 
        [JsonIgnore]
        public Dictionary<int, int> AmountOfRepresentiveImages { get; set; } = new Dictionary<int, int>();
        public DateTime? SubmissionDate { get; set; }
        public int? ConditionRender { get; set; }
        public int? TempParentRowId { get; set; }
        public int? ParentRowId { get; set; }
        public string Tag { get; set; }
        public int? GlobalProductId { get; set; }
        public ItemTypeModel ItemType { get; set; }

    }
    public class DataInventoryType
    {
        public int inventoryId { get; set; }
        public int clientId { get; set; }
        public string mainImage { get; set; }
        public string itemTypeCode { get; set; }
        public string itemTypeName { get; set; }
        public int itemTypeId { get; set; }
        public string note { get; set; }
        public string tag { get; set; }
        public string description { get; set; }
        public string manufacturerName { get; set; }
        public int? totalQuantity { get; set; }
        public int statusId { get; set; }
        public int? GlobalProductCatalogID { get; set; }
        public string QRCode { get; set; }
        public int WarrantyYears { get; set; }
    }

    public class DataInventoryTypeWithFirstItemLocationInfo: DataInventoryType
    {
        public string Building { get; set; }

        public string Room { get; set; }

        public string Floor { get; set; }

        public string ConditionName { get; set; }
    }
}
