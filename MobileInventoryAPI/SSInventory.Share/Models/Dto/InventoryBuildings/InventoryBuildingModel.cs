namespace SSInventory.Share.Models.Dto.InventoryBuildings
{
    public class InventoryBuildingModel
    {
        public int InventoryBuildingId { get; set; }
        public int? ClientId { get; set; }
        public string InventoryBuildingCode { get; set; }
        public string InventoryBuildingName { get; set; }
        public string InventoryBuildingDesc { get; set; }
    }
}
