using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Models
{
    public class InventoryModel
    {
        public int inventory_id { get; set; }
        public string item_code { get; set; }
        public string description { get; set; }
        public string manuf { get; set; }
        public string height { get; set; }
        public string width { get; set; }
        public string depth { get; set; }

        //public string building { get; set; }
        //public string floor { get; set; }
        //public string mploc { get; set; }
        //public string cond { get; set; }
        public string image_name { get; set; }
        public string image_url { get; set; }

        public int qty { get; set; }
        //public int inv_item_id {get;set;}
        public string fabric { get; set; }
        public string finish { get; set; }

        public string tootip_inv_item { get; set; }

        public string hwd_str { get; set; }

        public int client_id { get; set; }

        public string category { get; set; }

        //public InventoryItemModel InventoryItemModelsList { get; set; }
        //public List<InvItemDetailsModel> invItemDetailsModel { get; set; }

        public List<InventoryItemModel> InventoryItemModels { get; set; }

        public List<InventoryItemModel> inventoryItemModelsDisplay { get; set; }
    }
}
