using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.DBModels.InventoryDBModels
{
    [Table("InventoryItemCondition")]

    public class InventoryItemCondition
    {
        [Key]
       public int InventoryItemConditionID {get; set;}
       public string? ConditionName {get; set;}
       public int StatusID {get; set;}
       public int OrderSequence {get; set;}
       public DateTime CreateDateTime {get; set;}
       public int CreateID {get; set;}
       public DateTime? UpdateDateTime {get; set;}
       public int? UpdateID {get; set;}

    }
}
