using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.Models
{
    public class OrderModel
    {
        public string RequestorEmail { get; set; }
        public string RequestorForName { get; set; }
        public string ClientNote { get; set; }
        public List<Cart> CartItem { get; set; }

    }
    public class Cart
    {
        public int InventoryItemID { get; set; }
        public int BuildingId { get; set; }
        public int FloorId { get; set; }
        public string Room { get; set; }
        public string Condition { get; set; }
        public int ConditionId { get; set; }
        public int Qty { get; set; }
        public int InventoryID { get; set; }
        public string InventoryImageName { get; set; }
        public string InventoryImageUrl { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int PullQty { get; set; }
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string ClientPath { get; set; }
        public int RequestorId { get; set; }

        public List<ChildInventoryItemModel> ChildInventoryItemModels { get; set; }

        public string DestBuilding { get; set; }
        public string DestFloor { get; set; }
        public string DestRoom { get; set; }
        public string InstDate { get; set; }
        public string Comments { get; set; }
        public string DepartmentCostCenter { get; set; }
        public int DestInventoryBuildingID { get; set; }
        public int DestInventoryFloorID { get; set; }
    }

}
