using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Models
{
    public class InventoryOrderModel
    {
        public string requestoremail { get; set; }
        public string request_individual_project { get; set; }
        public string destination_building { get; set; }
        public string destination_floor { get; set; }
        public string destination_room { get; set; }
        public string requested_inst_date { get; set; }
        public string comments { get; set; }

        public string department_cost_center { get; set; }
        public List<Cart> cart_item { get; set; }
       
    }

    public class Cart
    {
     public int inv_item_id { get; set; }
     public string building { get; set; }       
     public string floor { get; set; }
     public string mploc { get; set; }
     public string cond { get; set; }
     public int qty { get; set; }
     public int inventory_id { get; set; }
     public string inv_image_name { get; set; }
     public string inv_image_url { get; set; }        
     public string item_code { get; set; }
     public string description { get; set; }
     public int pullqty { get; set; }
     public int client_id { get; set; }
     public string client_name { get; set; }

    }
}
