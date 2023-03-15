using SSInventory.Share.Models.Dto.Inventory;
using System.Collections.Generic;

namespace SSInventory.Share.Models.Search
{
    public class SearchResultModel
    {
        public int TotalItem { get; set; }

        public List<InventorySearchImageResultModel> Inventories { get; set; }
    }
}
