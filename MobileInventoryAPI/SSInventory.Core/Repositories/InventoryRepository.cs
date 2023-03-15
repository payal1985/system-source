using AutoMapper;
//using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SSInventory.Core.Models;
using SSInventory.Core.Models.External;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Core.Services.External;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryBuildings;
using SSInventory.Share.Models.Dto.InventoryImage;
using SSInventory.Share.Models.Dto.InventoryItem;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Share.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {
        private readonly IMapper _mapper;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IInventoryImageRepository _inventoryImageRepository;
        private readonly IBuildingService _buildingService;
        private readonly IHistoryTableRepository _historyTableRepository;
        private readonly IInventoryItemConditionRepository _inventoryItemConditionRepository;
        private readonly string _apiName;

        public InventoryRepository(SSInventoryDbContext dbContext, IMapper mapper,
            IManufacturerRepository manufacturerRepository,
            IInventoryImageRepository inventoryImageRepository,
            IBuildingService buildingService,
            IHistoryTableRepository historyTableRepository,
            IInventoryItemConditionRepository inventoryItemConditionRepository,
            IHttpContextAccessor httpContextAccessor)
            : base(dbContext)
        {
            _mapper = mapper;
            _manufacturerRepository = manufacturerRepository;
            _inventoryImageRepository = inventoryImageRepository;
            _buildingService = buildingService;
            _historyTableRepository = historyTableRepository;
            _inventoryItemConditionRepository = inventoryItemConditionRepository;
            _apiName = $"{httpContextAccessor.HttpContext?.Request?.Scheme}://{httpContextAccessor.HttpContext?.Request?.Host}{httpContextAccessor.HttpContext?.Request?.PathBase}{httpContextAccessor.HttpContext?.Request?.Path}";
        }


        public async Task<IList<InventoryModel>> ReadAsync(int? clientId = null, List<int> ids = null, int? itemTypeId = null,
          List<int> itemRowIds = null, DateTime? deviceDate = null, List<int?> submissionIds = null, string searchString = null,
          bool includeItemType = false, bool includeInventoryImage = false)
        {
            var query = Filter(clientId, ids, itemTypeId, itemRowIds: itemRowIds, deviceDate: deviceDate, submissionIds: submissionIds, searchString: searchString);
            if (includeItemType)
                query = query.Include(x => x.ItemType);

            var entities = await query.AsNoTracking().ToListAsync();
            var inventoryImages = new List<InventoryImageModel>();
            if (includeInventoryImage)
            {
                inventoryImages = await _inventoryImageRepository.ReadAsync(inventoryIds: entities.Select(x => x.InventoryId).Distinct().ToList());
            }
            var result = new List<InventoryModel>();
            foreach (var entity in entities)
            {
                var inventory = _mapper.Map<InventoryModel>(entity);
                inventory.InventoryImageModels = inventoryImages.Where(x => x.InventoryId == entity.InventoryId).ToList();
                result.Add(inventory);
            }

            return result;
        }

        public async Task<List<SearchInventoryItemSubmissionModel>> GroupInventoryItemsAsync(int?[] submissionIds, int clientId)
        {
            var result = await (from ii in _dbContext.InventoryItem
                                join i in _dbContext.Inventory on ii.InventoryId equals i.InventoryId
                                where submissionIds.Contains(ii.SubmissionId) && ii.ClientId == clientId
                                group ii by new { ii.InventoryItemId, ii.InventoryId, ii.InventoryBuildingId, ii.InventoryFloorId, ii.Gpslocation, ii.Room, ii.SubmissionId } into g
                                select new SearchInventoryItemSubmissionModel
                                {
                                    InventoryItemId = g.Key.InventoryItemId,
                                    InventoryId = g.Key.InventoryId,
                                    InventoryBuildingId = g.Key.InventoryBuildingId,
                                    InventoryFloorId = g.Key.InventoryFloorId,
                                    GpsLocation = g.Key.Gpslocation,
                                    Room = g.Key.Room,
                                    SubmissionId = g.Key.SubmissionId
                                }).OrderBy(p => p.InventoryItemId).ThenBy(p => p.InventoryId).ThenBy(p => p.InventoryBuildingId).ThenBy(p => p.InventoryFloorId).ThenBy(p => p.GpsLocation).ThenBy(p => p.Room).ThenBy(p => p.SubmissionId).ToListAsync();
            return result;
        }

        public async Task<InventoryModel> InsertAsync(InventoryModel model)
        {
            var entity = await AddAsync(_mapper.Map<Inventory>(model));
            await SaveManufacturer(model.ManufacturerName).ConfigureAwait(false);
            await InsertInventoryToHistory(_mapper.Map<InventoryHistoryViewModel>(entity), entity.CreateId, _apiName, null);
            return _mapper.Map<InventoryModel>(entity);
        }

        public virtual async Task<List<InventoryModel>> UpdateAsync(List<InventoryModel> models)
        {
            var entityList = _mapper.Map<List<Inventory>>(models);
            await UpdateAsync(entityList);
            return _mapper.Map<List<InventoryModel>>(entityList);
        }

        public virtual async Task<bool> UpdateInventoryCodeAndQRcode(int id, string code, string QRcode)
        {
            var entity = await GetAll().FirstOrDefaultAsync(x => x.InventoryId == id);
            if (entity is null) return false;
            var oldValue = _mapper.Map<InventoryHistoryViewModel>(entity);

            entity.ItemCode = code;
            //entity.QRcode = QRcode;
            await UpdateAsync(entity);
            var newValue = _mapper.Map<InventoryHistoryViewModel>(entity);
            await UpdateInventoryToHistory(oldValue, newValue, _apiName, null);

            return true;
        }

        public async Task<List<InventoryModel>> UpdateMainImages(List<Tuple<int, string>> mainImages)
        {
            foreach (var item in mainImages)
            {
                var entity = GetAll().FirstOrDefault(x => x.InventoryId == item.Item1);
                if (entity is null || string.IsNullOrEmpty(item.Item2)) continue;
                entity.MainImage = item.Item2;
                await UpdateAsync(entity, false);
            }

            await _dbContext.SaveChangesAsync();

            var inventoryIds = mainImages.ConvertAll(y => y.Item1).ToList();
            var entities = await GetAll().Where(x => inventoryIds.Contains(x.InventoryId)).AsNoTracking().ToListAsync();
            return _mapper.Map<List<InventoryModel>>(entities);
        }

        public async Task<SearchResultModel> SearchByImage(SearchModel input, string uploadedImagePath = null, bool usedAws = true)
        {
            var query = Filter(clientId: input.ClientId, itemTypeId: input.ItemTypeId);

            var totalCount = await query.CountAsync();

            query = query.OrderByDescending(x => x.UpdateDateTime);

            query = query.Skip((input.CurrentPage - 1) * input.ItemsPerPage).Take(input.ItemsPerPage);
            var result = await query.ToListAsync();
            var data = result.ConvertAll(x => new InventorySearchImageResultModel
            {
                InventoryId = x.InventoryId,
                MainImage = usedAws ? x.MainImage : $"{uploadedImagePath}/{(!string.IsNullOrWhiteSpace(x.MainImage) ? x.MainImage : "default.jpg")}"
            });
            return new SearchResultModel
            {
                TotalItem = totalCount,
                Inventories = data
            };
        }

        public virtual async Task<List<DataItemType>> GetInventorySearch(SearchImageInfo input, string uploadedImagePath = null,
            bool usedAws = true, string baseUrl = "", bool includeTotalCountImages = false, bool includeQRcode = false,
            bool includeQuanlity = false, bool isSearch = false, bool forEdit = false, bool includeInventoryItem = true)
        {
            var inventories = await Filter(clientId: input.ClientId,
                                                ids: new List<int> { input.InventoryId },
                                      submissionIds: new List<int?> { input.InventorySubmissionId }).AsNoTracking().ToListAsync();
            if (inventories != null)
            {
                var result = new List<DataItemType>();
                foreach (var inventory in inventories)
                {
                    result.Add(await ParseInventorySearchResult(inventory, uploadedImagePath, usedAws, baseUrl, includeTotalCountImages, includeQRcode, includeQuanlity, isSearch, inventoryItemId: input.InventoryItemId, forEdit: forEdit, includeInventoryItem));
                }
                return result;
            }

            return null;
        }

        public virtual async Task<List<DataInventoryType>> SearchSimpleInventorySearch(SearchSimpleInventoryModel input)
        {
            var inventories = Filter(clientId: input.ClientId, itemTypeId: input.ItemTypeId, searchString: input.SearchString);
            var result = new List<DataInventoryType>();
            if (inventories.Any())
            {
                if (input.InventoryIds?.Count > 0)
                    inventories = inventories.Where(x => input.InventoryIds.Contains(x.InventoryId));
                else
                    return result;

                var invent = await inventories
                    .OrderByDescending(x => x.CreateDateTime)
                    .Skip((input.CurrentPage - 1) * input.ItemsPerPage)
                    .Take(input.ItemsPerPage).ToListAsync();

                foreach (var inventory in invent)
                {
                    var obj = await ParseSimpleInventorySearchResult(inventory);
                    if (obj != null)
                        result.Add(obj);
                }
            }

            return result;
        }

        public virtual async Task<List<DataInventoryTypeWithFirstItemLocationInfo>> GetSimpleInventoriesWithFirstItemLocationInfo(SearchSimpleInventoryWithFirstItemLocationModel input)
        {
            var inventories = Filter(clientId: input.ClientId, itemTypeId: input.ItemTypeId, searchString: input.SearchString);
            var result = new List<DataInventoryTypeWithFirstItemLocationInfo>();
            if (inventories.Any())
            {
                if (input.InventoryIds?.Count > 0)
                    inventories = inventories.Where(x => input.InventoryIds.Contains(x.InventoryId));
                else
                    return result;

                var invent = await inventories
                    .OrderByDescending(x => x.CreateDateTime)
                    .Skip((input.CurrentPage - 1) * input.ItemsPerPage)
                    .Take(input.ItemsPerPage).ToListAsync();

                var inventoryBuildings = (await _buildingService.GetBuildings((int)input.ClientId))
                                        .Select(x => new InventoryBuildingModel
                                        {
                                            ClientId = input.ClientId,
                                            InventoryBuildingId = x.InventoryBuildingId,
                                            InventoryBuildingName = x.InventoryBuildingName,
                                            InventoryBuildingCode = x.InventoryBuildingName,
                                            InventoryBuildingDesc = x.InventoryBuildingName
                                        }).ToList();

                var condition = await _inventoryItemConditionRepository.ReadAsync(new List<int> { (int)input.ConditionId});

                foreach (var inventory in invent)
                {
                    var obj = await ParseSimpleInventoryWithFirstItemLocationInfo(inventory, inventoryBuildings);
                    obj.ConditionName = condition?.FirstOrDefault()?.ConditionName;
                    if (obj != null)
                        result.Add(obj);
                }
            }

            return result;
        }
        #region hangfire
        public async Task InsertInventoryToHistory(InventoryHistoryViewModel entity, int userId, string api, string description)
        {
            await _historyTableRepository.InsertToInventoryHistory(entity, "Inventory", entity.InventoryId, api, userId, description);
        }

        public async Task UpdateInventoryToHistory(InventoryHistoryViewModel oldValue, InventoryHistoryViewModel newValue,
            string api, string description)
        {
            await _historyTableRepository.UpdateToInventoryHistory(oldValue, newValue, "Inventory", oldValue.InventoryId, api, newValue.UpdateId, description);
        }

        public async Task UpdateInventoriesToHistory(List<InventoryHistoryViewModel> oldValues, List<InventoryHistoryViewModel> newValues,
            string api, string description)
        {
            for (int i = 0; i < oldValues.Count(); i++)
            {
                await UpdateInventoryToHistory(oldValues[i], newValues[i], api, description);
            }
        }
        #endregion

        #region Private Methods

        public IQueryable<Inventory> Filter(int? clientId = null, List<int> ids = null, int? itemTypeId = null, string searchImage = null,
            List<int> itemRowIds = null, DateTime? deviceDate = null, List<int?> submissionIds = null, string searchString = null)
        {
            var query = GetAll();
            if (clientId.HasValue)
            {
                query = query.Where(x => x.ClientId == clientId);
            }

            if (ids?.Any() == true)
            {
                query = query.Where(x => ids.Contains(x.InventoryId));
            }
            if (itemTypeId.HasValue && itemTypeId > 0)
            {
                query = query.Where(x => x.ItemTypeId == itemTypeId);
            }
            if (!string.IsNullOrWhiteSpace(searchImage))
            {
                query = query.Where(x => EF.Functions.Like(x.MainImage, $"%{searchImage}%"));
            }
            if (itemRowIds?.Any() == true)
            {
                query = query.Where(x => itemRowIds.Contains(x.ItemRowId));
            }
            if (deviceDate.HasValue)
            {
                query = query.Where(x => EF.Functions.DateDiffMinute(x.DeviceDate, deviceDate) == 0);
            }
            if (submissionIds?.Any(x => x != null) == true)
            {
                query = query.Where(x => submissionIds.Contains(x.SubmissionId));
            }
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x => EF.Functions.Like(x.PartNumber, $"%{searchString}%")
                                      || EF.Functions.Like(x.Description, $"%{searchString}%")
                                      || EF.Functions.Like(x.Tag, $"%{searchString}%")
                                      || EF.Functions.Like(x.ItemType.ItemTypeName, $"%{searchString}%"));
            }

            return query.OrderByDescending(x => x.InventoryId);
        }

        private async Task<DataItemType> ParseInventorySearchResult(Inventory inventory, string uploadedImagePath = null,
            bool usedAws = true, string baseUrl = "", bool includeTotalCountImages = false, bool includeQRcode = false,
            bool includeQuanlity = false, bool isSearch = false, int? inventoryItemId = null, bool forEdit = false, bool includeInventoryItem = true)
        {
            return new DataItemType
            {
                ItemTypeID = inventory.ItemTypeId,
                ClientId = inventory.ClientId,
                ItemCode = inventory.ItemCode,
                InventoryRowId = inventory.ItemRowId,
                InventoryId = inventory.InventoryId,
                //QRcode = string.IsNullOrWhiteSpace(inventory.QRcode) ? "" : $"{baseUrl}/{inventory.QRcode}",
                MainImage = !string.IsNullOrWhiteSpace(inventory.MainImage) ? (usedAws ? inventory.MainImage : $"{baseUrl}/{uploadedImagePath}/{inventory.MainImage}") : "",
                ItemTypeOptions = await ParseInventoryToOptions(inventory, baseUrl, includeTotalCountImages, includeQRcode, includeQuanlity, isSearch, inventoryItemId: inventoryItemId, forEdit: forEdit, includeInventoryItem)
            };
        }

        private async Task<List<ItemTypeOptionApiModel>> ParseInventoryToOptions(Inventory inventory, string baseUrl = "",
            bool includeTotalCountImages = false, bool includeQRcode = false, bool includeQuanlity = false, bool isSearch = false, int? inventoryItemId = null,
            bool forEdit = false, bool includeInventoryItem = true)
        {
            var manufacturer = _dbContext.Manufacturers.FirstOrDefault(x => x.ManufacturerId == inventory.ManufacturerId);
            var itemTypeOptions = await GetItemTypeOptions(inventory.ItemTypeId, inventory.ClientId);
            var result = new List<ItemTypeOptionApiModel>
            {
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "Description",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "Description"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Description })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "Manufacturer",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "Manufacturer"),
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
                    ItemTypeOptionCode = "Custom-Modular-AV",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "Custom-Modular-AV"),
                    ItemTypeOptionReturnValue = ParseSelectedMultipleValues(new List<object> { inventory.Modular })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "PartNumber",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "PartNumber"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.PartNumber })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "Finish",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "Finish"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Finish, inventory.Finish2 })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "Fabric",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "Fabric"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Fabric, inventory.Fabric2 })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "Unit",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "Unit"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Unit })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "Height",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "Height"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Height })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "Width",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "Width"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Width })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "TopFinish",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "TopFinish"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Top })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "EdgeFinish",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "EdgeFinish"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Edge })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "BaseFinish",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "BaseFinish"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Base })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "Depth",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "Depth"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Depth })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "Diameter",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "Diameter"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Diameter })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "FrameFinish",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "FrameFinish"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Frame })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "BackFinish",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "BackFinish"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Back })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "SeatFinish",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "SeatFinish"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Seat })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "SeatHeight",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "SeatHeight"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.SeatHeight })
                },
                new ItemTypeOptionApiModel
                {
                    ItemTypeOptionCode = "Tag",
                    ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "Tag"),
                    ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { inventory.Tag })
                }
            };

            if (includeInventoryItem)
            {
                var inventoryItems = await _dbContext.InventoryItem.Where(x => x.InventoryId == inventory.InventoryId)
                                                                       .WhereIf(isSearch, x => x.SubmissionId == inventory.SubmissionId)
                                                                       .ToListAsync();
                if (inventoryItems?.Any() == true)
                {
                    var firstInventoryItem = inventoryItems.WhereIf(inventoryItemId.HasValue, x => x.InventoryItemId == inventoryItemId).FirstOrDefault();
                    var building = (await _buildingService.GetBuildings(inventory.ClientId))
                        .Where(x => x.InventoryBuildingId == firstInventoryItem.InventoryBuildingId)
                        .FirstOrDefault();

                    var floor = _dbContext.InventoryFloors.FirstOrDefault(x => x.InventoryFloorId == firstInventoryItem.InventoryFloorId);
                    result.Add(new ItemTypeOptionApiModel
                    {
                        ItemTypeOptionCode = "Building",
                        ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "Building"),
                        ItemTypeOptionReturnValue = ParseDropdownSelectedValue(new List<SelectItemOptionModel>
                        {
                            new SelectItemOptionModel
                            {
                                ReturnValue =  building?.InventoryBuildingName,
                                ReturnID = firstInventoryItem.InventoryBuildingId,
                                ReturnCode = building?.InventoryBuildingName,
                                ReturnDesc = building?.InventoryBuildingName,
                                ReturnName = building?.InventoryBuildingName
                            }
                        })
                    });
                    result.Add(new ItemTypeOptionApiModel
                    {
                        ItemTypeOptionCode = "Floor",
                        ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "Floor"),
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
                        ItemTypeOptionCode = "GPS",
                        ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "GPS"),
                        ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { firstInventoryItem.Gpslocation })
                    });
                    result.Add(new ItemTypeOptionApiModel
                    {
                        ItemTypeOptionCode = "AreaOrRoom",
                        ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "AreaOrRoom"),
                        ItemTypeOptionReturnValue = ParseSelectedValue(new List<object> { firstInventoryItem.Room })
                    });

                    result.Add(new ItemTypeOptionApiModel
                    {
                        ItemTypeOptionCode = "TotalCount",
                        ItemTypeOptionName = GetItemTypeOptionByCode(itemTypeOptions, "TotalCount"),
                        ItemTypeOptionReturnValue = includeTotalCountImages ? (isSearch ? await ParseInventoryItemImagesForSearch(inventoryItems, baseUrl, includeQRcode, includeQuanlity) : await ParseInventoryItemImages(inventoryItems, baseUrl, includeQRcode, includeQuanlity, forEdit)) : null
                    });

                    result.Add(new ItemTypeOptionApiModel
                    {
                        ItemTypeOptionCode = "QRcode",
                        ItemTypeOptionName = "QRcode",
                        ItemTypeOptionReturnValue = ParseInventoryItemQRcodes(inventoryItems)
                    });
                }
            }
            return result;
        }

        private async Task<IEnumerable<Tuple<string, string>>> GetItemTypeOptions(int? itemTypeId, int? clientId)
        {
            if (!itemTypeId.HasValue || !clientId.HasValue) return null;

            var itemTypeOptions = await _dbContext.ItemTypeOptions.Where(x => x.ItemTypeId == itemTypeId && x.ClientId == clientId).ToListAsync();
            return itemTypeOptions.Count == 0
                ? null
                : itemTypeOptions.Select(x => new Tuple<string, string>(x.ItemTypeOptionCode, x.ItemTypeOptionName));
        }

        private async Task<DataInventoryType> ParseSimpleInventorySearchResult(Inventory inventory)
        {
            var itemType = _dbContext.ItemTypes.Where(x => x.ItemTypeId == inventory.ItemTypeId && (x.ClientId == inventory.ClientId || x.CreateId == 1)).FirstOrDefault();
            if (itemType == null) return null;

            var inventoryItem = _dbContext.InventoryItem.Where(x => x.InventoryId == inventory.InventoryId);
            return new DataInventoryType
            {
                inventoryId = inventory.InventoryId,
                clientId = inventory.ClientId,
                mainImage = !string.IsNullOrWhiteSpace(inventory.MainImage) ? inventory.MainImage : "",
                itemTypeId = inventory.ItemTypeId,
                itemTypeCode = itemType.ItemTypeCode,
                itemTypeName = itemType.ItemTypeName,
                description = inventory.Description,
                manufacturerName = inventory.ManufacturerName,
                tag = inventory.Tag,
                GlobalProductCatalogID = inventory.GlobalProductCatalogID,
                QRCode = inventory.QRCode,
                WarrantyYears = inventory.WarrantyYears,
                totalQuantity = await inventoryItem.CountAsync(),

            };
        }

        private async Task<DataInventoryTypeWithFirstItemLocationInfo> ParseSimpleInventoryWithFirstItemLocationInfo(Inventory inventory,
            List<InventoryBuildingModel> inventoryBuildings)
        {
            var itemType = _dbContext.ItemTypes.Where(x => x.ItemTypeId == inventory.ItemTypeId && (x.ClientId == inventory.ClientId || x.CreateId == 1)).FirstOrDefault();
            if (itemType == null) return null;

            var inventoryItems = _dbContext.InventoryItem.Where(x => x.InventoryId == inventory.InventoryId);
            var totalQuantity = await inventoryItems.CountAsync();
            var firstInventoryItem = await inventoryItems.FirstOrDefaultAsync();
            return new DataInventoryTypeWithFirstItemLocationInfo
            {
                inventoryId = inventory.InventoryId,
                clientId = inventory.ClientId,
                mainImage = !string.IsNullOrWhiteSpace(inventory.MainImage) ? inventory.MainImage : "",
                itemTypeId = inventory.ItemTypeId,
                itemTypeCode = itemType.ItemTypeCode,
                itemTypeName = itemType.ItemTypeName,
                description = inventory.Description,
                manufacturerName = inventory.ManufacturerName,
                tag = inventory.Tag,
                GlobalProductCatalogID = inventory.GlobalProductCatalogID,
                QRCode = inventory.QRCode,
                WarrantyYears = inventory.WarrantyYears,
                totalQuantity = totalQuantity,
                Floor = totalQuantity == 1 ? firstInventoryItem?.InventoryFloor?.InventoryFloorDesc: String.Empty,
                Room = totalQuantity == 1 ? firstInventoryItem?.Room : String.Empty,
                Building = totalQuantity == 1 ? inventoryBuildings.Find(x => x.InventoryBuildingId == firstInventoryItem?.InventoryBuildingId)?.InventoryBuildingName : String.Empty,
            };
        }

        private string GetItemTypeOptionByCode(IEnumerable<Tuple<string, string>> itemTypeOptions, string itemTypeOptionCode)
        {
            return string.IsNullOrWhiteSpace(itemTypeOptionCode) || itemTypeOptions?.Any() != true
                ? string.Empty
                : (itemTypeOptions.FirstOrDefault(x => x.Item1.Equals(itemTypeOptionCode))?.Item2);
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

        private async Task<object> ParseInventoryItemImages(List<InventoryItem> inventoryItems, string baseUrl = "", bool includeQRcode = false, bool includeQuanlity = false, bool forEdit = false)
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

                var images = await _dbContext.InventoryImages.Where(x => x.InventoryItemId == item.InventoryItemId).ToListAsync();

                if (includeQuanlity)
                {
                    var inventoryCount = await _dbContext.Inventory.CountAsync(x => x.InventoryId == item.InventoryId);
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
                //if (images.Count == 0)
                //{
                //    continue;
                //}

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

        private async Task<object> ParseInventoryItemImagesForSearch(List<InventoryItem> inventoryItems, string baseUrl = "", bool includeQRcode = false, bool includeQuanlity = false)
        {
            if (inventoryItems.Count < 1) return new List<InventorySearchingModel>();

            var data = new List<InventorySearchingModel>();
            var groupInventoryItems = inventoryItems.GroupBy(x => new { x.InventoryId, x.ClientId, x.InventoryBuildingId, x.InventoryFloorId, x.Room, x.ConditionId })
                .Select(x => new
                {
                    x.Key.InventoryId,
                    x.Key.ClientId,
                    x.Key.InventoryBuildingId,
                    x.Key.InventoryFloorId,
                    x.Key.Room,
                    x.Key.ConditionId
                }).ToList();
            var inventories = await _dbContext.Inventory.Where(x => inventoryItems.Select(y => y.InventoryId).Distinct().Contains(x.InventoryId))
                                                        .ToListAsync();
            var inventoryItemIds = inventoryItems.Select(y => y.InventoryItemId).Distinct().ToList();
            var inventoryItemImages = await _dbContext.InventoryImages.Where(x => x.InventoryItemId.HasValue
                                                                               && inventoryItemIds.Contains(x.InventoryItemId.Value))
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

                var inventoryItemBarcodes = new List<string>();
                foreach (var inventoryItem in selectedInventoryItems)
                {
                    if (includeQRcode && !string.IsNullOrWhiteSpace(inventoryItem.Barcode))
                    {
                        inventoryItemBarcodes.Add($"{inventoryItem.Barcode}");
                    }
                    var images = inventoryItemImages.Where(x => x.InventoryItemId == inventoryItem.InventoryItemId).ToList();

                    var existingCondition = data.FirstOrDefault(x => x.NameCondition == $"{item.ConditionId}_{index}");
                    if (existingCondition is null)
                    {
                        data.Add(new InventorySearchingModel
                        {
                            NameCondition = $"{item.ConditionId}_{index}",
                            DataCondition = new List<DataConditionModel>
                            {
                                new DataConditionModel
                                {
                                    Url = ConvertInventoryItemImagesToObject(images),
                                    InventoryItemBarcodes = inventoryItemBarcodes,
                                    TxtQuantity = selectedInventoryItems.Count()
                                }
                            }
                        });
                    }
                    else
                    {
                        var condition = existingCondition.DataCondition.First();
                        condition.Url.AddRange(ConvertInventoryItemImagesToObject(images));
                        condition.InventoryItemBarcodes = inventoryItemBarcodes;
                    }
                }
                index++;
            }

            var result = new List<object>();
            foreach (var item in data)
            {
                var condition = item.NameCondition.Split("_")[0];
                var urls = item.DataCondition.Where(x => x.Url?.Any() == true).Select(x => x.Url).ToList();
                var imageCount = urls.SelectMany(x => x).Count();
                if (includeQRcode)
                {
                    result.Add(new
                    {
                        nameCondition = condition,
                        dataCondition = new
                        {
                            url = urls,
                            imageCounter = imageCount,
                            txtQuanlity = item.DataCondition.Sum(x => x.TxtQuantity),
                            inventoryItemBarcodes = item.DataCondition.Select(x => x.InventoryItemBarcodes).ToArray()
                        }
                    });
                }
                else
                {
                    result.Add(new { nameCondition = condition, dataCondition = new { url = urls, imageCounter = imageCount } });
                }
            }
            return result;
        }

        private object ParseInventoryItemQRcodes(List<InventoryItem> inventoryItems)
        {
            if (inventoryItems.Count == 0)
                return null;

            var qrCodes = new List<Dictionary<string, object>>();
            foreach (var item in inventoryItems)
            {
                if (!string.IsNullOrWhiteSpace(item.Qrcode))
                {
                    qrCodes.Add(new Dictionary<string, object>() { { item.InventoryItemId.ToString(), item.Qrcode } });
                }
            }

            return qrCodes.Count > 0 ? new Dictionary<string, object>() { { "returnValue", qrCodes } } : null;
        }

        private List<InventoryImageResponseModel> ConvertInventoryItemImagesToObject(List<InventoryImages> images,
            bool forEdit = false)
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

        private async Task SaveManufacturer(string name)
        {
            await _manufacturerRepository.CreateAsync(new Share.Models.Dto.Manufactory.CreateOrEditManufacturerModel
            {
                ManufacturerName = name
            });
        }

        #endregion
    }
}
