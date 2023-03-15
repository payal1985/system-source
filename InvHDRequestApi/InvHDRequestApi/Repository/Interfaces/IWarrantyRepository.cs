using InvHDRequestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.Repository.Interfaces
{
    public interface IWarrantyRepository
    {
        Task<int> InsertWarrantyRequest(string apicallurl, List<GenericModel> listWarrantyModels);

    }
}
