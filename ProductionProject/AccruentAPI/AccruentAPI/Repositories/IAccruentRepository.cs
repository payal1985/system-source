using AccruentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccruentAPI.Repositories
{
    public interface IAccruentRepository
    {
        //Task<int> AddRequestAsync(Request request);
        bool AddRequestAsync(List<AccruentApi> accruentApisList);


    }
}
