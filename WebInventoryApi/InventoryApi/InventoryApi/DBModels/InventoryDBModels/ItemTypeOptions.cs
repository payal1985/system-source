using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.InventoryDBModels
{
    [Table("ItemTypeOptions")]
    public class ItemTypeOptions
    {
        [Key]
        public int ItemTypeOptionID {get; set;}
        public int ClientID {get; set;}
        public int ItemTypeID {get; set;}
        public string ItemTypeOptionCode {get; set;}
        public string ItemTypeOptionName {get; set;}
        public string ItemTypeOptionDesc {get; set;}
        public int OrderSequence {get; set;}
        public int ValType {get; set;}
        public int StatusID {get; set;}
        public int LimitMin {get; set;}
        public int LimitMax {get; set;}
        public bool IsHide {get; set;}
        public bool IsRequired {get; set;}
        public string FieldType {get; set;}
        public DateTime CreateDateTime {get; set;}
        public int CreateID {get; set;}
        public DateTime? UpdateDateTime {get; set;}
        public int? UpdateID {get; set;}
        public int ItemTypeSupportFileID {get; set;}
        public int ItemTypeAttributeID {get; set;}
    }
}
