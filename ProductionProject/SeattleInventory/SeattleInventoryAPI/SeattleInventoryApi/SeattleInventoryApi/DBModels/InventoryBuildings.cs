using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBModels
{
    [Table("InventoryBuildings")]
    public class InventoryBuildings
    {
        [Key]
        public int InventoryBuildingID{get; set;}
        public int? ClientID{get; set;}
        public string? InventoryBuildingCode{get; set;}
        public string? InventoryBuildingName{get; set;}
        public string? InventoryBuildingDesc{get; set;}
        public int? StatusID{get; set;}
        public int? OrderBy{get; set;}
        public int? CreatedBy{get; set;}
        public DateTime? CreatedDate{get; set;}
        public int? LastModBy{get; set;}
        public DateTime? LastModDate{get; set;}



    }
}
