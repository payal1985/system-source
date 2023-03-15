using InventoryApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IInventoryItemWarrantyRepository
    {
        Task<int> InsertWarrantyRequest(string apicallurl,List<InventoryItemWarrantyModel> listInventoryItemWarrantyModels);
        Task<bool> FixedWarrantyRequest(string isFixed, int requestId);
        //bool UpdateInventoryItemWarrantyRequestID(int requestId);

        Task<bool> UploadWarrantyAttachment(int requestid, IFormFileCollection files);
    }
}
