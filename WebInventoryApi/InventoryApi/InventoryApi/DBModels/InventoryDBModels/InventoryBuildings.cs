using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.InventoryDBModels
{
    [Table("InventoryBuildings")]
    public class InventoryBuildings
    {
        [Key]
        public int InventoryBuildingID{get; set;}
        public int? ClientID{get; set;}
        public string InventoryBuildingCode{get; set;}
        public string InventoryBuildingName{get; set;}
        public string InventoryBuildingDesc{get; set;}
        public int StatusID {get; set;}
        public int OrderSequence { get; set;}
        public int CreateID { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int? UpdateID { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}
