using SSInventory.Share.Enums;

namespace SSInventory.Share.Models.Search
{
    public class SearchModel : Pager
    {
        public int? ClientId { get; set; }
        public int? ItemTypeId { get; set; }
        public SearchEnums SearchType { get; set; } = SearchEnums.Server;
    }
}
