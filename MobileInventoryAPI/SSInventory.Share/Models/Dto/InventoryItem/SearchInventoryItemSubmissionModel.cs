using SSInventory.Share.Models.Dto.InventoryImage;
using System.Collections.Generic;

namespace SSInventory.Share.Models.Dto.InventoryItem
{
    public class SearchInventoryItemSubmissionModel
    {
        public int InventoryItemId { get; set; }
        public int InventoryId { get; set; }
        public string Building { get; set; }
        public int InventoryBuildingId { get; set; }
        public string Floor { get; set; }
        public int InventoryFloorId { get; set; }
        public string GpsLocation { get; set; }
        public string Condition { get; set; }
        public int? SubmissionId { get; set; }
        public string Room { get; set; }

        public List<InventoryImageModel> InventoryItemImages { get; set; }
    }
}
