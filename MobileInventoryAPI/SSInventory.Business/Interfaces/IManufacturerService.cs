using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.Manufactory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IManufacturerService
    {
        Task<List<ManufacturerModel>> ReadAsync(List<int> ids = null, string manufacturerName = "", string vendorNum = "", List<string> pmTypes = null);
        Task<ResponseModel> CreateAsync(CreateOrEditManufacturerModel model);
    }
}
