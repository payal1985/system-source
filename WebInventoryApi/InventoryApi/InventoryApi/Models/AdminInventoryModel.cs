using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class AdminInventoryModel
    {
        public int Count { get; set; }
        public int TotalPages { get; set; }
        public int PageNo { get; set; }
        public int RowsPerPage { get; set; }

        public List<AdminInventory> AdminInventories { get; set; }
    }

    public class AdminInventory
    {
        public int InventoryID { get; set; }
        public int ClientID { get; set; }
        public int ItemTypeID {get;set;}
        public int ItemRowID {get;set;}
        public string ItemCode {get;set;}
        public int? GlobalProductCatalogID {get;set;}
        public string Category {get;set;}
        public string Description {get;set;}
        public int ManufacturerID {get;set;}
        public string ManufacturerName {get;set;}
        public string Fabric {get;set;}
        public string Fabric2 {get;set;}
        public string Finish {get;set;}
        public string Finish2 {get;set;}
        public string Size {get;set;}
        public string RFIDCode {get;set;}
        public string BarCode {get;set;}
        public string? QRCode {get;set;}
        public string PartNumber {get;set;}
        public decimal? Height {get;set;}
        public decimal? Width {get;set;}
        public decimal? Depth {get;set;}
        public decimal? Diameter {get;set;}
        public string Top {get;set;}
        public string Edge {get;set;}
        public string Base {get;set;}
        public string Frame {get;set;}
        public string Seat {get;set;}
        public string Back {get;set;}
        public decimal? SeatHeight {get;set;}
        public string Modular {get;set;}
        //public string MainImage {get;set;}
        public string ImageName { get; set; }
        public int WarrantyYears {get;set;}
        public string? Tag {get;set;}
        public string Unit {get;set;}
        public DateTime CreateDateTime {get;set;}
        public int CreateID {get;set;}
        public DateTime? UpdateDateTime {get;set;}
        public int? UpdateID {get;set;}
        public DateTime DeviceDate {get;set;}
        public DateTime SubmissionDate {get;set;}
        public string SubmissionID {get;set;}
        
        public string ImageUrl { get; set; }
        public string ImageSrc { get; set; }

        //public string Finish { get; set; }
        //public string Category { get; set; }
        //public string AdditionalDescription { get; set; }
        //public string Notes { get; set; }
        //public string OtherNotes { get; set; }


        //public bool ImageExist { get; set; }

        //public List<string> Files { get; set; }

        public List<InventoryItemModel> InventoryItemModels { get; set; }

        public int TotalQty { get; set; }
        public int ReservedQty { get; set; }

        public string ImageBase64 { get; set; }
        public string BucketName { get; set; }
        public string ImagePath { get; set; }
        public string FilePath { get; set; }

    }

    public class FileUploadAdminInventory
    {
        public IFormFile files { get; set; }
    }
}
