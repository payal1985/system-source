using Newtonsoft.Json;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryImage;
using System;
using System.Collections.Generic;

namespace SSInventory.Share.Models.Dto.InventoryItem
{
    public class InventoryItemModel : ICloneable
    {
        public InventoryItemModel()
        {
            InventoryImages = new List<InventoryImageModel>();
        }

        public int InventoryItemId { get; set; }
        public int InventoryId { get; set; }
        public int LocationId { get; set; }
        public bool DisplayOnSite { get; set; }
        public int InventoryBuildingId { get; set; }
        public int InventoryFloorId { get; set; }
        public string Room { get; set; }
        public string Condition { get; set; }
        public int ConditionId { get; set; }
        public string DamageNotes { get; set; }
        public int ClientId { get; set; }
        public string GpsLocation { get; set; }
        public string Rfidcode { get; set; }
        public string Barcode { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? SubmissionId { get; set; }
        public int StatusId { get; set; }
        public string QRcode { get; set; }
        public int? UpdateId { get; set; }
        public InventoryModel Inventory { get; set; }
        public bool AddedToCartItem { get; set; }
        public int? PoOrderNo { get; set; }
        public DateTime? PoOrderDate { get; set; }
        [JsonIgnore]
        public bool IsIndividualItem { get; set; }
        [JsonIgnore]
        public List<InventoryImageModel> InventoryImages { get; set; }
        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
