using SSInventory.Business.Interfaces;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class ItemTypeOptionSetService : IItemTypeOptionSetService
    {
        private readonly IItemTypeOptionSetRepository _itemTypeOptionSetRepository;
        public ItemTypeOptionSetService(IItemTypeOptionSetRepository itemTypeOptionSetRepository)
        {
            _itemTypeOptionSetRepository = itemTypeOptionSetRepository;
        }

        public virtual async Task<List<ItemTypeModel>> ReadAsync(ItemTypeSearchModel filter = null)
        {
            filter = ValidateFilter(filter);
            return await _itemTypeOptionSetRepository.ReadAsync(keyword: filter.Keyword);
        }

        public async Task<CreateOrUpdateItemTypeModel> SaveAsync(CreateOrUpdateItemTypeModel input)
        {
            return input.ItemTypeId > 0 ? await UpdateAsync(input) : await CreateAsync(input);
        }

        public async Task<bool> DeleteAsync(List<int> ids) => await _itemTypeOptionSetRepository.DeleteAsync(ids);

        #region Private Methods

        private async Task<CreateOrUpdateItemTypeModel> CreateAsync(CreateOrUpdateItemTypeModel model)
            => await _itemTypeOptionSetRepository.CreateAsync(model);

        private async Task<CreateOrUpdateItemTypeModel> UpdateAsync(CreateOrUpdateItemTypeModel model)
            => await _itemTypeOptionSetRepository.UpdateAsync(model);

        private ItemTypeSearchModel ValidateFilter(ItemTypeSearchModel filter = null)
        {
            if (!string.IsNullOrEmpty(filter?.Keyword))
            {
                filter.Keyword = filter.Keyword.Trim();
            }

            return filter;
        }

        #endregion
    }
}
