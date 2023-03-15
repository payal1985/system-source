using SSInventory.Share.Models.Dto.InventoryItem;
using System.Collections.Generic;

namespace SSInventory.Share.Models.Dto.ItemTypes
{
    public class ItemTypeApiModel
    {
        public string DeviceDate { get; set; }
        public string Client { get; set; }
        public int? ClientId { get; set; }
        public string Status { get; set; }
        public int? LoginUserID { get; set; }
        public int? InventoryClientGroupID { get; set; }
        public bool IsBarScan { get; set; }
        public string InventoryAppId { get; set; }
        public int InventoryBuildingId { get; set; }
        public int InventoryFloorId { get; set; }
        public string EmailUser { get; set; }
        public string ApproverEmail { get; set; }
        public List<DataItemType> DataItemType { get; set; }
    }

    public class DataItemType
    {
        public int ItemTypeID { get; set; }
        public int? ClientId { get; set; }
        public string ItemCode { get; set; }
        public int InventoryRowId { get; set; }
        public string MainImage { get; set; }
 
        //ParentId is InventoryId column
        public int? ParentRowId { get; set; }
        public int? ConditionRender { get; set; }
        public int? InventoryItemId { get; set; }
        public int InventoryId { get; set; }
        // Update Inventory Item
        public string NoteForItem { get; set; } = "";
        public int StatusId { get; set; }
        public string Condition { get; set; } = "";
        public int ConditionId { get; set; }
        // End Update Inventory Item
        public string QRcode { get; set; }
        public int WarrantyYears { get; set; }
        public int? GlobalProductCatalogID { get; set; }
        public List<ItemTypeOptionApiModel> ItemTypeOptions { get; set; }
    }
    public class ItemTypeOptionApiModel
    {
        public string ItemTypeOptionCode { get; set; }
        public string ItemTypeOptionName { get; set; }
        public object ItemTypeOptionReturnValue { get; set; }
    }

    public class ItemTypeOptionReturnCondition
    {
        public string ConditionName { get; set; }
        public int ConditionId { get; set; }
        public string Note { get; set; }
        public string DamageNotes { get; set; }
        public int TxtQuantity { get; set; }
        public bool ExistedItems { get; set; }
        public List<ConditionData> ConditionData { get; set; }
        public List<RepresentativePhoto> RepresentativePhotos { get; set; }
    }

    public class ConditionData
    {
        public string ItemName { get; set; }
        public List<DataConditionUrl> Url { get; set; }
        public string DamageNotes { get; set; }
    }

    public class DataConditionUrl
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public int? ItemTypeAutomationOptionId { get; set; }
        public int? ItemTypeAutomationId { get; set; }
        public string TempPhotoName { get; set; }
        public int? InventoryItemId { get; set; }
        public int? InventoryImageId { get; set; }
    }

    public class PhotoUploadModel
    {
        public string Base64 { get; set; }
        public int ClientId { get; set; }
        public int ItemTypeAutomationId { get; set; }
        public int ItemTypeAutomationOptionId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string PhotoId { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
    }

    public class SelectedItemOptionModel
    {
        public int ItemTypeOptionLineID { get; set; }
        public string ItemTypeOptionLineCode { get; set; }
        public string ItemTypeOptionLineName { get; set; }
        public int? ItemTypeOptionID { get; set; }
    }

    public class SelectItemOptionModel
    {
        public string ReturnValue { get; set; }
        public int ReturnID { get; set; }
        public string ReturnCode { get; set; }
        public string ReturnName { get; set; }
        public string ReturnDesc { get; set; }
    }

    public class ItemTypeOptionReturnConditionMinimal
    {
        public string NameCondition { get; set; }
        public ConditionData DataCondition { get; set; }
    }
}