using System;

namespace SSInventory.Share.Models.Dto.InventoryImage
{
    [Serializable]
    public class InventoryImageResponseModel
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string TempPhotoName { get; set; }
        public int? InventoryItemId { get; set; }
        public int? InventoryImageId { get; set; }
    }
    
    [Serializable]
    public class InventoryImageV2ResponseModel
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string Name { get; set; }
        public string TempPhotoName { get; set; }
        public int? InventoryItemImgId { get; set; }
    }
}
