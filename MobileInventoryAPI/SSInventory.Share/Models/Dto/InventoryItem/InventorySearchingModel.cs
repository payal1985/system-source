using Newtonsoft.Json;
using SSInventory.Share.Models.Dto.InventoryImage;
using System;
using System.Collections.Generic;

namespace SSInventory.Share.Models.Dto.InventoryItem
{
    [Serializable]
    public class InventorySearchingModel
    {
        public int? ConditionID { get; set; }
        public string NameCondition { get; set; }
        public List<DataConditionModel> DataCondition { get; set; }
        public int? ImageCounter { get; set; }
        [JsonIgnore]
        public int? InventoryId { get; set; }
        public List<RepresentativePhoto> RepresentativePhotos { get; set; }
        public int TxtQuantity { get; set; }
        public int Type { get; set; }
       
    }

    [Serializable]
    public class DataConditionModel
    {
        public List<InventoryImageResponseModel> Url { get; set; }
        public int ImageCounter { get; set; }
        public int TxtQuantity { get; set; }
        public List<string> InventoryItemBarcodes { get; set; } = new List<string>();
        public string ItemName {get; set; }
        public int InventoryItemId {get; set; }
        public string DamageNotes {get; set; }
       
    }

    [Serializable]
    public class RepresentativePhoto
    {
        public int RepresentativePhotoTotalCount { get; set; }
        public List<InventoryImageResponseModel> Url { get; set; } = new List<InventoryImageResponseModel>();
    }
}
