using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.DBModels.InventoryDBModels
{
    [Table("InventoryImages")]

    public class InventoryImages
    {
        [Key]
        public int InventoryImageID{get; set;}
        public int InventoryID{get; set;}
        public int? InventoryItemID{get; set;}
        public int? ConditionID{get; set;}
        public Guid ImageGUID{get; set;}
        public string? ImageName {get; set;}
        public string ImageURL {get; set;}
        public int ClientID{get; set;}
        public int? Width{get; set;}
        public int? Height{get; set;}
        public int ItemTypeAutomationID{get; set;}
        public int ItemTypeAutomationOptionID{get; set;}
        public string TempPhotoName{get; set;}
        public DateTime CreateDateTime{get; set;}
        public int CreateID{get; set;}
        public DateTime? UpdateDateTime{get; set;}
        public int? UpdateID{get; set;}
        public DateTime? SubmissionDate { get; set; }
        public int StatusID { get; set; }

        //public InventoryItem InventoryItem { get; set; }
    }
}
