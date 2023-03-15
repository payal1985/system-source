namespace SSInventory.Share.Models.Dto.Inventory
{
    public class InventoryWithImageInfoModel
    {
        public int InventoryId { get; set; }
        public int NumberOfImages { get; set; }
        public string InventoryCode { get; set; }
        public int InventoryItemId { get; set; }
        public int InventoryImageId { get; set; }
    }
}
