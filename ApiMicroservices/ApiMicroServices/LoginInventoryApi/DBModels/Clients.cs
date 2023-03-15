using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.DBModels
{
    [Table("clients")]

    public class Clients
    {
        [Key]
        public int client_id { get; set; }
        public int? ssclient_id { get; set; }
        public string path { get; set; }
        public string name { get; set; }
        public string? catalog_title { get; set; }
        public bool active { get; set; }
        public int createid { get; set; }
        public DateTime createdate { get; set; }
        public int createprocess { get; set; }
        public int updateid { get; set; }
        public DateTime updatedate { get; set; }
        public int updateprocess { get; set; }
        public int client_inventory_display_column { get; set; }
        public bool inventory_app { get; set; }
    }
}
