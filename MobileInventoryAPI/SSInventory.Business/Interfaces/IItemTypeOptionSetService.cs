using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IItemTypeOptionSetService
    {
        Task<List<ItemTypeModel>> ReadAsync(ItemTypeSearchModel filter = null);
        Task<CreateOrUpdateItemTypeModel> SaveAsync(CreateOrUpdateItemTypeModel input);
        Task<bool> DeleteAsync(List<int> ids);
    }
}
