using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.DBModels.InventoryDBModels
{
    [Table("InventoryItem")]

    public class InventoryItem
    {
        [Key]
        public int InventoryItemID{get; set;}
        public int InventoryID{get; set;}
        //public int LocationID{get; set;}
        public int ClientID { get; set; }
        public bool DisplayOnSite{get; set;}
        public int StatusID { get; set; }
        public int InventoryBuildingID { get; set; } 
        public int InventoryFloorID { get; set; }
        public string Room{get; set; }
        public int InventorySpaceTypeID { get; set; }
        public int InventoryOwnerID { get; set; }
        public int ConditionID{get; set;}
        public string DamageNotes { get; set;}
        public string GPSLocation { get; set;}
        public string RFIDcode { get; set;}
        public string? Barcode{get; set; }
        public string? QRCode { get; set; }
        public string? ProposalNumber { get; set; }
        public decimal? PoOrderNo { get; set; }
        public DateTime? PoOrderDate { get; set; }
        public DateTime? NonSSIPurchaseDate { get; set; }
        public int? WarrantyRequestID { get; set; }
        public bool AddedToCartItem { get; set; }
        public DateTime CreateDateTime{get; set;}
        public int CreateID{get; set;}
        public DateTime? UpdateDateTime {get; set;}
        public int? UpdateID {get; set;}
        public DateTime? SubmissionDate { get; set; }
        public int? SubmissionID { get; set; }

              
        //public Inventory Inventory { get; set; }

        //public List<InventoryItemImages> InventoryItemImages { get; set; }
    }
}
