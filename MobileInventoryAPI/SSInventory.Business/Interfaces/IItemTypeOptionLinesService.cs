using SSInventory.Share.Models.Dto.ItemTypeOptionLines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IItemTypeOptionLinesService
    {
        Task<List<ItemTypeOptionLinesModel>> ReadAsync(List<int> itemTypeOptionIds = null);
        Task InsertAsync(List<CreateOrUpdateItemTypeOptionLineModel> model);
    }
}
