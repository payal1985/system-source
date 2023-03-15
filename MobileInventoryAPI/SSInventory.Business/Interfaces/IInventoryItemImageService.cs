using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryImage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IInventoryImageService
    {
        Task<List<InventoryImageModel>> ReadAsync(List<string> photoIds = null, List<string> tempPhotos = null,
            List<int> ids = null, List<int> inventoryItemIds = null);
        Task<InventoryImageModel> InsertAsync(InventoryImageModel model);
        Task<List<InventoryImageModel>> InsertAsync(List<InventoryImageModel> inventoryImageModels);
        Task<List<InventoryImageModel>> UpdateAsync(List<InventoryImageModel> images);
        Task<InventoryWithImageInfoModel> GetInventoryInfoByName(string photoName, int clientId);
        Task<Dictionary<int, int>> GetAmountOfRepresentativeImages(int inventoryId);
        Task<bool> UpdateImage(int id, string photoName);
        Task<InventoryImageModel> AddInventoryImageAsync(AddInventoryImageRequestModel requestModel,
            string awsEndpoint, string fileName, Guid imageGuiId, string userId);
        Task<InventoryImageModel> DeleteInventoryImageAsync(DeleteInventoryImageRequestModel requestModel, string userId);
        Task<InventoryImageModel> GetByIdAsync(int inventoryImageId);
    }
}
