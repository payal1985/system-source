using SSInventory.Core.Models;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.Manufactory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IManufacturerRepository : IRepository<Manufacturers>
    {
        Task<List<ManufacturerModel>> ReadAsync(List<int> ids = null, string manufacturerName = "", string vendorNum = "", List<string> pmTypes = null);
        Task<ResponseModel> CreateAsync(CreateOrEditManufacturerModel model);
    }
}
