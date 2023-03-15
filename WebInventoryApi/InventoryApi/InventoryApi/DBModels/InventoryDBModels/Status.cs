using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.InventoryDBModels
{
    [Table("Status")]
    public class Status
    {
        [Key]
       public int StatusID{get;set;}
       public string StatusType{get;set;}
       public string StatusName {get;set;}
       public string? StatusDesc {get;set;}
       public int? SortOrder{get;set;}
       public int CreateID{get;set;}
       public DateTime CreateDateTime{get;set;}
       public int? UpdateID{get;set;}
       public DateTime? UpdateDateTime{get;set;}
        
    }
}
