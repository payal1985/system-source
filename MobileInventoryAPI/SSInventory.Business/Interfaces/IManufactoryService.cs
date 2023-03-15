using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypeOptionLines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IManufactoryService
    {
        Task<List<ItemTypeOptionLineModel>> InsertAsync(List<CreateOrUpdateItemTypeOptionLineModel> model);
    }
}
