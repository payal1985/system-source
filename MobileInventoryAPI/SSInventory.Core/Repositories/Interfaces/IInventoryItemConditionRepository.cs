using SSInventory.Core.Models;
using SSInventory.Share.Models.Dto.InventoryItemConditions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IInventoryItemConditionRepository : IRepository<InventoryItemCondition>
    {
        Task<List<InventoryItemConditionModel>> ReadAsync(List<int> ids = null);
    }
}
