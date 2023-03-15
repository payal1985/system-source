using SSInventory.Business.Interfaces;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.InventoryItemConditions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class InventoryItemConditionService : IInventoryItemConditionService
    {
        private readonly IInventoryItemConditionRepository _InventoryItemConditionRepository;

        public InventoryItemConditionService(IInventoryItemConditionRepository InventoryItemConditionRepository)
        {
            _InventoryItemConditionRepository = InventoryItemConditionRepository;
        }

        public async Task<List<InventoryItemConditionModel>> ReadAsync(List<int> ids = null)
            => await _InventoryItemConditionRepository.ReadAsync(ids);
    }
}
