using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class AdminInventoryItemTypesOptionSetModel
    {
        public int ClientID { get; set; }
        public int StatusID { get; set; }
        public int ItemTypeID { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeName { get; set; }
        public int ItemTypeOptionID { get; set; }
        public string ItemTypeOptionName { get; set; }
        public string ItemTypeOptionCode { get; set; }
        public int OrderSequence { get; set; }
        public string FieldType { get; set; }
        public int ItemTypeAttributeID { get; set; }
        //public int ItemTypeSupportFileID { get; set; }
        public int ValType { get; set; }
        public bool IsHide { get; set; }
        public string ValTypeDesc { get; set; }
        public int ItemTypeOptionLineID { get; set; }
        public string ItemTypeOptionLineCode { get; set; }
        public string ItemTypeOptionLineName { get; set; }
        public string InventoryUserAcceptanceRulesRequired { get; set; }
        public string ItemTypeOptionLineNameDisplay { get; set; }
    }
}
