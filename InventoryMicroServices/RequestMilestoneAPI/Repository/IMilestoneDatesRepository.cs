using RequestMilestoneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestMilestoneAPI.Repository
{
    public interface IMilestoneDatesRepository
    {
        Task<List<MilestoneDatesModel>> GetMilestoneDates(int request_id);

        Task<bool> InsertMilestoneDates(List<MilestoneDatesModel> model);
    }
}
