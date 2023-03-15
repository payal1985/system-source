using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBModels
{
    [Table("email_queue")]
    public class EmailQueue
    {
      [Key]
       public int email_id {get; set;}
       public int client_id {get; set;}
       public int user_id {get; set;}
       public int stype {get; set;}
       public int? skey {get; set;}
       public DateTime scheduled_time {get; set;}
       public int status  {get; set;}
       public string em_subject {get; set;}
       public string em_body {get; set;}
       public string em_to {get; set;}
       public string? em_cc   {get; set;}
       public string? sdata {get; set;}
       public string? tip {get; set;}
       public int createid {get; set;}
       public DateTime createdate {get; set;}
       public int createprocess   {get; set;}
       public int updateid {get; set;}
       public DateTime updatedate {get; set;}
       public int updateprocess   {get; set;}
       public int? skey2 {get; set;}
    }
}
