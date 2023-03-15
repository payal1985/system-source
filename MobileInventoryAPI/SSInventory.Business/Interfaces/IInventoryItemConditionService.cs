using SSInventory.Share.Models.Dto.InventoryItemConditions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IInventoryItemConditionService
    {
        Task<List<InventoryItemConditionModel>> ReadAsync(List<int> ids = null);
    }
}
