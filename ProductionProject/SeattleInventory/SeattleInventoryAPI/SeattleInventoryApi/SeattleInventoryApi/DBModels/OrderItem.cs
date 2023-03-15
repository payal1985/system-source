using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBModels
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
       public int order_item_id {get; set;}
       public int order_id {get; set;}
       public int inv_id {get; set;}
       public int qty {get; set;}
       public int inv_item_id {get; set;}
       public string ic {get; set;}
       public bool completed {get; set;}
    }
}
