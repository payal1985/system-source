using System;
using System.ComponentModel.DataAnnotations;

namespace SSInventory.Share.Models.Dto.ItemTypeOption
{
    public class CreateOrUpdateItemTypeOptionModel
    {
        public int ItemTypeOptionId { get; set; }
        public int ClientId { get; set; }
        public int ItemTypeId { get; set; }
        [MaxLength(100)]
        public string ItemTypeOptionCode { get; set; }
        [MaxLength(100)]
        public string ItemTypeOptionName { get; set; }
        [MaxLength(250)]
        public string ItemTypeOptionDesc { get; set; }
        public int OrderSequence { get; set; }
        public int ValType { get; set; }
        public int StatusID { get; set; }
        public int LimitMin { get; set; }
        public int LimitMax { get; set; }
        public bool IsHide { get; set; }
        public bool IsRequired { get; set; }
        public string FieldType { get; set; }
        public int ItemTypeAttributeId { get; set; }
        public int SupportFile { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }

        public virtual ItemTypeModel ItemType { get; set; }
    }
}
