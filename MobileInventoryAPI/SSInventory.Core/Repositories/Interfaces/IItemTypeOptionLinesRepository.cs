using SSInventory.Core.Models;
using SSInventory.Share.Models.Dto.ItemTypeOptionLines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IItemTypeOptionLinesRepository : IRepository<ItemTypeOptionLines>
    {
        Task<List<ItemTypeOptionLinesModel>> ReadAsync(List<int> itemTypeOptionIds = null);
        Task InsertAsync(List<CreateOrUpdateItemTypeOptionLineModel> model);
    }
}
