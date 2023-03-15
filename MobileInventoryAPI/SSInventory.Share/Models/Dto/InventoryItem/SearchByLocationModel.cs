using SSInventory.Share.Models.Search;
using System.ComponentModel.DataAnnotations;

namespace SSInventory.Share.Models.Dto.InventoryItem
{
    public class SearchByLocationModel: Pager
    {
        [Required(ErrorMessage = "Client Id is required")]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Client ID value is invalid")]
        public int ClientId { get; set; }
        [Required(ErrorMessage = "Building is required")]
        public int? BuildingId { get; set; }
        public int? FloorId { get; set; }
        public string Room { get; set; }
        public int? ItemTypeId { get; set; }
        public string SearchString { get; set; }
        public int? ConditionId { get; set; }
    }
}
