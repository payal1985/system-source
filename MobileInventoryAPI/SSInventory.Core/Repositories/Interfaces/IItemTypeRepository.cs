using SSInventory.Core.Models;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IItemTypeRepository : IRepository<ItemTypes>
    {
        Task<List<ItemTypeModel>> ReadAsync(int? clientId = null, List<int?> ids = null);
        Task<List<ItemTypeMappingModel>> GetItemTypeOptionSetAsync(int? clientId, int itemTypeId);
    }
}
