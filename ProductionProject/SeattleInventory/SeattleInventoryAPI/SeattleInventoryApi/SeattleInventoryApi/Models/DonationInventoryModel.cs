using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Models
{
    public class DonationInventoryModel
    {
        public int inventory_id { get; set; }
        public string item_code { get; set; }
        public string description { get; set; }
        public string additional_description { get; set; }
        public string manuf { get; set; }
        public int qty { get; set; }
        public string fabric { get; set; }
        public string finish { get; set; }

        public string image_name { get; set; }
        public string tootip_inv_item { get; set; }

 
        public List<InventoryItemModel> InventoryItemModels { get; set; }

        public List<InventoryItemModel> inventoryItemModelsDisplay { get; set; }

       public List<string> image_url { get; set; }

    }
}
