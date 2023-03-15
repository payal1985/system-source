using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class InventoryModel
    {
        public int InventoryID { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string ManufacturerName { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Depth { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public string ImageBase64 { get; set; }
        public int Qty { get; set; }
        public string Fabric { get; set; }
        public string Finish { get; set; }

        public string TootipInventoryItem { get; set; }

        public string HWDStr { get; set; }

        public int ClientID { get; set; }

        public string Category { get; set; }

        public int ReservedQty { get; set; }
        public string TootipReservedInventoryItem { get; set; }

        public int WarrantyYears { get; set; }

        public bool MissingParts { get; set; }

        public List<InventoryItemModel> InventoryItemModels { get; set; }
        public List<InventoryItemModel> inventoryItemModelsDisplay { get; set; }

        public string BucketName { get; set; }
        public string ImagePath { get; set; }
        public string FilePath { get; set; }
        public string WarrantyFilePath { get; set; }
        public string Top { get; set; }
        public string Edge { get; set; }
        public string Base { get; set; }
        public string Frame { get; set; }
        public string Seat { get; set; }
        public string Back { get; set; }
        public decimal? SeatHeight { get; set; }
        public string? Tag { get; set; }
        public string PartNumber { get; set; }


    }
}
