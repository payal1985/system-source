using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBModels
{
    [Table("Order")]
    public class Order
    {
        [Key]
       public int order_id {get; set;}
       public string email {get; set;}
       public string project {get; set;}
       public string location {get; set;}
       public string room {get; set;}

       //[Column(TypeName = "DateTime2")] 
       public DateTime instdate {get; set;}
       public string comments {get; set;}
       public bool delivered {get; set;}
       public bool complete {get; set;}
       public string destb {get; set;}
       public string destf {get; set;}
       public DateTime instdater {get; set;}
       public string depcostcenter { get; set; }
    }
}
