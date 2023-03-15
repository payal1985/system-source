using SSInventory.Core.Models;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.InventoryItem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IInventoryItemRepository : IRepository<InventoryItem>
    {
        Task<List<InventoryItemModel>> ReadAsync(List<int> ids = null, int? clientId = null,
            List<int> buildingIds = null, List<int> floorIds = null, string room = null, List<int> inventoryIds = null, bool includeItemImages = false);
        Task<List<InventoryItemModel>> ReadAndPagingAsync(List<int> ids = null, int? clientId = null,
            List<int> buildingIds = null, List<int> floorIds = null, string room = null, List<int> inventoryIds = null,
            bool includeItemImages = false, int? conditionId = null, int currentPage = 1, int itemsPerPage = 10);
        Task<InventoryItemModel> InsertAsync(InventoryItemModel model, bool isIndividual = false);
        Task<InventoryItemModel> UpdateEntityAsync(InventoryItemModel model);
        Task<InventoryItemModel> UpdateAsync(InventoryItemModel model);
        Task<bool> UpdateBarcode(int id, string QRcode);
        Task<ResponseModel> UpdateLocations(UpdateLocationInputModel input);
        Task<InventoryItemModel> GetByIdAsync(int inventoryItemId);
        Task<List<int>> GetInventoryIdsAsync(int clientId, int conditionId, string room);
        Task<ResponseModel> UpdateLocations(List<OrderItem> orderItems, int clientId);
    }
}