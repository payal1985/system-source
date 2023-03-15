using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.InventoryDBModels
{
    [Table("Manufacturers")]
    public class Manufacturers
    {
        [Key]
        public int ManufacturerID{ get; set; }
        public int SSIManufacturerID { get; set; }
        public string ManufacturerName{ get; set; }
        public string SSIManufacturerName{ get; set; }
        public bool Active { get; set; }
        public string VendorNum{ get; set; }
        public string PMType{ get; set; }
        public string DisplayName{ get; set; }
        public bool IsDSREnabled { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateID { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateID { get; set; }

    }
}
