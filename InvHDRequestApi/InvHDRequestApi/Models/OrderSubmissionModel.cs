using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.Models
{
    public class OrderSubmissionModel
    {
        public int InventoryID { get; set; }
        public int InventoryItemID { get; set; }
        public int InventoryBuildingID { get; set; }
        public int InventoryFloorID { get; set; }
        public int ConditionId { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string Condition { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public int PullQty { get; set; }
        public int DestInventoryBuildingID { get; set; }
        public int DestInventoryFloorID { get; set; }
        public string DestBuilding { get; set; }
        public string DestFloor { get; set; }
        public string DestRoom { get; set; }
        public string InstDate { get; set; }
        public string Comment { get; set; }
        public string DestDepCostCenter { get; set; }
        public string RequestForName { get; set; }
        public string Email { get; set; }
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string ClientPath { get; set; }
        public int RequestorId { get; set; }
        public bool IsSelected { get; set; }
        public List<ChildInventoryItemModel> ChildInventoryItemModels { get; set; }

        public string? ClientNote { get; set; }
        public string InventoryImageName { get; set; }
    }
}
