using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBModels
{
    [Table("Client")]

    public class Client
    {
        [Key]
        public int client_id {get; set;}
        public int ssidb_client_id {get; set;}
        public int teamdesign_cust_no {get; set;}
        public string client_name {get; set;}
        public bool has_inventory { get; set; }
    }
}
