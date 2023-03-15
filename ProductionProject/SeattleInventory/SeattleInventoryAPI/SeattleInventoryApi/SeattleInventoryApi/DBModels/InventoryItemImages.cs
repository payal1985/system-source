using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBModels
{
    [Table("InventoryItemImages")]

    public class InventoryItemImages
    {
        [Key]
        public int inv_item_img_id{get; set;}
        public int inv_item_id{get; set;}
        public Guid inv_item_img_guid{get; set;}
        public string image_name{get; set;}
        public string image_url{get; set;}

        public int? client_id { get; set; }
        public InventoryItem InventoryItem { get; set; }
    }
}
