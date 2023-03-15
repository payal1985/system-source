using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class Inventory
    {
        public Inventory()
        {
            InventoryItem = new HashSet<InventoryItem>();
        }

        public int InventoryId { get; set; }
        public int ClientId { get; set; }
        public int ItemTypeId { get; set; }
        public int ItemRowId { get; set; }
        public string ItemCode { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ManufacturerName { get; set; }
        public string Fabric { get; set; }
        public string Fabric2 { get; set; }
        public string Finish { get; set; }
        public string Finish2 { get; set; }
        public string Size { get; set; }
        public string Rfidcode { get; set; }
        public string BarCode { get; set; }
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
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
        public DateTime DeviceDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int? SubmissionId { get; set; }
        public int ManufacturerId { get; set; }
        public string Tag { get; set; }
        public string Unit { get; set; }
        public int? GlobalProductId { get; set; }
        public int? GlobalProductCatalogID { get; set; }
        public string QRCode { get; set; }
        public int WarrantyYears { get; set; }


        public virtual ItemTypes ItemType { get; set; }
        public virtual ICollection<InventoryItem> InventoryItem { get; set; }
    }
}
