using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Models
{
    public class InventoryFloorsModel
    {
        public int client_id { get; set; }
        public string Floor { get; set; }
        public string UserName { get; set; }
    }
}
