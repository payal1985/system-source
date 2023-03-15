using SSInventory.Core.Models;
using SSInventory.Share.Enums;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryImage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IInventoryImageRepository : IRepository<InventoryImages>
    {
        Task<List<InventoryImageModel>> ReadAsync(List<string> photoIds = null, List<string> tempPhotos = null,
            List<int> ids = null, List<int> inventoryItemIds = null, List<int> inventoryIds = null);
        Task<InventoryImageModel> InsertAsync(InventoryImageModel model);
        Task<List<InventoryImageModel>> InsertAsync(List<InventoryImageModel> inventoryImageModels);
        Task<List<InventoryImageModel>> UpdateAsync(List<InventoryImageModel> images);
        Task<InventoryWithImageInfoModel> GetInventoryInfoByName(string photoName, int clientId);
        Task<bool> UpdateImage(int id, string photoName);
        Task InsertRepresentativeImages(List<int> inventoryIds);
        Task<Dictionary<int, int>> GetAmountOfRepresentativeImages(int inventoryId);
        Task<InventoryImageModel> UpdateInventoryImageStatusAsync(int inventoryImageId, InventoryImageStatus statusId, int userId);
        Task<InventoryImageModel> GetByIdAsync(int inventoryImageId);
    }
}