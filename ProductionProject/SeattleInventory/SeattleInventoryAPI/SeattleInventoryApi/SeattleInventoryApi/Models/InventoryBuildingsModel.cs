using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Models
{
    public class InventoryBuildingsModel
    {
        public int client_id { get; set; }
        public string Building { get; set; }
        public string UserName { get; set; }        
    }
}
