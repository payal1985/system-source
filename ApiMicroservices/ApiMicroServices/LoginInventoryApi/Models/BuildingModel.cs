using System.Collections.Generic;

namespace LoginInventoryApi.Models
{
    public class BuildingModel
    {
        public List<int> BuildingIds { get; set; }

        public int? ClientId { get; set; }
    }

    public class CreateInventoryBuildingModel
    {
        public int ClientId { get; set; }
        public string InventoryBuildingName { get; set; }
        public int UserId { get; set; }
    }
}