using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SSInventory.Business.Interfaces;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SSInventory.Share.Models.Dto.InventoryItem;
using SSInventory.Share.Ultilities;
using SSInventory.Share.Models.Dto.ItemTypeSupportFiles;
using SSInventory.Share.Models.Dto.Inventory;
using Microsoft.AspNetCore.Hosting;
using SSInventory.Web.Helpers;
using SSInventory.Web.Services.Aws;
using System.IO;
using SSInventory.Share.Models.Dto.ItemTypeOptionLines;
//using Hangfire;
using AutoMapper;
using SSInventory.Core.Models;

namespace SSInventory.Web.Controllers.Apis
{
    /// <summary>
    /// Inventory API
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InventoryController : CommonController
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly IInventoryService _inventoryService;
        private readonly IInventoryItemService _inventoryItemService;
        private readonly IItemTypeService _itemTypeService;
        private readonly IDapperService _dapperService;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FileService _fileService;
        private readonly string UploadTempFolder;
        private readonly string UploadOriginFolder;
        private readonly IMapper _mapper;
        private readonly string _apiName;

        /// <summary>
        /// Inventory constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inventoryService"></param>
        /// <param name="inventoryItemService"></param>
        /// <param name="config"></param>
        /// <param name="dapperService"></param>
        /// <param name="webHostEnvironment"></param>
        /// <param name="aws3Service"></param>
        public InventoryController(ILogger<InventoryController> logger,
            IInventoryService inventoryService,
            IInventoryItemService inventoryItemService,
            IConfiguration config,
            IDapperService dapperService,
            IWebHostEnvironment webHostEnvironment,
            IItemTypeService itemTypeService,
            FileService aws3Service,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _inventoryService = inventoryService;
            _inventoryItemService = inventoryItemService;
            _dapperService = dapperService;
            _config = config;
            _fileService = aws3Service;
            _webHostEnvironment = webHostEnvironment;
            _itemTypeService = itemTypeService;
            UploadTempFolder = _config["ssSettings:UploadTempFolder"];
            UploadOriginFolder = _config["ssSettings:UploadOriginFolder"];
            _mapper = mapper;
            _apiName = $"{httpContextAccessor.HttpContext?.Request?.Scheme}://{httpContextAccessor.HttpContext?.Request?.Host}{httpContextAccessor.HttpContext?.Request?.PathBase}{httpContextAccessor.HttpContext?.Request?.Path}";
        }

        /// <summary>
        /// Get inventory info
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="inventoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInventoryInfo(int clientId, int inventoryId)
        {
            if (clientId < 1 || inventoryId < 1)
                return BadRequest("Invalid data");

            try
            {
                var dataItemTypes = await _inventoryService.SearchInventory(new SearchImageInfo { ClientId = clientId, InventoryId = inventoryId },
                    UploadOriginFolder,
                    baseUrl: $"{Request.Scheme}://{Request.Host}{Request.PathBase}",
                    includeQRcode: true,
                    isSearch: true, includeInventoryItem: true, includeConditionData: false,
                    searchAPIName: "GetInventoryInfo");

                if (dataItemTypes?.Count > 0)
                {
                    var itemTypeOptionSets = await GetItemTypeOptionSet(dataItemTypes.First(), 2, new InventoryItemModel
                    {
                        InventoryId = inventoryId,
                        ClientId = clientId
                    });

                    return Ok(itemTypeOptionSets);
                }

                return NotFound("No data found");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get inventory item information
        /// </summary>
        /// <param name="inventoryItemId"></param>
        /// <param name="clientId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="404">Data not found</response>
        /// <response code="500">Error occurred while getting image information</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInventoryItemInfo(int inventoryItemId, int clientId)
        {
            if (inventoryItemId < 1 || inventoryItemId > int.MaxValue)
                return BadRequest("Invalid data");

            if (clientId < 1 || clientId > int.MaxValue)
                return BadRequest("Invalid client ID");

            try
            {
                var filter = new InventoryItemFilterModel
                {
                    Ids = new List<int> { inventoryItemId },
                    ClientId = clientId
                };
                var inventoryItems = await _inventoryItemService.ReadAsync(filter);
                if (inventoryItems?.Any() != true)
                    return NotFound("Inventory item is not found");

                var inventoryItem = inventoryItems[0];
                var dataItemTypes = await _inventoryService.SearchInventory(new SearchImageInfo
                //var dataItemTypes = await _inventoryService.GetInventory(new SearchImageInfo
                {
                    InventoryId = inventoryItem.InventoryId,
                    ClientId = inventoryItem.ClientId,
                    InventoryItemId = inventoryItem?.InventoryItemId
                }, UploadOriginFolder, baseUrl: $"{Request.Scheme}://{Request.Host}{Request.PathBase}", includeTotalCountImages: true, forEdit: true, includeQRcode: true, searchAPIName: "GetInventoryItemInfo");
                if (dataItemTypes.Count > 0)
                {
                    var itemTypeOptionSets = await GetItemTypeOptionSet(dataItemTypes.First(), 2, inventoryItem);

                    return Ok(itemTypeOptionSets);
                }

                return NotFound("No data found");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update Inventory
        /// </summary>
        /// <param name="input"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="500">Error while processing data</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInventory([FromBody] DataItemType input)
        {
            try
            {
                var inventory = (await _inventoryService.ReadAsync(new SearchInventoryModel { ClientId = input.ClientId, Ids = new List<int> { input.InventoryId } })).FirstOrDefault();

                if (inventory is null)
                    return BadRequest("Data is not found");
                var oldValue = _mapper.Map<InventoryHistoryViewModel>(inventory);

                foreach (var option in input.ItemTypeOptions)
                {
                    if (option.ItemTypeOptionReturnValue is null) continue;

                    option.CollectInventoryInfoFromJson(inventory);
                }

                inventory.UpdateDateTime = DateTime.Now;
                inventory.ItemTypeId = input.ItemTypeID;
                if (!string.IsNullOrWhiteSpace(input.ItemCode))
                {
                    inventory.ItemCode = input.ItemCode;
                }

                if (!string.IsNullOrWhiteSpace(input.MainImage))
                {
                    inventory.MainImage = input.MainImage;
                    var existingImageInTemp = CheckExistingImageInTempFolder(input.MainImage);
                    if (existingImageInTemp)
                    {
                        var usedAws = _config.GetValue<bool>("ssSettings:UseAWS");
                        RenameAndMoveFileFromTempToOrigin(input.MainImage, input.MainImage, UploadTempFolder, UploadOriginFolder);
                        if (usedAws)
                        {
                            await _fileService.UploadFilesToAWS(new List<Tuple<int, string>>
                            {
                                new Tuple<int, string>(input.ClientId.Value, @$"{_webHostEnvironment.ContentRootPath}/{Path.Combine(UploadOriginFolder, input.MainImage)}")
                            });
                        }
                    }
                }
                var newValue = _mapper.Map<InventoryHistoryViewModel>(inventory);

                await _inventoryService.UpdateAsync(new List<InventoryModel> { inventory });
                await _inventoryService.UpdateInventoryToHistory(oldValue, newValue, _apiName, null);
                return Ok("Updated data successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return Problem("Error while processing data" + ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get simple inventory information
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="404">Data not found</response>
        /// <response code="500">Error occurred while getting image information</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchSimpleInventories(SearchSimpleInventoryRequestModel input)
        {

            if (input.ClientId < 1 || input.ClientId > int.MaxValue)
                return BadRequest("Invalid client ID");
            if (input.CurrentPage < 1)
                input.CurrentPage = 1;
            if (input.ItemsPerPage > 500)
                input.ItemsPerPage = 500;
            if (input.ConditionId == null)
                return BadRequest("ConditionId is required");
            try
            {
                var result = new List<DataInventoryType>();

                var dataItemTypes = await _inventoryService.SearchSimpleInventorySearch(new SearchSimpleInventoryRequestModel
                {
                    ClientId = input.ClientId,
                    SearchString = input.SearchString,
                    ItemTypeId = input.ItemTypeId,
                    CurrentPage = input.CurrentPage,
                    ConditionId = input.ConditionId,
                    Room = input.Room,
                    ItemsPerPage = input.ItemsPerPage
                });
                result.AddRange(dataItemTypes);

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Get simple inventory information with first item location info
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="404">Data not found</response>
        /// <response code="500">Error occurred while getting image information</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSimpleInventoriesWithFirstItemLocationInfo(SearchSimpleInventoryRequestModel input)
        {

            if (input.ClientId < 1 || input.ClientId > int.MaxValue)
                return BadRequest("Invalid client ID");
            if (input.CurrentPage < 1)
                input.CurrentPage = 1;
            if (input.ItemsPerPage > 500)
                input.ItemsPerPage = 500;
            if (input.ConditionId == null)
                return BadRequest("ConditionId is required");
            try
            {
                var result = new List<DataInventoryTypeWithFirstItemLocationInfo>();

                var dataItemTypes = await _inventoryService.GetSimpleInventoriesWithFirstItemLocationInfo(new SearchSimpleInventoryRequestModel
                {
                    ClientId = input.ClientId,
                    SearchString = input.SearchString,
                    ItemTypeId = input.ItemTypeId,
                    CurrentPage = input.CurrentPage,
                    ConditionId = input.ConditionId,
                    Room = input.Room,
                    ItemsPerPage = input.ItemsPerPage
                });
                result.AddRange(dataItemTypes);

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        #region Private methods

        private async Task<List<ItemTypeModel>> GetItemTypeOptionSet(DataItemType dataItemType, int clientGroupId, InventoryItemModel inventoryItem = null)
        {
            try
            {
                var data = await _itemTypeService.GetItemTypeOptionSetAsync(dataItemType.ClientId, dataItemType.ItemTypeID);
                return await ToItemTypesModel(data, dataItemType: dataItemType, inventoryItem: inventoryItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        private async Task<List<ItemTypeModel>> ToItemTypesModel(List<ItemTypeMappingModel> data, DataItemType dataItemType, InventoryItemModel inventoryItem = null)
        {
            await Task.CompletedTask;
            var itemTypeIds = data.Select(x => x.ItemTypeID).Distinct();
            var result = new List<ItemTypeModel>();
            foreach (var itemTypeId in itemTypeIds)
            {
                var itemType = data.FirstOrDefault(x => x.ItemTypeID == itemTypeId);

                itemType.ClientId = (int)dataItemType.ClientId;
                var itemTypeModel = ConvertToItemTypeModel(itemType, inventoryItem: inventoryItem, dataItemType: dataItemType);
                foreach (var itemTypeOptionId in data.Where(x => x.ItemTypeID == itemTypeId).Select(x => x.ItemTypeOptionID).Distinct())
                {
                    var option = data.Where(x => x.ItemTypeOptionID == itemTypeOptionId).FirstOrDefault();
                    if (option is null) continue;

                    var itemTypeOption = ConvertToItemTypeOptionModel(option, itemTypeOptionId);
                    itemTypeOption.ItemTypeOptionReturnValue = GetItemTypeOptionReturnValue(dataItemType.ItemTypeOptions, option.ItemTypeOptionCode);

                    foreach (var optionLine in data.Where(x => x.ItemTypeID == itemTypeId && x.ItemTypeOptionID == itemTypeOptionId))
                    {
                        if (optionLine.ItemTypeOptionLineID != 0)
                        {
                            itemTypeOption.ItemTypeOptionLines.Add(new ItemTypeOptionLineModel
                            {
                                ItemTypeOptionLineID = optionLine.ItemTypeOptionLineID,
                                ItemTypeOptionLineName = optionLine.ItemTypeOptionLineName,
                                ItemTypeOptionLineCode = optionLine.ItemTypeOptionLineCode,
                                ItemTypeOptionID = itemTypeOptionId,
                                InventoryUserAcceptanceRulesRequired = optionLine.InventoryUserAcceptanceRulesRequired
                            });
                        }
                    }
                    itemTypeModel.ItemTypeOptions.Add(itemTypeOption);
                }
                result.Add(itemTypeModel);
            }

            return result;
        }

        private ItemTypeModel ConvertToItemTypeModel(ItemTypeMappingModel itemType, InventoryItemModel inventoryItem = null, DataItemType dataItemType = null)
        {
            var model = new ItemTypeModel
            {
                ItemTypeId = itemType.ItemTypeID,
                ClientId = itemType.ClientId,
                ItemTypeName = itemType.ItemTypeName,
                ItemTypeCode = itemType.ItemTypeCode.ConvertItemTypeName(itemType.ItemTypeName),
                StatusId = inventoryItem?.StatusId ?? 5,
                Condition = inventoryItem?.Condition,
                NoteForItem = inventoryItem?.DamageNotes,
                InventoryId = inventoryItem?.InventoryId,
                InventoryItemId = inventoryItem?.InventoryItemId,
                GlobalProductCatalogID = dataItemType?.GlobalProductCatalogID,
                WarrantyYears = dataItemType == null ? 1 : dataItemType.WarrantyYears,
                QRcode = dataItemType?.QRcode,
                AddedToCartItem = inventoryItem != null ? inventoryItem.AddedToCartItem : false,
                ItemTypeAdditionalOption = new ItemTypeAdditionalOptionsModel
                {
                    PoOrderDate = inventoryItem?.PoOrderDate,
                    PoOrderNumber = inventoryItem?.PoOrderNo
                },
                ItemTypeOptions = new List<ItemTypeOptionModel>()
            };

            if (!string.IsNullOrWhiteSpace(dataItemType?.MainImage))
            {
                model.MainImage = dataItemType.MainImage;
            }

            return model;
        }

        private ItemTypeOptionModel ConvertToItemTypeOptionModel(ItemTypeMappingModel option, int itemTypeOptionId)
        {
            return new ItemTypeOptionModel
            {
                ItemTypeOptionId = itemTypeOptionId,
                ItemTypeOptionName = option.ItemTypeOptionName,
                ItemTypeOptionCode = option.ItemTypeOptionCode,
                OrderSequence = option.OrderSequence,
                FieldType = option.FieldType,
                ValType = option.ValType,
                ItemTypeOptionDesc = option.ValTypeDesc,
                LimitMin = option.LimitMin ?? 0,
                LimitMax = option.LimitMax ?? 0,
                IsRequired = option.IsRequired,
                IsHide = option.IsHide,
                ItemTypeAttributeId = option.ItemTypeAttributeId,
                ItemTypeOptionLines = new List<ItemTypeOptionLineModel>(),
                InventorySupportFile = option.ItemTypeSupportFileID == 0 ? null : new ItemTypeSupportFileModel
                {
                    ItemTypeSupportFileId = option.ItemTypeSupportID,
                    Desc = option.SupportFileDesc,
                    FileName = option.SupportFileName,
                    FilePath = option.SupportFilePath
                }
            };
        }

        private object GetItemTypeOptionReturnValue(List<ItemTypeOptionApiModel> itemTypeOptionReturnValues, string itemTypeOptionCode)
        {
            if (string.IsNullOrWhiteSpace(itemTypeOptionCode) || itemTypeOptionReturnValues.Count == 0)
                return "";

            var selectedValue = itemTypeOptionReturnValues.FirstOrDefault(x => x.ItemTypeOptionCode.Equals(itemTypeOptionCode, StringComparison.CurrentCultureIgnoreCase));
            return selectedValue is null ? "" : selectedValue.ItemTypeOptionReturnValue;
        }

        private bool RenameAndMoveFileFromTempToOrigin(string tempFileName, string originFileName, string tempPath, string originPath)
        {
            var sourcePath = @$"{_webHostEnvironment.ContentRootPath}/{Path.Combine(tempPath, tempFileName)}";
            if (System.IO.File.Exists(sourcePath))
            {
                CreateFolder(originPath);
                var destinationPath = @$"{_webHostEnvironment.ContentRootPath}/{Path.Combine(originPath, originFileName)}";
                System.IO.File.Move(sourcePath, destinationPath, true);

                return true;
            }

            return false;
        }

        private void CreateFolder(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        private bool CheckExistingImageInTempFolder(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return false;
            var sourcePath = @$"{_webHostEnvironment.ContentRootPath}/{Path.Combine(UploadTempFolder, fileName)}";

            var existed = System.IO.File.Exists(sourcePath);
            if (!existed)
            {
                _logger.LogDebug($"The file name {fileName} is not found in the temporary {UploadTempFolder} folder");
            }

            return existed;
        }

        #endregion
    }
}
