using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RequestMilestoneAPI.DBModels
{
    [Table("client_date_settings")]

    public class ClientDateSettings
    {
        [Key]
        public int client_date_setting_ID {get; set;}
        public int? client_ID {get; set;}
        public string? date_name {get; set;}
        public bool default_fl {get; set;}
        public bool active_fl {get; set;}
        public int date_sort {get; set;}
        public int createid {get; set;}
        public DateTime createdate {get; set;}
        public int createprocess {get; set;}
        public int updateid {get; set;}
        public DateTime updatedate {get; set;}
        public int updateprocess {get; set;}
    }
}
