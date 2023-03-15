using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.InventoryDBModels
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int InventoryItemID { get; set; }
        public int InventoryID { get; set; }
        public string ItemCode { get; set; }
        public int StatusID { get; set; }
       // public int? LocationID { get; set; }
        public string DestRoom { get; set; }
        public int? DestBuildingID { get; set; }
        public int DestFloorID { get; set; }
        public string DepartmentCostCenter { get; set; }
        public DateTime InstallDate { get; set; }
        public DateTime? InstallDateReturn { get; set; }
        public bool Delivered { get; set; }
        public int? DeliveredByID { get; set; }
        //public bool Completed { get; set; }
        public string Comments { get; set; }
        public int Qty { get; set; }
        public int ClientID { get; set; }          
        public DateTime CreateDateTime { get; set; }
        public int CreateID { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateID { get; set; }
        public string? ActionNote { get; set; }

    }
}
