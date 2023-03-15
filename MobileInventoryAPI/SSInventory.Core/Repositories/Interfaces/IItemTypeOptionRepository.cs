using SSInventory.Core.Models;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypeOption;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IItemTypeOptionRepository : IRepository<ItemTypeOptions>
    {

        Task<List<ItemTypeOptionModel>> ReadAsync(int? clientId = null, List<int> ids = null, List<int> itemTypeIds = null, List<string> itemTypeOptionCodes = null);
        Task CreateAsync(List<CreateOrUpdateItemTypeOptionModel> itemTypeOptions);
    }
}
