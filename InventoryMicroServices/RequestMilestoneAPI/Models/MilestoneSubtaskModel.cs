using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestMilestoneAPI.Models
{
    public class MilestoneSubtaskModel
    {
        public int client_request_date_ID { get; set; }
        public int client_ID { get; set; }
        public int request_ID { get; set; }
        public int client_date_setting_ID { get; set; }
        public DateTime? begin_date { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? complete_date { get; set; }
        public string subtask_name { get; set; }
        public string assignee { get; set; }
        public string comments { get; set; }

        public string task_name { get; set; }
    }
}
