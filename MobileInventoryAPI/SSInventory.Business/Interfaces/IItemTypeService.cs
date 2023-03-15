using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{

    public interface IItemTypeService
    {
        Task<List<ItemTypeModel>> ReadAsync(int? clientId = null);
        Task<List<ItemTypeMappingModel>> GetItemTypeOptionSetAsync(int? clientId, int itemTypeId);
    }
}

