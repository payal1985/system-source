using System;
using System.ComponentModel.DataAnnotations;

namespace SSInventory.Share.Models.Dto.InventoryBuildings
{
    public class CreateOrUpdateInventoryBuildingModel
    {
        [Required]
        public int ClientId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Building name is required")]
        public string InventoryBuildingName { get; set; }
        public string InventoryBuildingDesc { get; set; }
        protected string InventoryBuildingCode { get; set; }
        protected DateTime CreatedDate = DateTime.Now;
        protected DateTime LastModDate = DateTime.Now;
    }

    public class CreateInventoryBuildingModel
    {
        public int ClientId { get; set; }
        public string InventoryBuildingName { get; set; }
        public int UserId { get; set; }
    }
}
