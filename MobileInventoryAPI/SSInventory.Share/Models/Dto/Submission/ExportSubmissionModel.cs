using System;
using System.Collections.Generic;

namespace SSInventory.Share.Models.Dto.Submission
{
    public class ExportSubmissionModel
    {
        public int SubmissionId { get; set; }
        public int ClientId { get; set; }
        public string Client { get; set; }
        public string Status { get; set; }
        public string InventoryAppId { get; set; }
        public List<ExportInventoryModel> Inventories { get; set; } = new List<ExportInventoryModel>();

        // For sending email
        public List<string> UserEmails { get; set; } = new List<string>();
        public List<string> ApproverEmails { get; set; } = new List<string>();
    }

    public class ExportInventoryModel
    {
        public int InventoryId { get; set; }
        public int ItemTypeId { get; set; }
        public string ItemTypeText { get; set; }
        public int ItemRowId { get; set; }
        public string ItemCode { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string Fabric { get; set; }
        public string Finish { get; set; }
        public string Fabric2 { get; set; }
        public string Finish2 { get; set; }
        public string Size { get; set; }
        public string Unit { get; set; }
        public string Rfidcode { get; set; }
        public string Barcode { get; set; }
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

        public List<ExportInventoryItemModel> InventoryItems { get; set; } = new List<ExportInventoryItemModel>();
    }

    public class ExportInventoryItemModel
    {
        public int InventoryItemId { get; set; }
        public int InventoryId { get; set; }
        public int LocationId { get; set; }
        public bool DisplayOnSite { get; set; }
        public int InventoryBuildingId { get; set; }
        public string InventoryBuildingName { get; set; }
        public int InventoryFloorId { get; set; }
        public string InventoryFloorName { get; set; }
        public string Room { get; set; }
        public int InventorySpaceTypeId { get; set; }
        public int InventoryOwnerId { get; set; }
        public string Condition { get; set; }
        public string Notes { get; set; }
        public int ClientId { get; set; }
        public string GpsLocation { get; set; }
        public string Rfidcode { get; set; }
        public string Barcode { get; set; }
        public int? SubmissionId { get; set; }
        public int StatusId { get; set; }
        public string StatusText { get; set; }
        public List<ExportInventoryItemImageModel> InventoryImages { get; set; }
    }

    public class ExportInventoryItemImageModel
    {
        public int InventoryImageId { get; set; }
        public int InventoryItemId { get; set; }
        public Guid ImageGuid { get; set; }
        public string ImageName { get; set; }
        public string TempImageUrl { get; set; }
        public string ImageUrl { get; set; }
        public int? ClientId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int? ItemTypeAutomationOptionId { get; set; }
        public int? ItemTypeAutomationId { get; set; }
        public string TempPhotoName { get; set; }
    }
}
