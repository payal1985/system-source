using SSInventory.Business.Interfaces;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.InventoryFloors;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class InventoryFloorsService : IInventoryFloorsService
    {
        private readonly IInventoryFloorsRepository _inventoryFloorsRepository;
        public InventoryFloorsService(IInventoryFloorsRepository inventoryFloorsRepository)
        {
            _inventoryFloorsRepository = inventoryFloorsRepository;
        }

        public virtual async Task<List<InventoryFloorModel>> ReadAsync(int? clientId = null, List<int> ids = null)
            => await _inventoryFloorsRepository.ReadAsync(clientId, ids);

        public virtual async Task<InventoryFloorModel> FetchFirstAsync(int id)
        {
            var results = await ReadAsync(ids: new List<int> { id });
            return results.FirstOrDefault();
        }
    }
}
