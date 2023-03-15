using System.ComponentModel.DataAnnotations;

namespace SSInventory.Share.Models
{
    public class ItemTypeOptionLineModel
    {
        public int ItemTypeOptionLineID { get; set; }

        public int? ClientID { get; set; }

        public int? ItemTypeOptionID { get; set; }
        public virtual ItemTypeOptionModel ItemTypeOption { get; set; }

        [MaxLength(100)]
        public string ItemTypeOptionLineCode { get; set; }

        [MaxLength(100)]
        public string ItemTypeOptionLineName { get; set; }

        [MaxLength(250)]
        public string ItemTypeOptionLineDesc { get; set; }

        public int? OrderSequence { get; set; }

        public string InventoryUserAcceptanceRulesRequired { get; set; }
    }
}
