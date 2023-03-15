using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.InventoryDBModels
{
    [Table("SpaceTypes")]
    public class SpaceTypes
    {
        [Key]
       public int SpaceTypeID {get; set;}
       public int ClientID {get; set;}
       public string SpaceTypeName {get; set;}
       public string? SpaceTypeDesc {get; set;}
       public DateTime CreateDateTime {get; set;}
       public int CreateID {get; set;}
       public DateTime? UpdateDateTime {get; set;}
       public int? UpdateID {get; set;}
    }
}
