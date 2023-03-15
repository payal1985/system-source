using SSInventory.Core.Models;
using SSInventory.Share.Models.Dto.InventoryBuildings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IInventoryBuildingsRepository : IRepository<InventoryBuildings>
    {
        Task<List<InventoryBuildingModel>> ReadAsync(int? clientId = null, List<int> ids = null);
        //Task<InventoryBuildingModel> InsertAsync(CreateOrUpdateInventoryBuildingModel model);
    }
}
