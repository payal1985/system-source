using SSInventory.Share.Enums;

namespace SSInventory.Share.Models.Search
{
    public class SearchImageInfo
    {
        public int? ClientGroupId { get; set; }
        public int ClientId { get; set; }
        public int InventoryId { get; set; }
        public int? InventoryItemId { get; set; }
        public SearchEnums SearchType { get; set; }
        public int? InventorySubmissionId { get; set; }
        public int? InventoryItemSubmissionId { get; set; }
    }
}
