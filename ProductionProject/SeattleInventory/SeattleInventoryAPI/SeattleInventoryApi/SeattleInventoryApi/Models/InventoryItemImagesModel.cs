using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Models
{
    public class InventoryItemImagesModel
    {
        public int inv_item_img_id { get; set; }
        public int inv_item_id { get; set; }
        public Guid inv_item_img_guid { get; set; }
        public string image_name { get; set; }
        public string image_url { get; set; } 
        public int? client_id { get;set; }
        public string location { get; set; }

        //      public InventoryItemModel inventoryItemModel { get; set; }
    }
}
