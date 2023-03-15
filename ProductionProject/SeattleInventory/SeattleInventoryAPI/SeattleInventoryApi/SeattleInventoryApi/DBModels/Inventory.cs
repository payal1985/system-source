using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBModels
{
    [Table("Inventory")]

    public class Inventory
    {
        [Key]
        public int inventory_id { get; set; }
        public string item_code { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string manuf { get; set; }
        public string fabric { get; set; }
        public string finish { get; set; }
        public string size { get; set; }
        public string notes { get; set; }
        public string other_notes { get; set; }
        public DateTime createdate { get; set; }
        public DateTime updatedate { get; set; }
        public string rfidcode { get; set; }
        public string barcode { get; set; }
        public string ownership { get; set; }
        public string part_number { get; set; }
        public string additionaldescription { get; set; }
        public decimal diameter { get; set; }
        public decimal height { get; set; }
        public decimal width { get; set; }
        public decimal depth { get; set; }
        public string top { get; set; }
        public string edge { get; set; }
        public string Base { get; set; }
        public string frame { get; set; }
        public string seat { get; set; }
        public string back { get; set; }
        public string seat_height { get; set; }
        public string createby { get; set; }
        public string updateby { get; set; }
        public DateTime? devicedate { get; set; }
        public DateTime? date { get; set; }

        public int client_id {get;set;}
        public List<InventoryItem> InventoryItems { get; set; }
    }
}
