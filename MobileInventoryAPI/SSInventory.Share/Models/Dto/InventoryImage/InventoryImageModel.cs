using SSInventory.Share.Enums;
using System;

namespace SSInventory.Share.Models.Dto.InventoryImage
{
    public class InventoryImageModel: ICloneable
    {
        public int InventoryImageId { get; set; }
        public int? InventoryItemId { get; set; }
        public Guid ImageGuid { get; set; }
        public string ImageName { get; set; }
        public string TempImageUrl { get; set; }
        public string ImageUrl { get; set; }
        public int? ClientId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int? ItemTypeAutomationOptionId { get; set; }
        public int? ItemTypeAutomationId { get; set; }
        public string TempPhotoName { get; set; }

        public DateTime? SubmissionDate { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }

        public int? InventoryId { get; set; }
        public int ConditionId { get; set; }
        public string DamageNotes { get; set; }
        public InventoryImageStatus? StatusId { get; set; } = InventoryImageStatus.Active;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
