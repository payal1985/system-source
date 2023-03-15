using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Enums;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class InventoryImageRepository : Repository<InventoryImages>, IInventoryImageRepository
    {
        private readonly IMapper _mapper;

        public InventoryImageRepository(SSInventoryDbContext dbContext, IMapper mapper)
            : base(dbContext)
        {
            _mapper = mapper;
        }

        public virtual async Task<List<InventoryImageModel>> ReadAsync(List<string> photoIds = null, List<string> tempPhotos = null,
            List<int> ids = null, List<int> inventoryItemIds = null, List<int> inventoryIds = null)
        {
            var entities = GetAll()
                                   .Where(x => x.StatusId == InventoryImageStatus.Active)
                                   .WhereIf(photoIds?.Any() == true, x => photoIds.Contains(x.ImageName))
                                   .WhereIf(tempPhotos?.Any() == true, x => tempPhotos.Contains(x.TempPhotoName))
                                   .WhereIf(ids?.Any() == true, x => ids.Contains(x.InventoryImageId))
                                   .WhereIf(inventoryItemIds?.Any() == true, x => x.InventoryItemId.HasValue && inventoryItemIds.Contains(x.InventoryItemId.Value))
                                   .WhereIf(inventoryIds?.Any() == true, x => x.InventoryId != null && inventoryIds.Contains(x.InventoryId.Value));

            var result = await entities.ToListAsync();

            return _mapper.Map<List<InventoryImageModel>>(result);
        }

        public async Task<InventoryImageModel> GetByIdAsync(int inventoryImageId)
        {
            var entity = await GetAll().FirstOrDefaultAsync(x => x.InventoryImageId == inventoryImageId
            && x.StatusId == InventoryImageStatus.Active);
            return _mapper.Map<InventoryImageModel>(entity);
        }

        public virtual async Task<InventoryImageModel> InsertAsync(InventoryImageModel model)
        {
            var entity = await AddAsync(_mapper.Map<InventoryImages>(model));

            return _mapper.Map<InventoryImageModel>(entity);
        }
        public virtual async Task<List<InventoryImageModel>> InsertAsync(List<InventoryImageModel> inventoryImageModels)
        {
            var insertedInventoryImages = await AddAsync(_mapper.Map<List<InventoryImages>>(inventoryImageModels));
            // Insert representative images
            //await InsertRepresentativeImages(insertedInventoryImages.Where(x => x.InventoryId.HasValue).Select(x => x.InventoryId.Value).Distinct().ToList());

            return _mapper.Map<List<InventoryImageModel>>(insertedInventoryImages);
        }

        public virtual async Task<List<InventoryImageModel>> UpdateAsync(List<InventoryImageModel> images)
        {
            var entities = GetAll()
                .Where(x => x.StatusId == InventoryImageStatus.Active
                    && images.Select(x => x.InventoryImageId).ToList().Contains(x.InventoryImageId));
            if (entities?.Any() != true)
                return null;

            foreach (var image in images)
            {
                var imageEntity = entities.FirstOrDefault(x => x.InventoryImageId == image.InventoryImageId);
                if (imageEntity is null) continue;

                imageEntity.Width = image.Width;
                imageEntity.Height = image.Height;
                imageEntity.ImageName = image.ImageName;
                imageEntity.UpdateDateTime = DateTime.Now;
            }
            var data = await entities.ToListAsync();
            await UpdateAsync(data);

            return images;
        }

        public virtual async Task<InventoryImageModel> UpdateInventoryImageStatusAsync(int inventoryImageId, InventoryImageStatus statusId, int userId)
        {
            var inventoryImageEntity = await GetAll()
                .Where(x => x.StatusId == InventoryImageStatus.Active 
                        && x.InventoryImageId == inventoryImageId)
                .FirstOrDefaultAsync();
            if (inventoryImageEntity == null)
                return null;
            inventoryImageEntity.StatusId = statusId;
            inventoryImageEntity.UpdateDateTime = DateTime.UtcNow;
            inventoryImageEntity.UpdateId = userId;
            var result = await UpdateAsync(inventoryImageEntity);
            return _mapper.Map<InventoryImageModel>(result);
        }

        public virtual async Task<InventoryWithImageInfoModel> GetInventoryInfoByName(string photoName, int clientId)
        {
            var entity = await GetAll()
                .Where(x => x.StatusId == InventoryImageStatus.Active)
                .Where(x => EF.Functions.Like(x.TempPhotoName, $"%{photoName}%") && x.ClientId == clientId).FirstOrDefaultAsync();
            if (entity is null)
                return null;

            var counter = await GetAll()
                .Where(x => x.StatusId == InventoryImageStatus.Active)
                .CountAsync(x => x.InventoryItemId == entity.InventoryItemId && !string.IsNullOrWhiteSpace(x.ImageName));
            var inventoryId = (await _dbContext.InventoryItem.FirstOrDefaultAsync(x => x.InventoryItemId == entity.InventoryItemId)).InventoryId;
            var inventory = await _dbContext.Inventory.FirstOrDefaultAsync(x => x.InventoryId == inventoryId);

            return inventory is null
                ? null
                : new InventoryWithImageInfoModel
                {
                    InventoryId = inventoryId,
                    InventoryCode = inventory.ItemCode,
                    InventoryItemId = (int)entity.InventoryItemId,
                    NumberOfImages = counter,
                    InventoryImageId = entity.InventoryImageId
                };
        }

        public virtual async Task<bool> UpdateImage(int id, string photoName)
        {
            var entity = await GetAll()
                .FirstOrDefaultAsync(x => x.InventoryImageId == id && x.StatusId == InventoryImageStatus.Active);
            if (entity is null) return false;

            entity.ImageName = photoName;
            entity.UpdateDateTime = System.DateTime.Now;
            await UpdateAsync(entity);

            return true;
        }

        public virtual async Task InsertRepresentativeImages(List<int> inventoryIds)
        {
            var groupedEntities = GetAll()
                .Where(x => x.StatusId == InventoryImageStatus.Active 
                && x.InventoryId.HasValue && inventoryIds.Contains(x.InventoryId.Value))
                        .GroupBy(x => new { x.InventoryId, x.ConditionId })
                        .Select(x => new
                        {
                            x.Key.InventoryId,
                            x.Key.ConditionId,
                            TotalImages = x.Count()
                        }).ToList();

            foreach (var image in groupedEntities)
            {
                //if (image.TotalImages >= 5) continue;

                var cloneImage = await _dbContext.InventoryImages
                    .FirstOrDefaultAsync(x => 
                    x.StatusId == InventoryImageStatus.Active
                    && x.InventoryId == image.InventoryId
                    && x.ConditionId == image.ConditionId);
                var newImages = new List<InventoryImages>();
                for (int i = 0; i < image.TotalImages; i++)
                {
                    newImages.Add(new InventoryImages
                    {
                        InventoryId = image.InventoryId,
                        ConditionId = image.ConditionId,
                        InventoryItemId = null,
                        ImageGuid = cloneImage.ImageGuid,
                        ImageName = cloneImage.ImageName,
                        ImageUrl = cloneImage.ImageUrl,
                        ClientId = cloneImage.ClientId,
                        Width = cloneImage.Width,
                        Height = cloneImage.Height,
                        ItemTypeAutomationId = cloneImage.ItemTypeAutomationId,
                        ItemTypeAutomationOptionId = cloneImage.ItemTypeAutomationOptionId,
                        TempPhotoName = cloneImage.TempPhotoName,
                        CreateDateTime = DateTime.Now,
                        CreateId = cloneImage.CreateId,
                        UpdateId = cloneImage.UpdateId,
                        SubmissionDate = cloneImage.SubmissionDate,
                    });
                }
                await AddAsync(newImages);
            }
        }

        public async Task<Dictionary<int, int>> GetAmountOfRepresentativeImages(int inventoryId)
        {
            var amountDic = (await GetAll()
                .Where(x => x.InventoryId == inventoryId
            && x.StatusId == InventoryImageStatus.Active
            && x.InventoryItemId == null).ToListAsync()).GroupBy(x => x.ConditionId)
            .ToDictionary(x => x.Key.Value, x => x.Count());
            return amountDic;
        }
    }
}
