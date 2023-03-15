using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestMilestoneAPI.Models
{
    public class MilestoneDatesModel
    {
        public string TaskName { get; set; }
        //public MilestoneSubtaskModel MilestoneSubtaskModels { get; set; }
        public List<MilestoneSubtaskModel> MilestoneSubtaskModels { get; set; }

        //public IList<MilestoneSubtaskModel> MilestoneSubtaskModels { get; set; } = new List<MilestoneSubtaskModel>();
    }
}
