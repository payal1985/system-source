using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class InventoryOrderItemModel
    {
        public int OrderID { get; set; }
        public int InventoryID { get; set; }
        public int InventoryItemID { get; set; }
        public string Email { get; set; }
        public string Project { get; set; }
        public DateTime InstDate { get; set; }
        public int? DestBuildingId { get; set; }
        public int DestFloorId { get; set; }
        public string DestRoom { get; set; }
        public int Qty { get; set; }
        public string Category { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string Building { get; set; }
        public int BuildingId { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string ImageBase64 { get; set; }
        public int ClientID { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public string Condition { get; set; }
        public int OrderItemId { get; set; }
        public string DestBuilding { get; set; }
        public string DestFloor { get; set; }
        public int ConditionId { get; set; }

        public string BucketName { get; set; }
        public string ImagePath { get; set; }

        public int? RequestId { get; set; }

        public bool IsEdit { get; set; }

        public int StatusId { get; set; }
        public string Status { get; set; }

        public string ActionNote { get; set; }
        public string ClientNote { get; set; }
        public string InstallerInstruction { get; set; }
        public string Comment { get; set; }

        public bool CompleteBtnFlag { get; set; }

        public bool IsCompletedRow { get; set; }
        public bool IsCompletedRowItem { get; set; }

        public int OrderTypeId { get; set; }

        public bool IsClosedRow { get; set; }

        public string ProposalNumber { get; set; }
        public string PoOrderNo { get; set; }

    }
}
