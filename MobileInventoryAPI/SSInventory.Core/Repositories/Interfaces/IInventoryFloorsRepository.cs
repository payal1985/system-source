using SSInventory.Core.Models;
using SSInventory.Share.Models.Dto.InventoryFloors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IInventoryFloorsRepository : IRepository<InventoryFloors>
    {
        Task<List<InventoryFloorModel>> ReadAsync(int? clientId = null, List<int> ids = null);
    }
}
