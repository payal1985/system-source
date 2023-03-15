using SSInventory.Business.Interfaces;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypeOption;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class ItemTypeOptionService : IItemTypeOptionService
    {
        private readonly IItemTypeOptionRepository _itemTypeOptionRepository;
        public ItemTypeOptionService(IItemTypeOptionRepository itemTypeOptionRepository)
        {
            _itemTypeOptionRepository = itemTypeOptionRepository;
        }

        public async Task<List<ItemTypeOptionModel>> ReadAsync(int? clientId = null, List<int> ids = null, List<int> itemTypeIds = null, List<string> itemTypeOptionCodes = null)
            => await _itemTypeOptionRepository.ReadAsync(clientId: clientId, ids: ids, itemTypeIds: itemTypeIds, itemTypeOptionCodes: itemTypeOptionCodes);

        public async Task CreateAsync(List<CreateOrUpdateItemTypeOptionModel> itemTypeOptions)
            => await _itemTypeOptionRepository.CreateAsync(itemTypeOptions);
    }
}
