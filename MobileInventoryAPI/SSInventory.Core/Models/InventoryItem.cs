using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class InventoryItem
    {
        public int InventoryItemId { get; set; }
        public int InventoryId { get; set; }
        public int ClientId { get; set; }
        public bool? DisplayOnSite { get; set; }
        public int StatusId { get; set; }
        public int InventoryBuildingId { get; set; }
        public int InventoryFloorId { get; set; }
        public string Room { get; set; }
        public int InventorySpaceTypeId { get; set; }
        public int InventoryOwnerId { get; set; }
        public int ConditionId { get; set; }
        public string DamageNotes { get; set; }
        public string Gpslocation { get; set; }
        public string Rfidcode { get; set; }
        public string Barcode { get; set; }
        public string ProposalNumber { get; set; }
        public decimal? PoOrderNo { get; set; }
        public DateTime? PoOrderDate { get; set; }
        public DateTime? NonSsipurchaseDate { get; set; }
        public int? WarrantyRequestId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public int? SubmissionId { get; set; }
        public string Qrcode { get; set; }
        public bool AddedToCartItem { get; set; }

        public virtual Inventory Inventory { get; set; }
        //public virtual InventoryBuildings InventoryBuilding { get; set; }
        public virtual InventoryFloors InventoryFloor { get; set; }
        public virtual Status Status { get; set; }
    }
}