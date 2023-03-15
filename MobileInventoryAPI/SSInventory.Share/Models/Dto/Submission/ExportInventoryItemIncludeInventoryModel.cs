namespace SSInventory.Share.Models.Dto.Submission
{
    public class ExportInventoryItemIncludeInventoryModel
    {
        public int InventoryId { get; set; }
        public string Description { get; set; }
        public string Fabric { get; set; }
        public string Finish { get; set; }
        public string Fabric2 { get; set; }
        public string Finish2 { get; set; }
        public string Size { get; set; }
        public string Unit { get; set; }
        public string Ownership { get; set; }
        public string PartNumber { get; set; }
        public decimal Diameter { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Depth { get; set; }
        public string Top { get; set; }
        public string Edge { get; set; }
        public string Base { get; set; }
        public string Frame { get; set; }
        public string Seat { get; set; }
        public string Back { get; set; }
        public string SeatHeight { get; set; }
        public string Modular { get; set; }
        public string MainImage { get; set; }
        public string Tag { get; set; }
        // Inventory Item Model
        public int InventoryItemId { get; set; }
        public int LocationId { get; set; }
        public string Room { get; set; }
        public string Condition { get; set; }
        public string Notes { get; set; }
        public string GpsLocation { get; set; }
        public string Rfidcode { get; set; }
        public int StatusId { get; set; }
    }
}
