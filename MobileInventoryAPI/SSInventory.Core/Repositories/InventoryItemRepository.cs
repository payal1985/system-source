using AutoMapper;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.InventoryItem;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.InventoryImage;
//using Hangfire;
using Microsoft.AspNetCore.Http;
using System;

namespace SSInventory.Core.Repositories
{
    public class InventoryItemRepository : Repository<InventoryItem>, IInventoryItemRepository
    {
        private readonly IMapper _mapper;
        private readonly IInventoryImageRepository _inventoryImageRepository;
        private readonly IHistoryTableRepository _historyTableRepository;
        private readonly string _apiName;

        public InventoryItemRepository(SSInventoryDbContext dbContext,
            IMapper mapper,
            IInventoryImageRepository inventoryItemImageRepository,
            IHistoryTableRepository historyTableRepository,
            IHttpContextAccessor httpContextAccessor)
            : base(dbContext)
        {
            _mapper = mapper;
            _inventoryImageRepository = inventoryItemImageRepository;
            _historyTableRepository = historyTableRepository;
            _apiName = $"{httpContextAccessor.HttpContext?.Request?.Scheme}://{httpContextAccessor.HttpContext?.Request?.Host}{httpContextAccessor.HttpContext?.Request?.PathBase}{httpContextAccessor.HttpContext?.Request?.Path}";
        }

        public async Task<List<InventoryItemModel>> ReadAsync(List<int> ids = null, int? clientId = null,
            List<int> buildingIds = null, List<int> floorIds = null, string room = null, List<int> inventoryIds = null,
            bool includeItemImages = false)
        {
            var entities = GetAll().WhereIf(ids?.Any() == true, x => ids.Contains(x.InventoryItemId))
                                   .WhereIf(clientId.HasValue, x => x.ClientId == clientId)
                                   .WhereIf(buildingIds?.Any() == true, x => buildingIds.Contains(x.InventoryBuildingId))
                                   .WhereIf(floorIds?.Any() == true, x => floorIds.Contains(x.InventoryFloorId))
                                   .WhereIf(!string.IsNullOrWhiteSpace(room) && room != "0", x => x.Room.Contains(room))
                                   .WhereIf(inventoryIds?.Any() == true, x => inventoryIds.Contains(x.InventoryId))
                                   .AsNoTracking();

            var inventoryItems = await entities.ToListAsync();

            var result = _mapper.Map<List<InventoryItemModel>>(inventoryItems);

            if (includeItemImages)
            {
                var inventoryItemIds = inventoryItems.Select(x => x.InventoryItemId).ToList();
                var inventoryItemImages = await _inventoryImageRepository.GetAll()
                    .Where(x => x.InventoryItemId.HasValue && inventoryItemIds.Contains(x.InventoryItemId.Value))
                    .ToListAsync();
                foreach (var item in result)
                {
                    item.InventoryImages = _mapper.Map<List<InventoryImageModel>>(inventoryItemImages.Where(x => x.InventoryItemId == item.InventoryItemId).ToList());
                }
            }

            return result;
        }

        public async Task<List<InventoryItemModel>> ReadAndPagingAsync(List<int> ids = null, int? clientId = null,
            List<int> buildingIds = null, List<int> floorIds = null, string room = null, List<int> inventoryIds = null,
            bool includeItemImages = false, int? conditionId = null,int currentPage = 1, int itemsPerPage = 10)
        {
            var entities = GetAll().WhereIf(ids?.Any() == true, x => ids.Contains(x.InventoryItemId))
                                   .WhereIf(clientId.HasValue, x => x.ClientId == clientId)
                                   .WhereIf(buildingIds?.Any() == true, x => buildingIds.Contains(x.InventoryBuildingId))
                                   .WhereIf(floorIds?.Any() == true, x => floorIds.Contains(x.InventoryFloorId))
                                   .WhereIf(!string.IsNullOrWhiteSpace(room) && room != "0", x => x.Room.Contains(room))
                                   .WhereIf(inventoryIds?.Any() == true, x => inventoryIds.Contains(x.InventoryId))
                                   .WhereIf(conditionId.HasValue && conditionId != 0, x => x.ConditionId == conditionId)
                                   .OrderByDescending(x => x.CreateDateTime)
                                   .Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage)
                                   .AsNoTracking();

            var inventoryItems = await entities.ToListAsync();

            var result = _mapper.Map<List<InventoryItemModel>>(inventoryItems);

            if (includeItemImages)
            {
                var inventoryItemIds = inventoryItems.Select(x => x.InventoryItemId).ToList();
                var inventoryItemImages = await _inventoryImageRepository.GetAll()
                    .Where(x => x.InventoryItemId.HasValue && inventoryItemIds.Contains(x.InventoryItemId.Value))
                    .ToListAsync();
                foreach (var item in result)
                {
                    item.InventoryImages = _mapper.Map<List<InventoryImageModel>>(inventoryItemImages.Where(x => x.InventoryItemId == item.InventoryItemId).ToList());
                }
            }

            return result;
        }

        public async Task<InventoryItemModel> InsertAsync(InventoryItemModel model, bool isIndividual = false)
        {
            var mappedModel = _mapper.Map<InventoryItem>(model);
            var entity = await AddAsync(mappedModel);
            await InsertInventoryItemToHistory(_mapper.Map<InventoryItemHistoryViewModel>(entity), _apiName, null);

            return _mapper.Map<InventoryItemModel>(entity);
        }

        public async Task<InventoryItemModel> UpdateEntityAsync(InventoryItemModel model)
        {
            var entity = await GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.InventoryItemId == model.InventoryItemId);
            if (entity is null) return null;
            var oldValue = _mapper.Map<InventoryItemHistoryViewModel>(entity);

            var updatedEntity = _mapper.Map<InventoryItem>(model);
            var newValue = _mapper.Map<InventoryItemHistoryViewModel>(updatedEntity);
            await UpdateAsync(updatedEntity);
            await UpdateInventoryItemToHistory(oldValue, newValue, _apiName, null);
            return _mapper.Map<InventoryItemModel>(updatedEntity);
        }

        public async Task<InventoryItemModel> UpdateAsync(InventoryItemModel model)
        {
            var entity = await GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.InventoryItemId == model.InventoryItemId);
            if (entity is null) return null;

            entity.Gpslocation = model.GpsLocation;
            entity.DamageNotes = !string.IsNullOrWhiteSpace(model.DamageNotes) ? model.DamageNotes : "";
            entity.StatusId = model.StatusId;
            entity.InventoryBuildingId = model.InventoryBuildingId;
            entity.InventoryFloorId = model.InventoryFloorId;
            entity.Room = model.Room;
            await UpdateAsync(entity);
            return _mapper.Map<InventoryItemModel>(entity);
        }

        public async Task<bool> UpdateBarcode(int id, string barcode)
        {
            var entity = await GetAll().FirstOrDefaultAsync(x => x.InventoryItemId == id);
            if (entity is null) return false;
            var oldValue = _mapper.Map<InventoryItemHistoryViewModel>(entity);
            entity.Barcode = barcode;
            await UpdateAsync(entity);
            var newValue = _mapper.Map<InventoryItemHistoryViewModel>(entity);
            await UpdateInventoryItemToHistory(oldValue, newValue, _apiName, null);
            return true;
        }

        public virtual async Task<ResponseModel> UpdateLocations(UpdateLocationInputModel input)
        {
            try
            {
                var entities = GetAll().Where(x => input.InventoryItemId.Contains(x.InventoryItemId) && x.ClientId == input.ClientId);

                var inventoryItems = await entities.ToListAsync();
                if (inventoryItems.Count < 1)
                    return ResponseModel.Failed("No data found");
                var oldValues = new List<InventoryItemHistoryViewModel>();
                var newValues = new List<InventoryItemHistoryViewModel>();
                foreach (var item in inventoryItems)
                {
                    var oldValue = _mapper.Map<InventoryItemHistoryViewModel>(item);
                    if (input.Locations.Building != null)
                    {
                        //item.Building = input.Locations.Building.Name;
                        item.InventoryBuildingId = input.Locations.Building.Id;
                    }

                    if (input.Locations.Floor != null)
                    {
                        //item.Floor = input.Locations.Floor.Name;
                        item.InventoryFloorId = input.Locations.Floor.Id;
                    }

                    if (input.Locations.Gps != null)
                    {
                        item.Gpslocation = $"{input.Locations.Gps.Longitude}/{input.Locations.Gps.Latitude}";
                    }

                    item.Room = input.Locations.RoomNumber;
                    var newValue = _mapper.Map<InventoryItemHistoryViewModel>(item);
                    oldValues.Add(oldValue);
                    newValues.Add(newValue);
                }

                await UpdateAsync(inventoryItems);

                await UpdateInventoriesToHistory(oldValues, newValues, _apiName, null);
                return ResponseModel.Successed("Updated locations successfully");
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<InventoryItemModel> GetByIdAsync(int inventoryItemId)
        {
            var entity = await GetAll().FirstOrDefaultAsync(x => x.InventoryItemId == inventoryItemId);
            return _mapper.Map<InventoryItemModel>(entity);
        }

        public async Task<List<int>> GetInventoryIdsAsync(int clientId, int conditionId, string room)
        {
            var inventoryIds = await GetAll().Where(x => x.ClientId == clientId)
                .WhereIf(conditionId != 0, x => x.ConditionId == conditionId)
                .WhereIf(!string.IsNullOrWhiteSpace(room) && room != "0", x=> x.Room.Contains(room))
                .Select(x => x.InventoryId).Distinct().ToListAsync();
            return inventoryIds;
        }

        public virtual async Task<ResponseModel> UpdateLocations(List<OrderItem> orderItems, int clientId)
        {
            try
            {
                var inventoryItemIds = orderItems.Select(x => x.InventoryItemID).ToList();
                var entities = GetAll().Where(x => inventoryItemIds.Contains(x.InventoryItemId) && x.ClientId == clientId);

                var inventoryItems = await entities.ToListAsync();
                if (inventoryItemIds.Count < 1)
                    return ResponseModel.Failed("No data found");
                foreach (var item in inventoryItems)
                {
                    var orderItem = orderItems.Where(x => x.InventoryItemID == item.InventoryItemId)?.FirstOrDefault();
                    if(orderItem != null)
                    {
                        item.InventoryBuildingId = (int)orderItem.DestBuildingID;
                        item.InventoryFloorId = (int)orderItem.DestFloorID;
                        item.Room = orderItem?.Room;
                    }    
                }

                await UpdateAsync(inventoryItems);

                return ResponseModel.Successed("Updated locations successfully");
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        #region Hang fire
        public async Task UpdateInventoriesToHistory(List<InventoryItemHistoryViewModel> oldValues, List<InventoryItemHistoryViewModel> newValues,
            string api, string description)
        {
            for (int i = 0; i < oldValues.Count(); i++)
            {
                await UpdateInventoryItemToHistory(oldValues[i], newValues[i], api, description);
            }
        }
        public async Task InsertInventoryItemToHistory(InventoryItemHistoryViewModel inventoryItem, string api, string description = null)
        {
            //await _historyTableRepository.InsertObject(inventoryItem, "InventoryItem", inventoryItem.InventoryItemId, api, inventoryItem.CreateId, description);
            await _historyTableRepository.InsertToInventoryHistory(inventoryItem, "InventoryItem", inventoryItem.InventoryItemId, api, inventoryItem.CreateId, description);
        }
        public async Task UpdateInventoryItemToHistory(InventoryItemHistoryViewModel oldValue, InventoryItemHistoryViewModel newValue,
            string api, string description = null)
        {
            //await _historyTableRepository.UpdateObject(oldValue, newValue, "InventoryItem", newValue.InventoryItemId, api, newValue.UpdateId, description);
            await _historyTableRepository.UpdateToInventoryHistory(oldValue, newValue, "InventoryItem", newValue.InventoryItemId, api, newValue.UpdateId, description);
        }
        #endregion
    }
}
