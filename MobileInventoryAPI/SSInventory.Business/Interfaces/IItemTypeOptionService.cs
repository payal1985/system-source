using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypeOption;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IItemTypeOptionService
    {
        Task<List<ItemTypeOptionModel>> ReadAsync(int? clientId = null, List<int> ids = null, List<int> itemTypeIds = null, List<string> itemTypeOptionCodes = null);
        Task CreateAsync(List<CreateOrUpdateItemTypeOptionModel> itemTypeOptions);
    }
}
