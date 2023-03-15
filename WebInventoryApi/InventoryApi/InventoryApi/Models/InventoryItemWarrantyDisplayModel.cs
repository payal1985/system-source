using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class InventoryItemWarrantyDisplayModel
    {
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string Condition { get; set; }
        public DateTime? PurchaseDate { get; set; }

        public string WarrantyYear { get; set; }
    }
}
