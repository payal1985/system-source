using System.ComponentModel.DataAnnotations;

namespace SSInventory.Share.Models.Dto.Manufactory
{
    public class CreateOrUpdateManufactoryModel
    {
        public int? ItemTypeID { get; set; }

        public int ClientId { get; set; }
        [StringLength(100, MinimumLength = 0)]
        public string ItemCode { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string ItemName { get; set; }
        [StringLength(255, MinimumLength = 0)]
        public string ItemDesc { get; set; }

        public int? OrderSequence { get; set; }
        public bool? IsNewType { get; set; }
    }
}
