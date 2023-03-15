using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Models
{
    public class ClientInventoryModel
    {
        public int client_id { get; set; }
        //public int ssidb_client_id { get; set; }
        //public int teamdesign_cust_no { get; set; }
        public string client_name { get; set; }
        public bool has_inventory { get; set; }

        public int? CustId { get; set; }
      
    }
}
