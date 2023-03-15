using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Models
{
    public class InventoryOrderItemModel
    {
        public int order_id { get; set; }
        public int inv_id { get; set; }
        public int inv_item_id { get; set; }
        public string email { get; set; }
        public string project { get; set; }
        public DateTime instdate { get; set; }
        public string destb { get; set; }
        public string destf { get; set; }
        public string room { get; set; }
        public int qty { get; set; }
        public string category { get; set; }
        public string item_code { get; set; }
        public string description { get; set; }
        public string building { get; set; }
        public string floor { get; set; }
        public string mploc { get; set; }
        public string imgBase64 { get; set; }
 
    }
}
