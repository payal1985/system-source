using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBModels
{
    [Table("InventoryItemRevisionLog")]
    public class InventoryItemRevisionLog
    {
        [Key]
        public int inv_item_rl_id { get; set; }
        public int inv_item_id { get; set; }
        public int inventory_id { get; set; }
        public int location_id { get; set; }
        public bool display_on_site { get; set; }
        public string building { get; set; }
        public string floor { get; set; }
        public string mploc { get; set; }
        public string cond { get; set; }
        public string notes { get; set; }
        public int client_id { get; set; }
        public string gps_location { get; set; }
        public string rfid { get; set; }
        public string barcode { get; set; }
        public string external_id { get; set; }
        public DateTime? original_create_date { get; set; }
        public string dest_building { get; set; }
        public string dest_floor{ get; set; }
        public string dest_mploc { get; set; }
        public string dep_cost_center { get; set; }
        public string status { get; set; }


    }
}
