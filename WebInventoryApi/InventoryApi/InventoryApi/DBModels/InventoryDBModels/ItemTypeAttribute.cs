using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.InventoryDBModels
{
    [Table("ItemTypeAttribute")]
    public class ItemTypeAttribute
    {
        [Key]
        public int ItemTypeAttributeID {get; set;}
        public string Name {get; set;}
        public DateTime CreateDateTime {get; set;}
        public int CreateID {get; set;}
        public DateTime? UpdateDateTime {get; set;}
        public int? UpdateID {get; set;}
    }
}
