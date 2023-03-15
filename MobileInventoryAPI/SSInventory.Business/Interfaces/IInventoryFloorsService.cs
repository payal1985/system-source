using SSInventory.Share.Models.Dto.InventoryFloors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IInventoryFloorsService
    {
        Task<List<InventoryFloorModel>> ReadAsync(int? clientId = null, List<int> ids = null);
        Task<InventoryFloorModel> FetchFirstAsync(int id);
    }
}
