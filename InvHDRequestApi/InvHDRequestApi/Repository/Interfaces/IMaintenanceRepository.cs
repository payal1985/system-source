using InvHDRequestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.Repository.Interfaces
{
    public interface IMaintenanceRepository
    {
        Task<int> InsertMaintenanceRequest(string apicallurl, List<GenericModel> listWarrantyModels);

    }
}
