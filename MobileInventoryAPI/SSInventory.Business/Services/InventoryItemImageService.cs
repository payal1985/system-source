using SSInventory.Business.Interfaces;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Enums;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryImage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class InventoryImageService : IInventoryImageService
    {
        private readonly IInventoryImageRepository _inventoryImageRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public InventoryImageService(IInventoryImageRepository inventoryImageRepository,
            IInventoryItemRepository inventoryItemRepository)
        {
            _inventoryImageRepository = inventoryImageRepository;
            _inventoryItemRepository = inventoryItemRepository;
        }

        public virtual async Task<List<InventoryImageModel>> ReadAsync(List<string> photoIds = null, List<string> tempPhotos = null,
            List<int> ids = null, List<int> inventoryItemIds = null)
        {
            return await _inventoryImageRepository.ReadAsync(photoIds, tempPhotos, ids, inventoryItemIds);
        }

        public virtual async Task<InventoryImageModel> InsertAsync(InventoryImageModel model)
        {
            return await _inventoryImageRepository.InsertAsync(model);
        }

        public virtual async Task<List<InventoryImageModel>> InsertAsync(List<InventoryImageModel> inventoryImageModels)
            => await _inventoryImageRepository.InsertAsync(inventoryImageModels);

        public virtual async Task<List<InventoryImageModel>> UpdateAsync(List<InventoryImageModel> images)
        {
            return await _inventoryImageRepository.UpdateAsync(images);
        }

        public virtual async Task<InventoryWithImageInfoModel> GetInventoryInfoByName(string photoName, int clientId)
        {
            return await _inventoryImageRepository.GetInventoryInfoByName(photoName, clientId);
        }

        public virtual async Task<bool> UpdateImage(int id, string photoName)
        {
            return await _inventoryImageRepository.UpdateImage(id, photoName);
        }

        public async Task<Dictionary<int, int>> GetAmountOfRepresentativeImages(int inventoryId)
        {
            return await _inventoryImageRepository.GetAmountOfRepresentativeImages(inventoryId);
        }

        public async Task<InventoryImageModel> AddInventoryImageAsync(AddInventoryImageRequestModel requestModel, 
            string awsEndpoint, string fileName, Guid imageGuiId, string userId)
        {
            if (requestModel.InventoryItemId != null)
            {
                var inventoryItem = await _inventoryItemRepository.GetByIdAsync(requestModel.InventoryItemId.Value);
                requestModel.ConditionId = inventoryItem.ConditionId;
            }
            var inventoryItemImage = new InventoryImageModel
            {
                ClientId = requestModel.ClientId,
                ImageGuid = imageGuiId,
                ImageName = fileName,
                InventoryId = requestModel.InventoryId,
                InventoryItemId = requestModel.InventoryItemId,
                ConditionId = requestModel.ConditionId,
                CreateId = string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId),
                CreateDateTime = DateTime.UtcNow,
                StatusId = InventoryImageStatus.Active,
                ImageUrl = awsEndpoint,
                Width = requestModel.Width,
                Height = requestModel.Height
            };
            return await _inventoryImageRepository.InsertAsync(inventoryItemImage);
        }

        public async Task<InventoryImageModel> DeleteInventoryImageAsync(DeleteInventoryImageRequestModel requestModel, string userId)
        {
            var userIdParam = string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId);
            return await _inventoryImageRepository.UpdateInventoryImageStatusAsync(requestModel.InventoryImageId, InventoryImageStatus.InActive, userIdParam);
        }

        public async Task<InventoryImageModel> GetByIdAsync(int inventoryImageId)
        {
            return await _inventoryImageRepository.GetByIdAsync(inventoryImageId);
        }
    }
}
