using SSInventory.Business.Interfaces;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class ItemTypeService : IItemTypeService
    {

        private readonly IItemTypeRepository _itemTypeRepository;

        public ItemTypeService(IItemTypeRepository itemTypeRepository)
        {
            _itemTypeRepository = itemTypeRepository;
        }

        public virtual async Task<List<ItemTypeModel>> ReadAsync(int? clientId = null)
            => await _itemTypeRepository.ReadAsync(clientId);

        public async Task<List<ItemTypeMappingModel>> GetItemTypeOptionSetAsync(int? clientId, int itemTypeId)
        {
            return await _itemTypeRepository.GetItemTypeOptionSetAsync(clientId, itemTypeId);
        }
    }
}
