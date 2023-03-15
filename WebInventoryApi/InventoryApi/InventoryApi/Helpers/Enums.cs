using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Helpers
{
    public class Enums
    {
        public enum Status
        {
            [Display(Name = "Active")]
            Active = 5,
            [Display(Name = "Inactive")]
            Inactive = 6,
            [Display(Name = "Reserved")]
            Reserved = 7,
            [Display(Name = "Open")]
            OrdOpen = 11,
            [Display(Name = "Approval Pending")]
            OrdApprovalPending = 12,
            [Display(Name = "Work in Progress")]
            OrdWorkinProgress = 13,
            [Display(Name = "Closed")]
            OrdClosed = 14,
            [Display(Name = "Cancelled")]
            OrdCancelled = 15,
            [Display(Name = "Warranty Service")]
            WarrantyService = 16,
            [Display(Name = "Maintenance")]
            Maintenance = 17,
            [Display(Name = "New")]
            New = 18,
            [Display(Name = "Complete")]
            OrdComplete = 19,
            [Display(Name = "Incomplete")]
            OrdIncomplete = 20,
            [Display(Name = "Assigned")]
            OrdAssigned = 21
        }
        public enum Condition
        {           
	        New  = 1,
	        Good = 2,
	        Fair = 3,
	        Poor = 4,
	        Damaged = 5,
	        MissingParts = 6,
        }
        public enum OrderType
        {            
            Relocate = 1,            
            Warranty = 2,            
            Clean = 3,           
            NonWarrantyRepair =4,
            NewInstallation=6
        }

        public enum DestBuilding
        {
            [Display(Name = "Dispose / Landfill")]
            DisposeLandfill = -4,
            [Display(Name = "Dispose / Donation")]
            DisposeDonation = -3,
            [Display(Name = "Dispose / Recycle")]
            DisposeRecycle = -2,
            [Display(Name = "Dispose / Internal Sale")]
            DisposeInternalSale  = -1
        }

        public enum ColumnMapper
        {
            [Description("ItemCode")]
            itemCode,
            [Description("Category")]
            category,
            [Description("Description")]
            description,
            [Description("Finish")]
            finish,
            [Description("Notes")]
            notes,
            [Description("OtherNotes")]
            otherNotes,
            [Description("AdditionalDescription")]
            additionalDescription,
            [Description("Condition")]
            condition
        }
    }

}
