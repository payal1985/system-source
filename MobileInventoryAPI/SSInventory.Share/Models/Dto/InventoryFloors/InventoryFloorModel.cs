namespace SSInventory.Share.Models.Dto.InventoryFloors
{
    public class InventoryFloorModel
    {
        public int InventoryFloorId { get; set; }
        public int? ClientId { get; set; }
        public string InventoryFloorCode { get; set; }
        public string InventoryFloorName { get; set; }
        public string InventoryFloorDesc { get; set; }
    }
}
