using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.Models
{
    public class ClientLocationsModel
    {
        public int ClientId { get; set; }
        public int InventoryBuildingId { get; set; }
        public string InventoryBuildingName { get; set; }
        public int UserId { get; set; }
    }
}
