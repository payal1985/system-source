using System;

namespace SSInventory.Share.Models.Dto.ItemTypeOptionLines
{
    public class CreateOrUpdateItemTypeOptionLineModel
    {
        public int? ItemTypeOptionLineId { get; set; }
        public int? ClientId { get; set; }
        public int? ItemTypeOptionId { get; set; }
        public string ItemTypeOptionLineCode { get; set; }
        public string ItemTypeOptionLineName { get; set; }
        public string ItemTypeOptionLineDesc { get; set; }
        public int? OrderSequence { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
    }
}
