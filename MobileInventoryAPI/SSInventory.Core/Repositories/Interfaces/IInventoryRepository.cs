using SSInventory.Core.Models;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryItem;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Share.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        IQueryable<Inventory> Filter(int? clientId = null, List<int> ids = null, int? itemTypeId = null, string searchImage = null,
            List<int> itemRowIds = null, DateTime? deviceDate = null, List<int?> submissionIds = null, string searchString = null);
        Task<IList<InventoryModel>> ReadAsync(int? clientId = null, List<int> ids = null, int? itemTypeId = null, List<int> itemRowIds = null,
            DateTime? deviceDate = null, List<int?> submissionIds = null, string searchString = null,
            bool includeItemType = false, bool includeInventoryImage = false);
        Task<InventoryModel> InsertAsync(InventoryModel model);
        Task<List<InventoryModel>> UpdateAsync(List<InventoryModel> models);
        Task<bool> UpdateInventoryCodeAndQRcode(int id, string code, string QRcode);
        Task<List<InventoryModel>> UpdateMainImages(List<Tuple<int, string>> mainImages);
        Task<SearchResultModel> SearchByImage(SearchModel input, string uploadedImagePath = null, bool usedAws = true);
        Task<List<DataItemType>> GetInventorySearch(SearchImageInfo input, string uploadedImagePath = null, bool usedAws = true, string baseUrl = "",
            bool includeTotalCountImages = false, bool includeQRcode = false, bool includeQuanlity = false, bool isSearch = false,
            bool forEdit = false, bool includeInventoryItem = true);
        Task<List<DataInventoryType>> SearchSimpleInventorySearch(SearchSimpleInventoryModel input);
        Task<List<SearchInventoryItemSubmissionModel>> GroupInventoryItemsAsync(int?[] submissionIds, int clientId);
        Task UpdateInventoryToHistory(InventoryHistoryViewModel oldValue, InventoryHistoryViewModel newValue,
    string api, string description);
        Task UpdateInventoriesToHistory(List<InventoryHistoryViewModel> oldValues, List<InventoryHistoryViewModel> newValues,
            string api, string description);
        Task<List<DataInventoryTypeWithFirstItemLocationInfo>> GetSimpleInventoriesWithFirstItemLocationInfo(SearchSimpleInventoryWithFirstItemLocationModel input);
    }
}