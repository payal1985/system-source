using SSInventory.Share.Models.Dto.InventoryItem;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Search;

namespace SSInventory.Business.Interfaces
{
    public interface IInventoryItemService
    {
        Task<List<InventoryItemModel>> ReadAsync(InventoryItemFilterModel filter);
        Task<InventoryItemModel> InsertAsync(InventoryItemModel model, bool isIndividual = false);
        Task<InventoryItemModel> UpdateEntityAsync(InventoryItemModel model);
        Task<InventoryItemModel> UpdateAsync(InventoryItemModel model);
        Task UpdateBarcode(List<Tuple<int, string>> data);
        Task<ResponseModel> UpdateLocations(UpdateLocationInputModel input);
        Task<List<InventoryItemSearchResultModel>> SearchByConditions(InventoryItemFilterModel filter);
        Task<List<SimpleInventoryItemSearchResultModel>> SearchSimpleInventoryItems(SearchSimpleInventoryItem filter);
        Task<InventoryItemModel> GetByIdAsync(int inventoryItemId);
    }
}
