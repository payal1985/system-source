using SchIntegrationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchIntegrationAPI.Repositories
{
    public interface ISchIntegrationRepository
    {
        //Task<int> AddRequestAsync(Request request);
        bool AddRequestAsync(List<SchApi> accruentApisList);


    }
}
