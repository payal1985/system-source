using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.InventoryDBModels
{
    [Table("ItemTypeOptionLines")]
    public class ItemTypeOptionLines
    {
       [Key]
       public int ItemTypeOptionLineID {get; set;}
       public int ClientID {get; set;}
       public int ItemTypeOptionID {get; set;}
       public string ItemTypeOptionLineCode {get; set;}
       public string ItemTypeOptionLineName {get; set;}
       public string ItemTypeOptionLineDesc {get; set;}
       public int StatusID {get; set;}
       public int OrderSequence {get; set;}
       public string? InventoryUserAcceptanceRulesRequired {get; set;}
       public DateTime CreateDateTime {get; set;}
       public int CreateID {get; set;}
       public DateTime? UpdateDateTime {get; set;}
       public int? UpdateID {get; set;}
    }
}
