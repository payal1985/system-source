using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBModels
{
    [Table("client_settings")]
    public class ClientSettings
    {
        [Key]
        public int client_setting_id { get; set; }
        public int client_id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }
}
