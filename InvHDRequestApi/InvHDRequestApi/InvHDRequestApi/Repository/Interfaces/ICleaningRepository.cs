using InvHDRequestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.Repository.Interfaces
{
    public interface ICleaningRepository
    {
        Task<int> InsertCleaningRequest(string apicallurl, List<GenericModel> listWarrantyModels);

    }
}
