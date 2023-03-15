using SSInventory.Business.Interfaces;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.InventoryItem;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.Inventory;
using System.Linq;
using SSInventory.Share.Models.Search;
using SSInventory.Share.Models.Dto.InventoryBuildings;
using SSInventory.Share.Models.Dto.InventoryFloors;
using SSInventory.Share.Models.Dto.InventoryItemConditions;
using SSInventory.Core.Services.External;

namespace SSInventory.Business.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IItemTypeRepository _itemTypeRepository;
        private readonly IInventoryBuildingsRepository _inventoryBuildingsRepository;
        private readonly IInventoryFloorsRepository _inventoryFloorsRepository;
        private readonly IInventoryItemConditionRepository _inventoryItemConditionRepository;
        private readonly IBuildingService _buildingService;

        public InventoryItemService(IInventoryItemRepository inventoryItemRepository,
            IInventoryRepository inventoryRepository, IItemTypeRepository itemTypeRepository,
            IInventoryBuildingsRepository inventoryBuildingsRepository,
            IInventoryFloorsRepository inventoryFloorsRepository,
            IInventoryItemConditionRepository inventoryItemConditionRepository,
            IBuildingService buildingService)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _inventoryRepository = inventoryRepository;
            _itemTypeRepository = itemTypeRepository;
            _inventoryBuildingsRepository = inventoryBuildingsRepository;
            _inventoryFloorsRepository = inventoryFloorsRepository;
            _inventoryItemConditionRepository = inventoryItemConditionRepository;
            _buildingService = buildingService;
        }

        public virtual async Task<List<InventoryItemModel>> ReadAsync(InventoryItemFilterModel filter)
        {
            var conditions = await _inventoryItemConditionRepository.ReadAsync();
            var inventoryItems = await _inventoryItemRepository.ReadAsync(filter.Ids,
                clientId: filter.ClientId,
                buildingIds: filter.BuildingIds,
                floorIds: filter.FloorIds,
                room: filter.Room,
                inventoryIds: filter.inventoryIds,
                includeItemImages: filter.IncludeItemImages);

            foreach (var item in inventoryItems)
                item.Condition = conditions.Where(x => x.InventoryItemConditionId == item.ConditionId).Select(x => x.ConditionName).FirstOrDefault();
            return inventoryItems;
        }

        public virtual async Task<InventoryItemModel> InsertAsync(InventoryItemModel model, bool isIndividual = false)
            => await _inventoryItemRepository.InsertAsync(model, isIndividual);

        public virtual async Task<InventoryItemModel> UpdateEntityAsync(InventoryItemModel model)
            => await _inventoryItemRepository.UpdateEntityAsync(model);

        public virtual async Task<InventoryItemModel> UpdateAsync(InventoryItemModel inventoryItem)
            => await _inventoryItemRepository.UpdateAsync(inventoryItem);

        public virtual async Task UpdateBarcode(List<Tuple<int, string>> data)
        {
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    await _inventoryItemRepository.UpdateBarcode(item.Item1, item.Item2);
                }
            }
        }

        public virtual async Task<ResponseModel> UpdateLocations(UpdateLocationInputModel input)
            => await _inventoryItemRepository.UpdateLocations(input);

        public virtual async Task<List<InventoryItemSearchResultModel>> SearchByConditions(InventoryItemFilterModel filter)
        {
            var inventoryIds = new List<int>();
            List<InventoryModel> inventories = null;
            var result = new List<InventoryItemSearchResultModel>();
            int? filterItemTypeId = null;
            if (filter.ItemTypeId > 0)
                filterItemTypeId = filter.ItemTypeId;
            inventories = (await _inventoryRepository.ReadAsync(clientId: filter.ClientId, itemTypeId: filterItemTypeId, searchString: filter.SearchString)).ToList();

            if (!string.IsNullOrWhiteSpace(filter.SearchString) && (inventories is null || inventories?.Count == 0))
                return result;

            if (inventories?.Count > 0)
                inventoryIds = inventories.Select(x => x.InventoryId.Value).Distinct().ToList();

            var inventoryItemsModel = await _inventoryItemRepository.ReadAndPagingAsync(clientId: filter.ClientId,
                                                                          buildingIds: filter.BuildingIds,
                                                                          floorIds: filter.FloorIds,
                                                                          room: filter.Room,
                                                                          inventoryIds: inventoryIds,
                                                                          includeItemImages: filter.IncludeItemImages,
                                                                          conditionId: filter.ConditionId,
                                                                          currentPage: filter.CurrentPage,
                                                                          itemsPerPage: filter.ItemsPerPage);

            if (filter.ItemTypeId == 0)
            {
                inventoryIds = inventoryItemsModel.Select(x => x.InventoryId).Distinct().ToList();
                inventories = (await _inventoryRepository.ReadAsync(clientId: filter.ClientId, ids: inventoryIds)).ToList();
            }

            var itemTypeIds = inventories.Select(x => x.ItemTypeId).Distinct().ToList();
            var itemTypes = await _itemTypeRepository.ReadAsync(clientId: filter.ClientId, ids: itemTypeIds);

            var inventoryBuildingIds = inventoryItemsModel.Select(x => x.InventoryBuildingId).Distinct().ToList();
            var inventoryBuildings = (await _buildingService.GetBuildings((int)filter.ClientId))
                .Select(x => new InventoryBuildingModel
            {
                ClientId = filter.ClientId,
                InventoryBuildingId = x.InventoryBuildingId,
                InventoryBuildingName = x.InventoryBuildingName,
                InventoryBuildingCode = x.InventoryBuildingName,
                InventoryBuildingDesc = x.InventoryBuildingName
            }).ToList();

            var inventoryFloorIds = inventoryItemsModel.Select(x => x.InventoryFloorId).Distinct().ToList();
            var inventoryFloors = await _inventoryFloorsRepository.ReadAsync(ids: inventoryFloorIds);
            var conditions = await _inventoryItemConditionRepository.ReadAsync();
            foreach (var item in inventoryItemsModel)
            {
                var inventory = inventories?.Where(x => x.InventoryId == item.InventoryId).FirstOrDefault();
                result.Add(new InventoryItemSearchResultModel
                {
                    InventoryId = item.InventoryId,
                    InventoryItemId = item.InventoryItemId,
                    InventoryBuildingId = item.InventoryBuildingId,
                    Building = inventoryBuildings.Find(x => x.InventoryBuildingId == item.InventoryBuildingId)?.InventoryBuildingName,
                    Condition = conditions?.FirstOrDefault(c => c.InventoryItemConditionId == item.ConditionId)?.ConditionName,
                    InventoryFloorId = item.InventoryFloorId,
                    Floor = inventoryFloors.Find(x => x.InventoryFloorId == item.InventoryFloorId)?.InventoryFloorName,
                    Room = item.Room,
                    MainImage = inventories?.Where(x => x.InventoryId == item.InventoryId).FirstOrDefault()?.MainImage,
                    ItemTypeId = inventory?.ItemTypeId ?? 0,
                    ItemType = itemTypes.FirstOrDefault(x => x.ItemTypeId == inventory?.ItemTypeId)?.ItemTypeName,
                    PartNumber = inventory?.PartNumber,
                    Description = inventory?.Description,
                    Notes = item.DamageNotes,
                    DamageNotes = item.DamageNotes,
                    AddedToCartItem = item.AddedToCartItem
                });
            }

            return result.OrderByDescending(x => x.InventoryItemId).ToList();
        }

        public async Task<List<SimpleInventoryItemSearchResultModel>> SearchSimpleInventoryItems(SearchSimpleInventoryItem input)
        {
            var inventoryIds = new List<int>
            {
                input.InventoryID
            };
            List<InventoryModel> inventories = null;

            inventories = (await _inventoryRepository
                .ReadAsync(clientId: input.ClientId, ids: inventoryIds, includeItemType: true))
                .ToList();

            var inventoryItemsModel = (await _inventoryItemRepository.ReadAndPagingAsync(clientId: input.ClientId,
                 inventoryIds: inventoryIds, currentPage: input.CurrentPage, itemsPerPage: input.ItemsPerPage))
                .ToList();

            var inventoryBuildingIds = inventoryItemsModel.Select(x => x.InventoryBuildingId).Distinct().ToList();
            var inventoryBuildings = (await _buildingService.GetBuildings((int)input.ClientId))
                .Select(x => new InventoryBuildingModel
            {
                ClientId = input.ClientId,
                InventoryBuildingId = x.InventoryBuildingId,
                InventoryBuildingName = x.InventoryBuildingName,
                InventoryBuildingCode = x.InventoryBuildingName,
                InventoryBuildingDesc = x.InventoryBuildingName
            }).ToList();

            var inventoryFloorIds = inventoryItemsModel.Select(x => x.InventoryFloorId).Distinct().ToList();
            var inventoryFloors = await _inventoryFloorsRepository.ReadAsync(ids: inventoryFloorIds);
            var inventoryItemConditionIds = inventoryItemsModel.Select(x => x.ConditionId).Distinct().ToList();
            var inventoryItemConditionItems = await _inventoryItemConditionRepository.ReadAsync(inventoryItemConditionIds);

            if (inventories.Count == 0)
            {
                return ParseSimpleInventoryItemsToModel(inventories, inventoryItemsModel, inventoryBuildings, inventoryFloors, inventoryItemConditionItems);
            }

            Parallel.ForEach(inventoryItemsModel, item =>
            {
                item.Inventory = inventories?.Where(x => x.InventoryId == item.InventoryId).FirstOrDefault() ?? new InventoryModel();
            });
            var groupedInventories = inventoryItemsModel.GroupBy(x => new { x.InventoryItemId, x.Inventory.ItemType.ItemTypeName }, (key, group) => new
            {
                key.InventoryItemId,
                key.ItemTypeName,
                Result = group.OrderBy(x => (x.InventoryBuildingId, x.InventoryFloorId, x.Room, x.CreateDateTime)).ToList()
            });
            var data = groupedInventories.SelectMany(x => x.Result).ToList();

            return ParseSimpleInventoryItemsToModel(inventories, data, inventoryBuildings, inventoryFloors, inventoryItemConditionItems);
        }

        public async Task<InventoryItemModel> GetByIdAsync(int inventoryItemId)
        {
            return await _inventoryItemRepository.GetByIdAsync(inventoryItemId);
        }

        private List<SimpleInventoryItemSearchResultModel> ParseSimpleInventoryItemsToModel(List<InventoryModel> inventories, List<InventoryItemModel> inventoryItemsModel,
           List<InventoryBuildingModel> inventoryBuildings,
           List<InventoryFloorModel> inventoryFloors,
           List<InventoryItemConditionModel> inventoryItemConditions)
        {
            var result = new List<SimpleInventoryItemSearchResultModel>();
            foreach (var item in inventoryItemsModel)
            {
                var inventory = inventories?.Where(x => x.InventoryId == item.InventoryId).FirstOrDefault();

                if (inventory != null)
                    result.Add(new SimpleInventoryItemSearchResultModel
                    {
                        InventoryId = item.InventoryId,
                        InventoryItemID = item.InventoryItemId,
                        ClientID = item.ClientId,
                        Notes = item.DamageNotes,
                        Condition = inventoryItemConditions?.Where(x => x.InventoryItemConditionId == item.ConditionId)
                        .Select(x => x.ConditionName)?.FirstOrDefault(),
                        BuildingID = item.InventoryBuildingId,
                        Building = inventoryBuildings.Find(x => x.InventoryBuildingId == item.InventoryBuildingId)?.InventoryBuildingName,
                        Floor = inventoryFloors.Find(x => x.InventoryFloorId == item.InventoryFloorId)?.InventoryFloorName,
                        Room = item.Room,
                        MainImage = inventory?.MainImage,
                        ItemTypeCode = inventory?.ItemType?.ItemTypeCode,
                        ItemTypeName = inventory?.ItemType?.ItemTypeName,
                        CreateDateTime = item.CreateDateTime,
                        StatusID = item.StatusId,
                        AddedToCartItem = item.AddedToCartItem,
                    });
            }

            return result?.OrderByDescending(x => x.InventoryItemID).ToList();
        }
    }
}