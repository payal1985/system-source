using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.InventoryDBModels
{
    [Table("ItemTypes")]
    public class ItemTypes
    {
       [Key]
        public int ItemTypeID{get; set;}
       public int ClientID{get; set;}
       public string ItemTypeCode{get; set;}
       public string ItemTypeName{get; set;}
       public string ItemTypeDesc { get; set;}
       public int OrderSequence{get; set;}
       public bool IsNewType{get; set;}
       public int ItemTypeAttributeID{get; set;}
       public DateTime CreateDateTime{get; set;}
       public int CreateID{get; set;}
       public DateTime? UpdateDateTime {get; set;}
       public int? UpdateID{get; set;}
        public int StatusID { get; set; }

    }
}
