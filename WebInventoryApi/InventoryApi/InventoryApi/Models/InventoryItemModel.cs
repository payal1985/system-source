using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class InventoryItemModel
    {
        public int InventoryItemID { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string Condition { get; set; }
        public int ConditionId { get; set; }
        public int Qty { get; set; }
        public int InventoryID { get; set; }
        public string InventoryImageName { get; set; }
        public string InventoryImageUrl { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int InventoryBuildingID { get; set; }
        public int InventoryFloorID { get; set; } 
        public int ClientID { get; set; }
        public string ImageBase64 { get; set; }
        public string BucketName { get; set; }
        public string ImagePath { get; set; }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public string DamageNotes { get; set; }
        public bool IsEdit { get; set; }
        public bool IsNewRow { get; set; }

        public int InventorySpaceTypeId { get; set; }
        public string InventorySpaceType { get; set; }
        public int InventoryOwnerId { get; set; }
        public string InventoryOwner { get; set; }

        public string ProposalNumber { get; set; }
        public string PoOrderNo { get; set; }
    }

}
