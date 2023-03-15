using System;

namespace SSInventory.Share.Models.Dto.InventoryItem
{
    public class InventoryItemSearchResultModel
    {
        public int InventoryId { get; set; }
        public int InventoryItemId { get; set; }
        public string Condition { get; set; }
        public int InventoryBuildingId { get; set; }
        public string Building { get; set; }
        public int InventoryFloorId { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string MainImage { get; set; }
        public int ItemTypeId { get; set; }
        public string ItemType { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string DamageNotes { get; set; }
        public bool AddedToCartItem { get; set; }
    }
    public class SimpleInventoryItemSearchResultModel
    {

        public int? InventoryId { get; set; }
        public int? InventoryItemID { get; set; }
        public string Condition { get; set; }
        public int ClientID { get; set; }
        public int BuildingID { get; set; }
        public string Building { get; set; }
        public int InventoryFloorId { get; set; }
        public string Floor { get; set; }
        public string Notes { get; set; }
        public string Room { get; set; }
        public string MainImage { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeName { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int StatusID { get; set; }
        public bool AddedToCartItem { get; set; }
    }
}