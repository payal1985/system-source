using SSInventory.Share.Models.Dto.ItemTypes;

namespace SSInventory.Share.Models.Dto.InventoryItem
{
    public class UpdateInventoryModel
    {
        public int UserId { get; set; }
        public DataItemType InventoryItem { get; set; }
    }
}
