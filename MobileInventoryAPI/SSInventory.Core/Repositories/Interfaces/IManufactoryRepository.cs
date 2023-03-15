using SSInventory.Core.Models;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypeOptionLines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IManufactoryRepository : IRepository<ItemTypeOptionLines>
    {
        Task<List<ItemTypeOptionLineModel>> InsertAsync(List<CreateOrUpdateItemTypeOptionLineModel> model);
    }
}