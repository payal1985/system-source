using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.DBModels.InventoryDBModels
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public string Email { get; set; }
        public string RequestForName { get; set; }
        public int? RequestorID { get; set; }
        public int ClientID { get; set; }
        public int OrderTypeID { get; set; }
        public int StatusID { get; set; }
        public int? RequestID { get; set; }
        public int? AssignedToID { get; set; }
        public string? Barcode { get; set; }
        public string? ActionNote{ get; set; }
        public string? ClientNote { get; set; }
        public string? InstallerInstruction { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateID { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateID { get; set; }
        public int? AssignToCompanyID { get; set; }

    }
}
