using System.Collections.Generic;

namespace SSInventory.Share.Models.Dto.InventoryItem
{
    public class InventoryItemFilterModel
    {
        public List<int> Ids { get; set; }
        public int? ClientId { get; set; }
        public List<int> BuildingIds { get; set; }
        public List<int> FloorIds { get; set; }
        public string Room { get; set; }
        public int ItemTypeId { get; set; } = 0;
        //Searching for PartNumber and Description in Inventory table
        public string SearchString { get; set; }

        public List<int> inventoryIds { get; set; }
        public bool IncludeItemImages { get; set; }
        public int? ConditionId { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 10;
    }
}
