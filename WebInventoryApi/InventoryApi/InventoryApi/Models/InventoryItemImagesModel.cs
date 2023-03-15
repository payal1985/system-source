using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class InventoryItemImagesModel
    {
        public int InventoryItemImageID { get; set; }
        public int? InventoryItemID { get; set; }
        public Guid InventoryItemImageGuid { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public int ClientID { get; set; }
        public string Location { get; set; }
        public string ImageBase64 { get; set; }
        public string BucketName { get; set; }
        public string ImagePath { get; set; }
        //public int inv_item_img_id { get; set; }
        //public int inv_item_id { get; set; }
        //public Guid inv_item_img_guid { get; set; }
        //public string image_name { get; set; }
        //public string image_url { get; set; } 
        //public int? client_id { get;set; }
        //public string location { get; set; }

        //      public InventoryItemModel inventoryItemModel { get; set; }
    }
}
