using System;
using System.Collections.Generic;

#nullable disable

namespace SSInventory.Core.Models
{
    public partial class Manufacturers
    {
        public int ManufacturerId { get; set; }
        public int SsimanufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string SsimanufacturerName { get; set; }
        public bool? Active { get; set; }
        public string VendorNum { get; set; }
        public string Pmtype { get; set; }
        public string DisplayName { get; set; }
        public bool IsDsrenabled { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateId { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public int? UpdateId { get; set; }
    }
}
