using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class InventoryFloorsModel
    {
        public int InventoryFloorId { get; set; }
        public int? ClientID { get; set; }
        public string InventoryFloorName { get; set; }
        public int UserId { get; set; }
    }
}
