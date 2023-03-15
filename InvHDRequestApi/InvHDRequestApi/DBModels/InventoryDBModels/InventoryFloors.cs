using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.DBModels.InventoryDBModels
{
    [Table("InventoryFloors")]
    public class InventoryFloors
    {
        [Key]
        public int InventoryFloorID{get; set;}
        public int? ClientID{get; set;}
        public string InventoryFloorCode{get; set;}
        public string InventoryFloorName{get; set;}
        public string InventoryFloorDesc{get; set;}
        public int StatusID{get; set;}
        public int OrderSequence { get; set;}
        public DateTime CreateDateTime { get; set;}
        public int CreateID { get; set;}
        public DateTime? UpdateDateTime { get; set;}
        public int? UpdateID { get; set;}
    }
}
