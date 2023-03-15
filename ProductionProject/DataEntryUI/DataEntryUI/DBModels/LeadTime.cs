using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataEntryUI.DBModels
{
    [Table("LeadTime")]

    public class LeadTime
    {
        public int lead_time_ID { get; set; }
        public string lead_time_description { get; set; }
        public int day_lower { get; set; }
        public int day_upper { get; set; }
        public string createLogin { get; set; }
        public DateTime createDate { get; set; }
        public string updateLogin { get; set; }
        public DateTime updateDate { get; set; }
    }
}
