using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class DonationInventoryModel
    {
        public int InventoryID { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string AdditionalDescription { get; set; }
        public string ManufacturerName { get; set; }
        public int Qty { get; set; }
        public string Fabric { get; set; }
        public string Finish { get; set; }

        public string ImageName { get; set; }
        public string TootipInventoryItem { get; set; }

        //public int inventory_id { get; set; }
        //public string item_code { get; set; }
        //public string description { get; set; }
        //public string additional_description { get; set; }
        //public string manuf { get; set; }
        //public int qty { get; set; }
        //public string fabric { get; set; }
        //public string finish { get; set; }

        //public string image_name { get; set; }
        //public string tootip_inv_item { get; set; }


        public List<InventoryItemModel> InventoryItemModels { get; set; }

        public List<InventoryItemModel> InventoryItemModelsDisplay { get; set; }

       //public List<string> ImageUrl { get; set; }
       public string ImageUrl { get; set; }

    }
}
