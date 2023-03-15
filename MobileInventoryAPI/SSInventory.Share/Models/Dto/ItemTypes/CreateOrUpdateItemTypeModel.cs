using System;
using System.ComponentModel.DataAnnotations;

namespace SSInventory.Share.Models.Dto.ItemTypes
{
    public class CreateOrUpdateItemTypeModel
    {
        public int ItemTypeId { get; set; }
        public int ClientId { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string ItemTypeCode { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string ItemTypeName { get; set; }
        [StringLength(255, MinimumLength = 0)]
        public string ItemTypeDesc { get; set; }
        public int OrderSequence { get; set; } = 0;
        public bool IsNewType { get; set; }
        public int ItemTypeAttributeId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
    }
}
