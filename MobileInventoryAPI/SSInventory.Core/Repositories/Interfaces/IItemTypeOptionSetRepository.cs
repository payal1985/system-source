using SSInventory.Core.Models;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IItemTypeOptionSetRepository : IRepository<ItemTypes>
    {
        Task<List<ItemTypeModel>> ReadAsync(string keyword = "");
        Task<CreateOrUpdateItemTypeModel> CreateAsync(CreateOrUpdateItemTypeModel model);
        Task<CreateOrUpdateItemTypeModel> UpdateAsync(CreateOrUpdateItemTypeModel model);
        Task<bool> DeleteAsync(List<int> ids);
    }
}

