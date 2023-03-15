using System.Collections.Generic;

namespace SSInventory.Share.Models.Dto.InventoryItem
{
    public class InventoryItemQRcodeModel
    {
        public int ClientId { get; set; }
        public string ItemTypeName { get; set; }
        public int InventoryItemId { get; set; }
        public List<ItemTypeOption> ItemTypeOptions { get; set; }
    }

    public class ItemTypeOption
    {
        public string ItemTypeOptionCode { get; set; }
        public object ItemTypeOptionReturnValue { get; set; }
    }
}
