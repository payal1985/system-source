using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.InventoryDBModels
{
    [Table("OrderType")]
    public class OrderType
    {
        [Key]
        public int OrderTypeID {get;set;}
        public string OrderTypeName { get; set; }
        public string? OrderTypeDesc { get; set; }
        public int StatusID { get; set; }
        public int? SortOrder { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateID { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateID {get;set;}
    }
}
