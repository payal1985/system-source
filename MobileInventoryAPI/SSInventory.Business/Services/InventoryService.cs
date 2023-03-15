using Microsoft.EntityFrameworkCore;
using SSInventory.Business.Interfaces;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Core.Services.External;
using SSInventory.Share.Constants;
using SSInventory.Share.Enums;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryImage;
using SSInventory.Share.Models.Dto.InventoryItem;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Share.Models.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IItemTypeOptionRepository _itemTypeOptionRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IInventoryFloorsRepository _inventoryFloorsRepository;
        private readonly IInventoryImageRepository _inventoryItemImageRepository;
        private readonly IInventoryItemConditionRepository _inventoryItemConditionRepository;
        private readonly IBuildingService _buildingService;

        public InventoryService(IInventoryRepository inventoryRepository,
            IManufacturerRepository manufacturerRepository,
            IItemTypeOptionRepository itemTypeOptionRepository,
            IInventoryItemRepository inventoryItemRepository,
            IInventoryFloorsRepository inventoryFloorsRepository,
            IInventoryImageRepository inventoryItemImageRepository,
            IBuildingService buildingService,
            IInventoryItemConditionRepository inventoryItemConditionRepository)
        {
            _inventoryRepository = inventoryRepository;
            _manufacturerRepository = manufacturerRepository;
            _itemTypeOptionRepository = itemTypeOptionRepository;
            _inventoryItemRepository = inventoryItemRepository;
            _inventoryFloorsRepository = inventoryFloorsRepository;
            _inventoryItemImageRepository = inventoryItemImageRepository;
            _inventoryItemConditionRepository = inventoryItemConditionRepository;
            _buildingService = buildingService;
        }


        public async Task<List<SearchInventoryItemSubmissionModel>> GroupInventoryItemsAsync(int?[] submissionIds, int clientId)
        {
            return await _inventoryRepository.GroupInventoryItemsAsync(submissionIds, clientId);
        }

        public virtual async Task<IList<InventoryModel>> ReadAsync(SearchInventoryModel input, bool includeInventoryImage = false)
            => await _inventoryRepository.ReadAsync(clientId: input.ClientId, ids: input.Ids, itemTypeId: input.ItemTypeId,
                itemRowIds: input.ItemRowIds, deviceDate: input.DeviceDate, submissionIds: input.SubmissionIds,
                includeInventoryImage: includeInventoryImage);

        public virtual async Task<InventoryModel> InsertAsync(InventoryModel model)
            => await _inventoryRepository.InsertAsync(model);

        public virtual async Task<List<InventoryModel>> UpdateAsync(List<InventoryModel> models)
            => await _inventoryRepository.UpdateAsync(models);

        public virtual async Task UpdateInventoryCodeAndQRcodes(List<Tuple<int, string>> data)
        {
            if (data.Count > 0)
            {
                var inventoryIds = data.ConvertAll(y => y.Item1).ToList();
                var inventories = await _inventoryRepository.ReadAsync(ids: inventoryIds);
                if (inventories.Count > 0)
                {
                    foreach (var inventory in inventories)
                    {
                        var itemCode = $"{inventory.ItemCode}{(!string.IsNullOrWhiteSpace(inventory.ManufacturerName) ? inventory.ManufacturerName.Replace(", ", "") : "")}{inventory.InventoryId}";

                        var QRcode = data.FirstOrDefault(x => x.Item1 == inventory.InventoryId)?.Item2;
                        await _inventoryRepository.UpdateInventoryCodeAndQRcode(inventory.InventoryId.Value, itemCode, QRcode);
                    }
                }
            }
        }

        public async Task<List<InventoryModel>> UpdateMainImages(List<Tuple<int, string>> mainImages)
        {
            var inventoryIds = mainImages.Select(x => x.Item1).ToList();
            var images = mainImages.Select(x => x.Item2).ToList();
            var inventoryImages = await _inventoryItemImageRepository.ReadAsync(null, images, null, null, inventoryIds);
            var mainImagesRef = new List<Tuple<int, string>>();
            foreach (var image in mainImages)
            {
                if(inventoryImages.Any(x => x.TempPhotoName == image.Item2))
                    mainImagesRef.Add(new Tuple<int, string>(image.Item1,
                        inventoryImages.Where(x => x.TempPhotoName == image.Item2
                        && x.InventoryId == image.Item1)
                        .Select(x => x.ImageName)
                        .FirstOrDefault()));
            }    
            return await _inventoryRepository.UpdateMainImages(mainImagesRef);
        }

        public virtual async Task<SearchResultModel> SearchByImage(SearchModel input, string uploadedImagePath = null, bool usedAws = true)
            => await _inventoryRepository.SearchByImage(input, uploadedImagePath, usedAws);

        public virtual async Task<List<DataItemType>> GetInventorySearch(SearchImageInfo input, string uploadedImagePath = null,
            bool usedAws = true, string baseUrl = "",
            bool includeTotalCountImages = false, bool includeQRcode = false, bool includeQuanlity = false, bool isSearch = false,
            bool forEdit = false, bool includeInventoryItem = true)
            => await _inventoryRepository.GetInventorySearch(input, uploadedImagePath, usedAws, baseUrl, includeTotalCountImages, includeQRcode, includeQuanlity, isSearch, forEdit: forEdit, includeInventoryItem);

        public virtual async Task<List<DataItemType>> SearchInventory(SearchImageInfo input,
            string uploadedImagePath = null,
            bool usedAws = true,
            string baseUrl = "",
            bool includeTotalCountImages = false,
            bool includeQRcode = false,
            bool includeQuanlity = false,
            bool isSearch = false,
            bool forEdit = false,
            bool includeInventoryItem = true,
            bool includeConditionData = true,
            string searchAPIName = "")
        {
            var inventories = await _inventoryRepository.Filter(clientId: input.ClientId,
                                                               ids: new List<int> { input.InventoryId },
                                                     submissionIds: new List<int?> { input.InventorySubmissionId }).ToListAsync();
            if (inventories.Any())
            {
                var result = new List<DataItemType>();
                foreach (var inventory in inventories)
                {
                    result.Add(await ParseInventorySearchResult(inventory, uploadedImagePath, usedAws, baseUrl, includeQRcode, isSearch, inventoryItemId: input.InventoryItemId, input.InventoryItemSubmissionId, includeInventoryItem, includeConditionData, searchAPIName));
                }
                return result;
            }

            return null;
        }

        public virtual async Task<List<DataInventoryType>> SearchSimpleInventorySearch(SearchSimpleInventoryRequestModel input)
         {
            var inventoryIds = await _inventoryItemRepository.GetInventoryIdsAsync((int)input.ClientId, (int)input.ConditionId, input.Room);
            var inventory = await _inventoryRepository.SearchSimpleInventorySearch(new SearchSimpleInventoryModel
            {
                ClientId = input.ClientId,
                CurrentPage = input.CurrentPage,
                InventoryIds = inventoryIds,
                ItemsPerPage = input.ItemsPerPage,
                ItemTypeId = input.ItemTypeId,
                SearchString = input.SearchString
            });
            return inventory;
        }

        public virtual async Task<List<DataInventoryTypeWithFirstItemLocationInfo>> GetSimpleInventoriesWithFirstItemLocationInfo(SearchSimpleInventoryRequestModel input)
        {
            var inventoryIds = await _inventoryItemRepository.GetInventoryIdsAsync((int)input.ClientId, (int)input.ConditionId, input.Room);
            var inventory = await _inventoryRepository.GetSimpleInventoriesWithFirstItemLocationInfo(new SearchSimpleInventoryWithFirstItemLocationModel
            {
                ClientId = input.ClientId,
                CurrentPage = input.CurrentPage,
                InventoryIds = inventoryIds,
                ItemsPerPage = input.ItemsPerPage,
                ItemTypeId = input.ItemTypeId,
                SearchString = input.SearchString,
                ConditionId = input.ConditionId
            });
            return inventory;
        }

        public Task UpdateInventoryToHistory(InventoryHistoryViewModel oldValue, InventoryHistoryViewModel newValue,
           string api, string description)
           => _inventoryRepository.UpdateInventoryToHistory(oldValue, newValue, api, description);

        public Task UpdateInventoriesToHistory(List<InventoryHistoryViewModel> oldValues, List<InventoryHistoryViewModel> newValues,
            string api, string description)
            => _inventoryRepository.UpdateInventoriesToHistory(oldValues, newValues, api, description);

        #region Private Methods

        private async Task<DataItemType> ParseInventorySearchResult(Inventory inventory, string uploadedImagePath = null, bool usedAws = true, string baseUrl = "", bool includeQRcode = false,
            bool isSearch = false, int? inventoryItemId = null, int? inventoryItemSubmissionId = null, bool includeInventoryItem = true, bool includeConditionData = true, string searchAPIName = "")
        {
            return new DataItemType
            {
                ItemTypeID = inventory.ItemTypeId,
                ClientId = inventory.ClientId,
                ItemCode = inventory.ItemCode,
                InventoryRowId = inventory.ItemRowId,
                InventoryId = inventory.InventoryId,
                QRcode = inventory.QRCode,
                GlobalProductCatalogID = inventory.GlobalProductCatalogID,
                WarrantyYears = inventory.WarrantyYears,
                MainImage = !string.IsNullOrWhiteSpace(inventory.MainImage) ? (usedAws ? inventory.MainImage : $"{baseUrl}/{uploadedImagePath}/{inventory.MainImage}") : "",
                ItemTypeOptions = await ParseInventoryToOptions(inventory, includeQRcode, isSearch, inventoryItemId: inventoryItemId, inventoryItemSubmissionId, includeInventoryItem, includeConditionData, searchAPIName)
            };
        }

        private async Task<List<ItemTypeOptionApiModel>> ParseInventoryToOptions(Inventory inventory,
            bool includeQRcode = false, bool isSearch = false, int? inventoryItemId = null, int? submissionId = null,
            bool includeInventoryItem = true,
            bool includeConditionData = true,
            string searchAPIName = "")
        {
            var manufacturer = _manufacturerRepository.GetAll().FirstOrDefault(x => x.ManufacturerId == inventory.ManufacturerId);
            var itemTypeOptions = await GetItemTypeOptions(inventory.ItemTypeId, inventory.ClientId);
            var result = new List<ItemTypeOptionApiModel>
            {
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.Description,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.Description),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Description })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.Manufacturer,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.Manufacturer),
                    ItemTypeOptionReturnValue = ParseDropdownSelectedValue(new List<SelectItemOptionModel>
                    {
                        new SelectItemOptionModel
                        {
                            ReturnValue = inventory.ManufacturerName,
                            ReturnID = inventory.ManufacturerId,
                            ReturnCode = manufacturer?.ManufacturerName?.Replace(" ", ""),
                            ReturnName = manufacturer?.ManufacturerName
                        }
                    })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.Modular,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.Modular),
                    ItemTypeOptionReturnValue = ParseSelectedMultipleValues(new List<object> { inventory.Modular })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.PartNumber,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.PartNumber),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.PartNumber })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.Finish,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.Finish),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Finish, inventory.Finish2 })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.Fabric,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.Fabric),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Fabric, inventory.Fabric2 })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.Unit,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.Unit),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Unit })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.Height,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.Height),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Height })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.Width,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.Width),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Width })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.TopFinish,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.TopFinish),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Top })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.EdgeFinish,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.EdgeFinish),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Edge })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.BaseFinish,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.BaseFinish),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Base })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.Depth,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.Depth),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Depth })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.Diameter,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.Diameter),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Diameter })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.FrameFinish,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.FrameFinish),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Frame })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.BackFinish,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.BackFinish),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Back })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.SeatFinish,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.SeatFinish),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Seat })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.SeatHeight,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.SeatHeight),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.SeatHeight })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = ItemTypeOptionCodeConstants.Tag,
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.Tag),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Tag })
                }
            };

            if (includeInventoryItem)
            {
                var inventoryItems = await _inventoryItemRepository.GetAll().Where(x => x.InventoryId == inventory.InventoryId)
                                                                   .WhereIf(isSearch && submissionId != null,
                                                                   x => x.SubmissionId == submissionId)
                                                                   .ToListAsync();
                if (inventoryItems?.Any() == true)
                {
                    var firstInventoryItem = inventoryItems.WhereIf(inventoryItemId.HasValue, x => x.InventoryItemId == inventoryItemId).FirstOrDefault();
                    if (firstInventoryItem is null)
                        firstInventoryItem = inventoryItems[0];

                    var building = (await _buildingService.GetBuildings(inventory.ClientId))
                        .Where(x => x.InventoryBuildingId == firstInventoryItem.InventoryBuildingId)
                        .FirstOrDefault();

                    if(building != null)
                    {
                        result.Add(new ItemTypeOptionApiModel
                        {
                            ItemTypeOptionCode = ItemTypeOptionCodeConstants.Building,
                            ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.Building),
                            ItemTypeOptionReturnValue = ParseDropdownSelectedValue(new List<SelectItemOptionModel>
                            {
                                new SelectItemOptionModel
                                {
                                    ReturnValue = building.InventoryBuildingName,
                                    ReturnID = firstInventoryItem.InventoryBuildingId,
                                    ReturnName = building.InventoryBuildingName,
                                    ReturnCode = building.InventoryBuildingName
                                }
                            })
                        });
                    }
                    var floor = await _inventoryFloorsRepository.GetAll().FirstOrDefaultAsync(x => x.InventoryFloorId == firstInventoryItem.InventoryFloorId);
                   
                    result.Add(new ItemTypeOptionApiModel
                    {
                        ItemTypeOptionCode = ItemTypeOptionCodeConstants.Floor,
                        ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.Floor),
                        ItemTypeOptionReturnValue = ParseDropdownSelectedValue(new List<SelectItemOptionModel>
                        {
                            new SelectItemOptionModel
                            {
                                ReturnValue = floor.InventoryFloorCode,
                                ReturnID = firstInventoryItem.InventoryFloorId,
                                ReturnName = floor.InventoryFloorName,
                                ReturnCode = floor.InventoryFloorCode,
                                ReturnDesc = floor.InventoryFloorDesc
                            }
                        })
                    });
                    result.Add(new ItemTypeOptionApiModel
                    {
                        ItemTypeOptionCode = ItemTypeOptionCodeConstants.GPS,
                        ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.GPS),
                        ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { firstInventoryItem.Gpslocation })
                    });
                    result.Add(new ItemTypeOptionApiModel
                    {
                        ItemTypeOptionCode = ItemTypeOptionCodeConstants.AreaOrRoom,
                        ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.AreaOrRoom),
                        ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { firstInventoryItem.Room })
                    });
                    //Key: TotalCount
                    result.Add(new ItemTypeOptionApiModel
                    {
                        ItemTypeOptionCode = ItemTypeOptionCodeConstants.TotalCount,
                        ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, ItemTypeOptionCodeConstants.TotalCount),
                        ItemTypeOptionReturnValue = await ParseInventoryItemImagesForSearch(inventoryItems, inventory.InventoryId, includeQRcode, inventoryItemId, includeConditionData: includeConditionData, searchAPIName: searchAPIName)
                    });
                }
            }

            return result;
        }

        private async Task<IEnumerable<Tuple<string, string>>> GetItemTypeOptions(int? itemTypeId, int? clientId)
        {
            if (!itemTypeId.HasValue || !clientId.HasValue) return null;

            var itemTypeOptions = await _itemTypeOptionRepository.GetAll().Where(x => x.ItemTypeId == itemTypeId && x.ClientId == clientId).ToListAsync();
            return itemTypeOptions.Count == 0
                ? null
                : itemTypeOptions.Select(x => new Tuple<string, string>(x.ItemTypeOptionCode, x.ItemTypeOptionName));
        }

        private string GetItemTypeOptionByCode(IEnumerable<Tuple<string, string>> itemTypeOptions, string itemTypeOptionCode)
        {
            return string.IsNullOrWhiteSpace(itemTypeOptionCode) || itemTypeOptions?.Any() != true
                ? string.Empty
                : (itemTypeOptions.FirstOrDefault(x => x.Item1.Equals(itemTypeOptionCode))?.Item2);
        }

        private object ParseSelectedValue(List<object> values)
        {
            var result = new List<Dictionary<string, object>>();
            foreach (var value in values)
            {
                if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                {
                    result.Add(new Dictionary<string, object>() { { "returnValue", value.ToString() } });
                }
            }

            return result.Count > 0 ? result : null;
        }

        private object ParseDropdownSelectedValue(List<SelectItemOptionModel> items)
        {
            var result = new List<Dictionary<string, object>>();
            foreach (var item in items)
            {
                if (item != null && !string.IsNullOrWhiteSpace(item.ReturnName))
                {
                    result.Add(new Dictionary<string, object>()
                    {
                        { "returnValue", item.ReturnValue },
                        { "returnID", item.ReturnID },
                        { "returnCode", item.ReturnCode },
                        { "returnName", item.ReturnName },
                        { "returnDesc", item.ReturnDesc }
                    });
                }
            }
            return result.Count > 0 ? result : null;
        }

        private object ParseSelectedMultipleValues(List<object> values)
        {
            var result = new List<Dictionary<string, object>>();
            foreach (var value in values)
            {
                if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                {
                    foreach (var item in value.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries))
                    {
                        result.Add(new Dictionary<string, object>() { { "returnValue", item } });
                    }
                }
            }

            return result.Count > 0 ? result : null;
        }

        private async Task<object> ParseInventoryItemImages(List<InventoryItem> inventoryItems, bool includeQRcode = false, bool includeQuanlity = false, bool forEdit = false)
        {
            var data = new List<Dictionary<string, List<InventoryImageResponseModel>>>();
            var inventoryItemQRcodes = new List<string>();
            var inventoryQuanlities = new List<Dictionary<string, int>>();
            foreach (var item in inventoryItems)
            {
                if (includeQRcode && !string.IsNullOrWhiteSpace(item.Qrcode))
                {
                    inventoryItemQRcodes.Add($"{item.Qrcode}");
                }

                var images = await _inventoryItemImageRepository.GetAll()
                    .Where(x => x.InventoryItemId == item.InventoryItemId
                    && x.StatusId == InventoryImageStatus.Active).ToListAsync();

                if (includeQuanlity)
                {
                    var inventoryCount = await _inventoryRepository.GetAll().CountAsync(x => x.InventoryId == item.InventoryId);
                    // Add images counter for the GetSubmission API
                    var existingInventory = inventoryQuanlities.Find(x => x.ContainsKey(item.ConditionId.ToString()));
                    if (existingInventory != null)
                    {
                        existingInventory.TryGetValue(item.ConditionId.ToString(), out int imageCounter);
                        existingInventory[item.ConditionId.ToString()] = imageCounter + inventoryCount;
                    }
                    else
                    {
                        inventoryQuanlities.Add(new Dictionary<string, int>() { { item.ConditionId.ToString(), inventoryCount } });
                    }
                }

                var currentDic = data.Find(x => x.ContainsKey(item.ConditionId.ToString()));
                if (currentDic != null)
                {
                    currentDic.TryGetValue(item.ConditionId.ToString(), out List<InventoryImageResponseModel> listObj);
                    listObj.AddRange(ConvertInventoryItemImagesToObject(images, forEdit));
                    currentDic[item.ConditionId.ToString()] = listObj;
                }
                else
                {
                    data.Add(new Dictionary<string, List<InventoryImageResponseModel>>()
                    {
                        {
                            item.ConditionId.ToString(), ConvertInventoryItemImagesToObject(images, forEdit)
                        }
                    });
                }
            }

            var result = new List<object>();
            foreach (var condition in data)
            {
                var key = condition.Keys.First();
                condition.TryGetValue(key, out List<InventoryImageResponseModel> values);

                if (includeQRcode)
                {
                    var imageCounter = 0;
                    var quanlityDic = inventoryQuanlities.Find(x => x.ContainsKey(key));
                    quanlityDic?.TryGetValue(key, out imageCounter);
                    result.Add(new
                    {
                        nameCondition = key,
                        dataCondition = new
                        {
                            url = values,
                            imageCounter = values.Count,
                            txtQuanlity = imageCounter,
                            inventoryItemQRcodes = inventoryItemQRcodes.ToArray()
                        }
                    });
                }
                else
                {
                    result.Add(new { nameCondition = key, dataCondition = new { url = values, imageCounter = values.Count } });
                }

            }

            return result;
        }
        /// <summary>
        /// Get Inventory Item Images
        /// </summary>
        /// <param name="inventoryItems"></param>
        /// <param name="includeQRcode"></param>
        /// <returns></returns>
        private async Task<object> ParseInventoryItemImagesForSearch(List<InventoryItem> inventoryItems, int inventoryId, bool includeQRcode = false,
            int? inventoryItemId = null, bool includeConditionData = true, string searchAPIName = "")
        {
            if (inventoryItems.Count < 1) return new List<InventorySearchingModel>();

            var data = new List<InventorySearchingModel>();
            var conditions = await _inventoryItemConditionRepository.GetAll().ToListAsync();

            var itemConditionId = 0;
            List<int> inventoryItemIds = new List<int>();
            if (inventoryItemId == null)
            {
                inventoryItemIds = inventoryItems.Select(x => x.InventoryItemId).Distinct().ToList();
            }
            else
            {
                inventoryItemIds.Add(inventoryItemId.GetValueOrDefault());

                var item = await _inventoryItemRepository.GetByIdAsync(inventoryItemId.GetValueOrDefault());
                itemConditionId = item.ConditionId;
            }

            var inventoryItemImages = await _inventoryItemImageRepository.GetAll()
                                            .Where(x => x.InventoryItemId.HasValue
                                            && inventoryItemIds.Contains(x.InventoryItemId.Value)
                                            && x.StatusId == InventoryImageStatus.Active)
                                            .ToListAsync();


            var inventoryResentativeImages = await _inventoryItemImageRepository.GetAll()
               .Where(x => x.InventoryItemId == null
               && x.InventoryId.Value == inventoryId
               && (inventoryItemId == null || x.ConditionId == itemConditionId)
               && x.StatusId == InventoryImageStatus.Active)
               .ToListAsync();

            var groupRepresentativeImages = inventoryResentativeImages.GroupBy(x =>
            new { x.InventoryId, x.ConditionId })
                .Select(x => new
                {
                    x.Key.InventoryId,
                    Condition = conditions.FirstOrDefault(y => y.InventoryItemConditionId == x.Key.ConditionId)?.ConditionName,
                    ConditionID = conditions.FirstOrDefault(y => y.InventoryItemConditionId == x.Key.ConditionId)?.InventoryItemConditionId,
                    x.Key.ConditionId,
                    Counter = x.Count()
                });

            var groupInventoryItems = inventoryItems.Where(x => x.InventoryItemId == inventoryItemId || inventoryItemId == null)
                .GroupBy(x => new { x.InventoryId, x.ClientId, x.InventoryBuildingId, x.InventoryFloorId, x.Room, x.ConditionId })
                .Select(x => new
                {
                    x.Key.InventoryId,
                    x.Key.ClientId,
                    x.Key.InventoryBuildingId,
                    x.Key.InventoryFloorId,
                    x.Key.Room,
                    x.Key.ConditionId
                }).ToList();

            var inventories = await _inventoryRepository.GetAll().Where(x => inventoryItems.Select(y => y.InventoryId).Distinct().Contains(x.InventoryId))
                                                        .ToListAsync();
            var index = 1;
            foreach (var item in groupInventoryItems)
            {
                var selectedInventoryItems = inventoryItems.Where(x => x.InventoryId == item.InventoryId
                                                           && x.ClientId == item.ClientId
                                                           && x.InventoryBuildingId == item.InventoryBuildingId
                                                           && x.InventoryFloorId == item.InventoryFloorId
                                                           && x.Room == item.Room
                                                           && x.ConditionId == item.ConditionId);
                if (selectedInventoryItems?.Any() != true) continue;


                var dataConditions = new List<DataConditionModel>();
                foreach (var inventoryItem in selectedInventoryItems)
                {
                    var inventoryItemBarcodes = new List<string>();
                    if (includeQRcode && !string.IsNullOrWhiteSpace(inventoryItem.Barcode))
                    {
                        inventoryItemBarcodes.Add($"{inventoryItem.Barcode}");
                    }
                    var images = inventoryItemImages.Where(x => x.InventoryItemId == inventoryItem.InventoryItemId).ToList();
                    var imageUrls = ConvertInventoryItemImagesToObject(images);

                    if ((inventoryItemId == inventoryItem.InventoryItemId && inventoryItemId != null && searchAPIName == "GetInventoryItemInfo") || searchAPIName != "GetInventoryItemInfo")
                        dataConditions.Add(new DataConditionModel
                        {
                            ItemName = "",
                            DamageNotes = inventoryItem.DamageNotes,
                            InventoryItemId = inventoryItem.InventoryItemId,
                            Url = imageUrls,
                            InventoryItemBarcodes = inventoryItemBarcodes,
                            TxtQuantity = images.Count()
                        });
                }

                data.Add(new InventorySearchingModel
                {
                    ConditionID = item.ConditionId,

                    InventoryId = item.InventoryId,
                    DataCondition = dataConditions
                });
                index++;
            }

            var result = new List<object>();
            foreach (var item in data)
            {
                var urls = item.DataCondition.Where(x => x.Url?.Any() == true).SelectMany(x => x.Url).ToList();
                var imageCount = urls.Count();
                var representative = groupRepresentativeImages.FirstOrDefault(x => x.InventoryId == item.InventoryId);
                var representativeImages = inventoryResentativeImages.Where(x => x.ConditionId == item.ConditionID
                                                && x.InventoryId == item.InventoryId).ToList();

                var inventoryItemBarcodesList = item.DataCondition.Where(x => x.InventoryItemBarcodes?.Any() == true)
                    .ToList();
                if (includeQRcode)
                {
                    result.Add(new
                    {
                        conditionID = item.ConditionID,
                        type = item.ConditionID,
                        txtQuantity = item.DataCondition.Count,
                        existedItems = true,
                        representativePhotos = new[]
                        {
                            new
                            {
                                representativePhotoTotalCount = representativeImages.Count(),
                                url = representativeImages.Count()>0?ConvertInventoryItemRepresentativeImagesToObjectV2(representativeImages):new List<InventoryImageV2ResponseModel>()
                            }
                        },
                        conditionData = includeConditionData ? item.DataCondition.Select(c => new
                        {
                            inventoryItemId = c.InventoryItemId,
                            damageNotes = c.DamageNotes,
                            itemName = c.ItemName,
                            url = urls.Count() > 0 ?
                            ConvertInventoryItemImagesToObjectV2(urls.Where(x => x.InventoryItemId == c.InventoryItemId).ToList())
                            : new List<InventoryImageV2ResponseModel>(),
                            //imageCounter = imageCount,
                            txtQuantity = urls.Count(x => x.InventoryItemId == c.InventoryItemId),
                            inventoryItemBarcodes = inventoryItemBarcodesList.Where(x => x.InventoryItemId == c.InventoryItemId)
                            .Select(x => x.InventoryItemBarcodes).ToArray()
                        }) : null,


                    }); ;
                }
                else
                {

                    result.Add(new
                    {
                        conditionID = item.ConditionID,
                        type = item.ConditionID,
                        txtQuantity = item.DataCondition.Count,
                        existedItems = true,
                        representativePhotos = new[]
                        {
                            new
                            {
                                representativePhotoTotalCount = representativeImages.Count(),
                                url = representativeImages.Count()>0?ConvertInventoryItemRepresentativeImagesToObjectV2(representativeImages):new List<InventoryImageV2ResponseModel>()
                            }
                        },
                        conditionData = includeConditionData ? item.DataCondition.Select(c => new
                        {
                            inventoryItemId = c.InventoryItemId,
                            damageNotes = c.DamageNotes,
                            itemName = c.ItemName,
                            url = urls.Count() > 0
                            ? ConvertInventoryItemImagesToObjectV2(urls.Where(x => x.InventoryItemId == c.InventoryItemId).ToList())
                            : new List<InventoryImageV2ResponseModel>(),
                            //imageCounter = imageCount,
                            txtQuantity = urls.Count(x => x.InventoryItemId == c.InventoryItemId),
                            inventoryItemBarcodes = inventoryItemBarcodesList.Where(x => x.InventoryItemId == c.InventoryItemId)
                            .Select(x => x.InventoryItemBarcodes).ToArray()
                        }) : null

                    });
                }
            }
            return result;
        }

        private List<InventoryImageResponseModel> ConvertInventoryItemImagesToObject(List<InventoryImages> images,
            bool forEdit = true)
        {
            var result = new List<InventoryImageResponseModel>();
            foreach (var image in images)
            {
                result.Add(new InventoryImageResponseModel
                {
                    Width = image.Width,
                    Height = image.Height,
                    Name = image.ImageName,
                    ImageUrl = $"{image.ImageUrl}/{image.ClientId}",
                    TempPhotoName = image.TempPhotoName,
                    InventoryItemId = forEdit ? image.InventoryItemId : null,
                    InventoryImageId = forEdit ? image.InventoryImageId : null
                });
            }
            return result;
        }

        private List<InventoryImageV2ResponseModel> ConvertInventoryItemImagesToObjectV2(List<SSInventory.Share.Models.Dto.InventoryImage.InventoryImageResponseModel> images,
            bool forEdit = true)
        {
            var result = new List<InventoryImageV2ResponseModel>();
            foreach (var image in images)
            {
                result.Add(new InventoryImageV2ResponseModel
                {
                    Width = image.Width,
                    InventoryItemImgId = forEdit ? image.InventoryImageId : null,
                    Height = image.Height,
                    Name = image.Name,
                    TempPhotoName = image.TempPhotoName

                });
            }
            return result;
        }

        private List<InventoryImageV2ResponseModel> ConvertInventoryItemRepresentativeImagesToObjectV2(List<InventoryImages> images,
            bool forEdit = true)
        {
            var result = new List<InventoryImageV2ResponseModel>();
            foreach (var image in images)
            {
                result.Add(new InventoryImageV2ResponseModel
                {
                    Width = image.Width,
                    InventoryItemImgId = forEdit ? image.InventoryImageId : null,
                    Height = image.Height,
                    Name = image.ImageName,
                    TempPhotoName = image.TempPhotoName

                });
            }
            return result;
        }




        #endregion
    }
}
