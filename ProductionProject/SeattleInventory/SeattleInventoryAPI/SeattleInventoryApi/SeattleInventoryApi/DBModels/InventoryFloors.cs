using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBModels
{
    [Table("InventoryFloors")]
    public class InventoryFloors
    {
        [Key]
        public int InventoryFloorID{get; set;}
        public int? ClientID{get; set;}
        public string? InventoryFloorCode{get; set;}
        public string? InventoryFloorName{get; set;}
        public string? InventoryFloorDesc{get; set;}
        public int? StatusID{get; set;}
        public int? OrderBy{get; set;}
        public int? CreatedBy{get; set;}
        public DateTime? CreatedDate{get; set;}
        public int? LastModBy{get; set;}
        public DateTime? LastModDate{get; set;}
    }
}
