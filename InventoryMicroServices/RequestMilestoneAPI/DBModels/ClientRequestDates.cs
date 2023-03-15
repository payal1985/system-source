using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RequestMilestoneAPI.DBModels
{
    [Table("client_request_dates")]

    public class ClientRequestDates
    {
        [Key]
       public int client_request_date_ID{get; set;}
       public int client_ID{get; set;}
       public int request_ID{get; set;}
       public int client_date_setting_ID{get; set;}
       public DateTime? begin_date{get; set;}
       public DateTime? end_date{get; set;}
       public int createid{get; set;}
       public DateTime createdate{get; set;}
       public int createprocess{get; set;}
       public int updateid{get; set;}
       public DateTime updatedate{get; set;}
       public int updateprocess{get; set;}
       public DateTime? complete_date{get; set;}
       public string? subtask_name{get; set;}
       public string? assignee{get; set;}
       public string? comments{get; set;}
    }
}
