using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Models
{
    public class InventoryItemModel
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

        public int client_id { get; set; }


        //public int inventory_id { get; set; }
        //public List<InvItemDetailsModel> invItemDetailsModels { get; set; }
    }

    //public class InvItemDetailsModel
    //{
    //    public string building { get; set; }
    //    public string floor { get; set; }
    //    public string mploc { get; set; }
    //    public string cond { get; set; }
    //    public int qty { get; set; }

    //    public int inv_id { get; set; }
    //    public string inv_image_name { get; set; }
    //}

    //public class InventoryItemAddCartPopupModel
    //{

    //}
}
