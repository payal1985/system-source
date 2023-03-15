using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.InventoryDBModels
{
    [Table("InventoryOwners")]
    public class InventoryOwners
    {
        [Key]
        public int InventoryOwnerID {get;set;}
        public int ClientID {get;set;}
        public string OwnerName {get;set;}
        public DateTime CreateDateTime {get;set;}
        public int CreateID {get;set;}
        public DateTime? UpdateDateTime {get;set;}
        public int? UpdateID {get;set;}
    }
}
