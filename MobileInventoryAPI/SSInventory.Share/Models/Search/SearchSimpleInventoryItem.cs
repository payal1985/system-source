namespace SSInventory.Share.Models.Search
{
    public class SearchSimpleInventoryItem : Pager
    {
        public int? ClientId { get; set; }
        public int InventoryID { get; set; }
    }
}
