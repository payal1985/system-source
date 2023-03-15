using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.DBModels.InventoryDBModels
{
    [Table("Inventory")]

    public class Inventory
    {
        [Key]
        public int InventoryID { get; set; }
        public int ClientID { get; set; }
        public int ItemTypeID { get; set; }
        public int ItemRowID { get; set; }
        public string ItemCode { get; set; }
        public int StatusID { get; set; }
        public int? GlobalProductCatalogID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int ManufacturerID { get; set; }
        public string ManufacturerName { get; set; }
        public string Fabric { get; set; }
        public string Fabric2 { get; set; }
        public string Finish { get; set; }
        public string Finish2 { get; set; }
        public string Size { get; set; }
        public string RFIDCode { get; set; }
        public string BarCode { get; set; }
        public string? QRCode { get; set; }
        public string PartNumber { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Depth { get; set; }
        public decimal? Diameter { get; set; }
        public decimal? SeatHeight { get; set; }
        public decimal? ArmHeight { get; set; }
        public decimal? SeatDepth { get; set; }
        public string Top { get; set; }
        public string Edge { get; set; }
        public string Base { get; set; }
        public string Frame { get; set; }
        public string Seat { get; set; }
        public string Back { get; set; }
        public string Modular { get; set; }
        public string MainImage { get; set; }
        public int WarrantyYears { get; set; }
        public string? Tag { get; set; }
        public string Unit { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateID { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateID { get; set; }
        public DateTime DeviceDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int? SubmissionID { get; set; }

        // public List<InventoryItem> InventoryItems { get; set; }

    }
}
