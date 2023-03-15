using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Models
{
    public class InventoryCartOrderItemModel
    {
       public int inv_item_id{get; set;}
       public string building{get; set;}
       public string floor{get; set;}
       public string mploc{get; set;}
       public string cond{get; set;}
       public int qty{get; set;}
       public int inventory_id{get; set;}
       public string inv_image_name{get; set;}    
       public string inv_image_url{get; set;}    
       public string item_code{get; set;}
       public string description{get; set;}  
       public int client_id{get; set;}
       public int pullqty{get; set;}
       public bool isSelected{ get; set; }
       public string destbuilding{get; set;}
       public string destfloor{get; set;}
       public string destroom{get; set;}
       public string inst_date{get; set;}
       public string comment{get; set;}
       //public string username{get; set;}
       public string reqname{get; set; }
        public string email{get; set;}

        public string client_name { get; set; }
        public string destdepcostcenter { get; set; }
    }
}
