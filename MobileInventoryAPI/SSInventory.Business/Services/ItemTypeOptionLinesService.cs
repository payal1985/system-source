using SSInventory.Business.Interfaces;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.ItemTypeOptionLines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class ItemTypeOptionLinesService : IItemTypeOptionLinesService
    {
        private readonly IItemTypeOptionLinesRepository _itemTypeOptionLinesRepository;
        public ItemTypeOptionLinesService(IItemTypeOptionLinesRepository itemTypeOptionLinesRepository)
        {
            _itemTypeOptionLinesRepository = itemTypeOptionLinesRepository;
        }

        public virtual async Task<List<ItemTypeOptionLinesModel>> ReadAsync(List<int> itemTypeOptionIds = null)
            => await _itemTypeOptionLinesRepository.ReadAsync(itemTypeOptionIds: itemTypeOptionIds);

        public virtual async Task InsertAsync(List<CreateOrUpdateItemTypeOptionLineModel> model)
            => await _itemTypeOptionLinesRepository.InsertAsync(model);
    }
}
