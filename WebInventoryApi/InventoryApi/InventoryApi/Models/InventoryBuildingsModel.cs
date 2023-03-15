using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class InventoryBuildingsModel
    {
        public int? ClientID { get; set; }
        public int InventoryBuildingId { get; set; }
        public string InventoryBuildingName { get; set; }
        public int UserId { get; set; }       
        public int OrderSequence { get; set;}

    }
}
