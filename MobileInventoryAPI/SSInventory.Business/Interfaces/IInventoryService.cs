using SSInventory.Core.Models;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryItem;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Share.Models.Search;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IInventoryService
    {
        Task<IList<InventoryModel>> ReadAsync(SearchInventoryModel input, bool includeInventoryImage = false);
        Task<InventoryModel> InsertAsync(InventoryModel model);
        Task<List<InventoryModel>> UpdateAsync(List<InventoryModel> models);
        Task UpdateInventoryCodeAndQRcodes(List<Tuple<int, string>> data);
        Task<List<InventoryModel>> UpdateMainImages(List<Tuple<int, string>> mainImages);
        Task<SearchResultModel> SearchByImage(SearchModel input, string uploadedImagePath = null, bool usedAws = true);
        Task<List<DataItemType>> GetInventorySearch(SearchImageInfo input, string uploadedImagePath = null, bool usedAws = true, string baseUrl = "",
            bool includeTotalCountImages = false, bool includeQRcode = false, bool includeQuanlity = false, bool isSearch = false,
            bool forEdit = false, bool includeInventoryItem = true);
        Task<List<DataItemType>> SearchInventory(SearchImageInfo input, string uploadedImagePath = null,
               bool usedAws = true, string baseUrl = "", bool includeTotalCountImages = false, bool includeQRcode = false,
               bool includeQuanlity = false, bool isSearch = false, bool forEdit = false, bool includeInventoryItem = true,
               bool includeConditionData = true, string searchAPIName = "");
        Task<List<DataInventoryType>> SearchSimpleInventorySearch(SearchSimpleInventoryRequestModel input);
        Task<List<SearchInventoryItemSubmissionModel>> GroupInventoryItemsAsync(int?[] submissionIds, int clientId);
        Task UpdateInventoryToHistory(InventoryHistoryViewModel oldValue, InventoryHistoryViewModel newValue,
    string api, string description);
        Task UpdateInventoriesToHistory(List<InventoryHistoryViewModel> oldValues, List<InventoryHistoryViewModel> newValues,
            string api, string description);
        Task<List<DataInventoryTypeWithFirstItemLocationInfo>> GetSimpleInventoriesWithFirstItemLocationInfo(SearchSimpleInventoryRequestModel input);
    }
}
