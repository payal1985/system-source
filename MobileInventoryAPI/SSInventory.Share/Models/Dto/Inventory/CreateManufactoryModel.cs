namespace SSInventory.Share.Models.Dto.Inventory
{
    public class CreateManufacturerModel
    {
        public int ClientId { get; set; }
        // ItemTypeOptionId
        public int ItemTypeOptionId { get; set; }
        public string OptionCode { get; set; }
        public string OptionName { get; set; }
        public string OptionDesc { get; set; }
    }
}
