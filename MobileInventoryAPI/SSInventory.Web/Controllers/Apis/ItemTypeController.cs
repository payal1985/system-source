using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSInventory.Business.Interfaces;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryItem;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Share.Models.Dto.Submission;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SSInventory.Share.Ultilities;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using SSInventory.Web.Services.Aws;
using SSInventory.Web.Models;
using SSInventory.Share.Models.Search;
using SSInventory.Web.Models.QRcode;
using SSInventory.Share.Models;
using SSInventory.Web.Utilities;
using SSInventory.Share.Enums;
using Microsoft.Extensions.Caching.Memory;
using SSInventory.Web.Services.Email;
using SSInventory.Web.Models.Email;
using System.Diagnostics;
using SSInventory.Web.Helpers;
using SSInventory.Share.Models.Dto.InventoryImage;
using SSInventory.Share.Constants;
using SSInventory.Web.Constants;
namespace SSInventory.Web.Controllers
{
    /// <summary>
    /// ItemType controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemTypeController : CommonController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ItemTypeController> _logger;
        private readonly IDapperService _dapperService;
        private readonly IMapper _mapper;
        private readonly ISubmissionService _submissionService;
        private readonly IInventoryService _inventoryService;
        private readonly IInventoryItemService _inventoryItemService;
        private readonly IInventoryImageService _inventoryItemImageService;
        private readonly IConfiguration _config;
        private readonly IItemTypeService _itemTypeService;
        private readonly IItemTypeOptionSetService _itemTypeOptionSetService;
        private readonly IInventoryItemConditionService _inventoryItemConditionService;
        private readonly IInventoryBuildingsService _inventoryBuildingsService;
        private readonly FileService _fileService;
        private readonly IMemoryCache _memoryCache;

        private readonly string UploadOriginFolder;
        private readonly string UploadTempFolder;

        /// <summary>
        /// ItemType controller contructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dapperService"></param>
        /// <param name="webHostEnvironment"></param>
        /// <param name="mapper"></param>
        /// <param name="submissionService"></param>
        /// <param name="inventoryService"></param>
        /// <param name="inventoryItemService"></param>
        /// <param name="inventoryItemImageService"></param>
        /// <param name="config"></param>
        /// <param name="itemTypeService"></param>
        /// <param name="itemTypeOptionSetService"></param>
        /// <param name="fileService"></param>
        /// <param name="memoryCache"></param>
        /// <param name="inventoryItemConditionService"></param>
        /// <param name="inventoryBuildingsService"></param>
        public ItemTypeController(ILogger<ItemTypeController> logger, IDapperService dapperService, IWebHostEnvironment webHostEnvironment,
            IMapper mapper, ISubmissionService submissionService, IInventoryService inventoryService, IInventoryItemService inventoryItemService,
            IInventoryImageService inventoryItemImageService, IConfiguration config, IItemTypeService itemTypeService,
            IItemTypeOptionSetService itemTypeOptionSetService, FileService fileService, IMemoryCache memoryCache,
            IInventoryItemConditionService inventoryItemConditionService,
            IInventoryBuildingsService inventoryBuildingsService)
        {
            _logger = logger;
            _dapperService = dapperService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _submissionService = submissionService;
            _inventoryService = inventoryService;
            _inventoryItemService = inventoryItemService;
            _inventoryItemImageService = inventoryItemImageService;
            _config = config;
            _itemTypeService = itemTypeService;
            _itemTypeOptionSetService = itemTypeOptionSetService;
            _inventoryItemConditionService = inventoryItemConditionService;
            _inventoryBuildingsService = inventoryBuildingsService;
            _fileService = fileService;
            _memoryCache = memoryCache;

            UploadOriginFolder = _config["ssSettings:UploadOriginFolder"];
            UploadTempFolder = _config["ssSettings:UploadTempFolder"];
        }

        /// <summary>
        /// Get list item type
        /// </summary>
        /// <param name="clientId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItemTypes(int clientId)
        {
            try
            {
                if (clientId < 1)
                {
                    return BadRequest("Invalid client");
                }
                var data = (await _itemTypeService.ReadAsync(clientId))
                                            .OrderBy(x => x.ItemTypeName)
                                            .ToList();
                return Ok(data);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get item type option set
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="itemId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItemTypeOptionSet(int clientId, int itemId)
        {
            try
            {
                if (clientId < 1)
                    return BadRequest("Invalid client");


                var data = await _itemTypeService.GetItemTypeOptionSetAsync(clientId, itemId);
                var result = ConvertToItemTypesModel(data);

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Process Main Json
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="clientId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="409">Existed submission</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProcessJsonPost([FromBody] List<ItemTypeApiModel> jsonData, int clientId)
        {
            if (clientId < 1)
            {
                return BadRequest("Client Id is required");
            }
            var submissionId = 0;
            try
            {
                if (jsonData is null)
                {
                    return Problem("Invalid json data", statusCode: StatusCodes.Status400BadRequest);
                }

                var sendEmailSubmission = _config.GetValue<bool>("EmailConfigure:Enable");
                var exportSubmissions = new List<ExportSubmissionModel>();
                var inventories = new List<InventoryModel>();
                var updatedInventoryItems = new List<InventoryItemModel>();
                var inventoryItemImages = new List<InventoryImageModel>();
                var submissionDate = DateTime.Now;
                var deviceDate = DateTime.Now;
                string parsedJsonData = System.Text.Json.JsonSerializer.Serialize(jsonData);
                var inventoryItemConditions = await _inventoryItemConditionService.ReadAsync();
                // Process input data
                foreach (var data in jsonData)
                {
                    var exportSubmissionModel = new ExportSubmissionModel();
                    var submission = _mapper.Map<SubmissionModel>(data);
                    deviceDate = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(submission.DeviceDate), TimeZoneInfo.Utc);
                   
                    //DeviceDate not existing
                    var existingSubmission = (await _inventoryService.ReadAsync(new SearchInventoryModel { DeviceDate = deviceDate })).Count;
                    if (existingSubmission > 0)
                    {
                        return Problem("Existed submission", statusCode: StatusCodes.Status409Conflict);
                    }

                    submission.Status = "Submitted";
                    submission.InventoryAppId = submission.DeviceDate;
                    submission.DeviceDate = deviceDate.ToString();
                    submission.TempColumnJsonForTesting = parsedJsonData;
                    submission.ClientId = clientId;
                    submission.CreateId = data.LoginUserID ?? 0;
                    submission.UpdateDateTime = deviceDate;
                    submissionId = await _submissionService.InsertAsync(submission);
                    if (sendEmailSubmission && (!string.IsNullOrWhiteSpace(data.EmailUser) || !string.IsNullOrWhiteSpace(data.ApproverEmail)))
                    {
                        exportSubmissionModel = _mapper.Map<ExportSubmissionModel>(submission);
                        exportSubmissionModel.SubmissionId = submissionId;
                        if (!string.IsNullOrWhiteSpace(data.EmailUser) && exportSubmissionModel.UserEmails?.Any(x => x.Equals(data.EmailUser)) != true)
                        {
                            exportSubmissionModel.UserEmails.Add(data.EmailUser);
                        }
                        if (!string.IsNullOrWhiteSpace(data.ApproverEmail) && exportSubmissionModel.ApproverEmails?.Any(x => x.Equals(data.ApproverEmail)) != true)
                        {
                            exportSubmissionModel.ApproverEmails.Add(data.ApproverEmail);
                        }
                    }
                    if (data.DataItemType?.Any() == true)
                    {
                        var parentIds = data.DataItemType.Where(x => x.ParentRowId.HasValue && (x.ConditionRender == (int)ConditionRender.Server))
                                                         .Select(x => (int)x.ParentRowId).Distinct().ToList();
                        var existingServerInventories = await _inventoryService.ReadAsync(new SearchInventoryModel { Ids = parentIds }, includeInventoryImage: true);


                        foreach (var itemType in data.DataItemType)
                        {
                            // ConditionRender = ConditionRender.Barcode: Just update InventoryItem only
                            if (itemType.ConditionRender == (int)ConditionRender.Barcode && itemType.InventoryItemId.HasValue)
                            {
                                updatedInventoryItems.Add(GetInventoryItemInfoForScanBarcode(itemType));
                                continue;
                            }
                            var inventoryItems = new List<InventoryItemModel>();
                            var inventory = GetExistingOrNewInventory(existingServerInventories, itemType, submissionId, clientId, deviceDate, submissionDate);

                            // Collect information for InventoryItem
                            if (itemType.ItemTypeOptions?.Any() == true)
                            {
                                var inventoryItem = new InventoryItemModel
                                {
                                    ClientId = clientId,
                                    SubmissionId = submissionId,
                                    InventoryBuildingId = submission.InventoryBuildingID,
                                    InventoryFloorId = submission.InventoryFloorID,
                                    InventoryItemId = itemType.InventoryItemId ?? default
                                };
                                foreach (var itemOption in itemType.ItemTypeOptions)
                                {
                                    if (itemOption.ItemTypeOptionReturnValue is null) continue;

                                    try
                                    {
                                        #region Map to Inventory table

                                        if (itemType.ParentRowId == null)
                                            itemOption.CollectInventoryInfoFromJson(inventory);

                                        #endregion

                                        #region Map to InventoryItem table

                                        itemOption.CollectInventoryItemInfoFromJson(inventoryItem);

                                        // Process InventoryItemImage object
                                        if (itemOption.ItemTypeOptionCode.Equals(ItemTypeOptionCodeConstants.TotalCount))
                                        {
                                            var optionCodeTotalCount = ParseDataCondition(itemOption.ItemTypeOptionReturnValue);
                                            if (optionCodeTotalCount.Count > 0)
                                            {
                                                foreach (var dataConditionGroup in optionCodeTotalCount.Where(x => !x.ExistedItems))
                                                {
                                                    var conditionInventoryItem = (InventoryItemModel)inventoryItem.Clone();
                                                    conditionInventoryItem.Condition = dataConditionGroup.ConditionName;
                                                    conditionInventoryItem.DamageNotes = dataConditionGroup.DamageNotes;
                                                    conditionInventoryItem.ConditionId = dataConditionGroup.ConditionId;

                                                    if (dataConditionGroup.ConditionData.Any(x => x.ItemName.Contains(ItemTypeConstants.SHARED_ITEMS_NAME)))
                                                    {
                                                        foreach (var itemImage in dataConditionGroup.ConditionData)
                                                        {
                                                            var newInventoryImages = AddInventoryImages(itemImage.Url, clientId,
                                                                conditionInventoryItem.ConditionId, dataConditionGroup.DamageNotes);

                                                            var index = 0;
                                                            while (index < newInventoryImages.Count())
                                                            {
                                                                inventory.InventoryImageModels.Add(newInventoryImages.ElementAt(index));
                                                                index++;
                                                            }
                                                        }
                                                        for (var index = 0; index < dataConditionGroup.TxtQuantity; index++)
                                                            inventoryItems.Add(conditionInventoryItem);
                                                    }
                                                    else
                                                    {
                                                        foreach (var item in dataConditionGroup.ConditionData)
                                                        {
                                                            var newInventoryItem = (InventoryItemModel)conditionInventoryItem.Clone();
                                                            newInventoryItem.DamageNotes = item.DamageNotes;
                                                            newInventoryItem.IsIndividualItem = item.ItemName == ItemTypeConstants.SHARED_ITEMS_NAME ? false : true;
                                                            newInventoryItem.InventoryImages = new List<InventoryImageModel>();
                                                            newInventoryItem.InventoryImages.AddRange(AddInventoryImages(item.Url, clientId,
                                                                conditionInventoryItem.ConditionId, item.DamageNotes, true));
                                                            inventoryItems.Add(newInventoryItem);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                inventoryItems.Add(inventoryItem);
                                            }
                                        }
                                        #endregion
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, ex.Message);
                                        throw new Exception($"Error occurred at {itemOption.ItemTypeOptionCode}");
                                    }
                                }

                                inventory.InventoryItems.AddRange(inventoryItems);
                                inventory.CreateId = data.LoginUserID ?? 0;
                            }

                            inventories.Add(inventory);
                        }
                    }

                    if (sendEmailSubmission && (!string.IsNullOrWhiteSpace(data.EmailUser) || !string.IsNullOrWhiteSpace(data.ApproverEmail)))
                    {
                        exportSubmissions.Add(exportSubmissionModel);
                    }
                }

                if (updatedInventoryItems.Count > 0)
                {
                    foreach (var inventoryItem in updatedInventoryItems)
                    {
                        await _inventoryItemService.UpdateAsync(inventoryItem).ConfigureAwait(false);
                    }
                }
                var awsEndpoint = _fileService.GetAwsEndpoint();
                // Save mapped data to database
                var updatedInventoryMainImages = new List<Tuple<int, string>>();
                var updateInventories = new List<Tuple<int, string>>();
                var updateInventoryItems = new List<Tuple<int, string>>();
                var filesNeedUpload = new List<Tuple<int, string>>();
                var individualUploadImages = new List<InventoryImageModel>();
                var usedAws = _config.GetValue<bool>("ssSettings:UseAWS");
                // Insert inventories and children
                foreach (var inventory in inventories)
                {
                    var checkExistingTempMainImage = CheckExistingImageInTempFolder(inventory.MainImage);
                    var inventoryId = inventory.InventoryId;
                    // If existed inventory in database, only insert inventory item and inventory item images
                    if ((inventoryId ?? 0) == 0)
                    {
                        // Just insert when Temporary Parent row Id is zero because this case the data have been changed from client app.
                        // So it will be a new inventory
                        if ((inventory.ParentRowId ?? 0) == 0)
                        {
                            inventory.UpdateDateTime = deviceDate;
                            var inventoryResult = await _inventoryService.InsertAsync(inventory);
                            inventoryId = inventoryResult.InventoryId.Value;

                            inventory.AmountOfRepresentiveImages = await _inventoryItemImageService.GetAmountOfRepresentativeImages((int)inventoryId);
                            if (inventory.InventoryImageModels?.Count > 0)
                            {
                                foreach (var representativeImage in inventory.InventoryImageModels)
                                {
                                    representativeImage.InventoryItemId = null;
                                    representativeImage.InventoryId = inventoryId;
                                    SetInformationInventoryImages(inventory, filesNeedUpload, awsEndpoint, submissionDate, deviceDate, representativeImage);
                                }
                                await _inventoryItemImageService.InsertAsync(inventory.InventoryImageModels);
                                //representativeUploadImages.AddRange(inventory.InventoryImageModels);
                            }
                            updateInventories.Add(new Tuple<int, string>(inventoryResult.InventoryId.Value, ""));
                        }
                    }
                    else if (inventory.InventoryImageModels.Count > 0)
                    {
                        inventory.AmountOfRepresentiveImages = await _inventoryItemImageService.GetAmountOfRepresentativeImages((int)inventory.InventoryId);
                        inventory.InventoryImageModels.ForEach(item =>
                        {
                            item.InventoryItemId = null;
                            item.InventoryId = inventoryId;
                        });
                        await AddRepresentativeImagesForInventory(inventory, filesNeedUpload,
                            awsEndpoint, submissionDate, deviceDate, inventoryId);
                    }


                    if (checkExistingTempMainImage)
                    {
                        if (!string.IsNullOrWhiteSpace(inventory.MainImage))
                            updatedInventoryMainImages.Add(new Tuple<int, string>(inventoryId ?? 0,
                                inventory.MainImage));
                    }

                    foreach (var inventoryItem in inventory.InventoryItems)
                    {
                        inventoryItem.StatusId = 5;

                        if ((inventory.ParentRowId ?? 0) == 0)
                            inventoryItem.InventoryId = inventoryId.Value;

                        var inventoryItemResult = new InventoryItemModel();
                        // Update inventory item only
                        if (inventory.ParentRowId == null && inventory.ConditionRender == (int)ConditionRender.Barcode
                            && inventoryItem.InventoryItemId > 0)
                        {
                            inventoryItemResult = await _inventoryItemService.UpdateAsync(inventoryItem);
                            if (inventoryItemResult is null)
                            {
                                _logger.LogInformation($"The InventoryItemId {inventoryItem.InventoryItemId} in submission: {submissionId} not found");
                            }
                        }
                        else
                        {

                            if ((inventory.ParentRowId ?? 0) > 0 && inventory.ConditionRender == (int)ConditionRender.Client)
                            {
                                //Search for inventoryId based on clientId, inventoryRowId, InventoryAppId => Inventory was inserted
                                var existedInventory = (await _inventoryService.ReadAsync(new SearchInventoryModel
                                {
                                    ClientId = clientId,
                                    ItemRowIds = new List<int> { inventory.ParentRowId.Value },
                                    DeviceDate = deviceDate
                                })).FirstOrDefault();

                                if (existedInventory is null) continue;

                                if (existedInventory != null)
                                {
                                    inventoryItem.InventoryId = existedInventory.InventoryId.Value;
                                }

                                await AddRepresentativeImagesForInventory(inventory, filesNeedUpload, awsEndpoint,
                                    submissionDate, deviceDate, inventoryItem.InventoryId);
                            }
                            inventoryItem.InventoryItemId = default;
                            inventoryItem.SubmissionDate = submissionDate;
                            inventoryItem.LocationId = 1;
                            inventoryItem.DisplayOnSite = true;
                            inventoryItem.CreateId = inventory.CreateId;
                            inventoryItem.UpdateDateTime = deviceDate;
                            inventoryItem.CreateDateTime = deviceDate;

                            inventoryItemResult = await _inventoryItemService.InsertAsync(inventoryItem, inventoryItem.IsIndividualItem);

                            if (inventoryItem.IsIndividualItem && inventoryItemResult != null)
                            {
                                if (inventoryItem.InventoryImages.Count > 0)
                                {
                                    var representativeImages = new List<InventoryImageModel>(inventoryItem.InventoryImages);
                                    foreach (var representativeImage in representativeImages)
                                    {
                                        representativeImage.InventoryItemId = inventoryItemResult.InventoryItemId;
                                        representativeImage.InventoryId = inventoryItem.InventoryId;
                                        SetInformationInventoryImages(inventory, filesNeedUpload, awsEndpoint, submissionDate, deviceDate, representativeImage);
                                        individualUploadImages.Add((InventoryImageModel)representativeImage.Clone());
                                    }
                                    await _inventoryItemImageService.InsertAsync(representativeImages);


                                }
                            }
                            //await GenerateInventoryItemToQRcode(inventoryItemResult, inventoryItemResult.InventoryId, clientId)
                            //GenerateBarcodeToImage
                            updateInventoryItems.Add(new Tuple<int, string>(inventoryItemResult.InventoryItemId, await GenerateBarcodeToImage(inventoryItemResult.InventoryItemId, inventoryItemResult.InventoryId, clientId)));
                        }

                        if (inventoryItemResult != null)
                        {
                            var representativeImages = new List<InventoryImageModel>(inventoryItem.InventoryImages);
                            foreach (var inventoryImage in representativeImages)
                            {
                                inventoryImage.InventoryItemId = inventoryItemResult.InventoryItemId;
                                SetInformationInventoryImages(inventory, filesNeedUpload, awsEndpoint, submissionDate, deviceDate, inventoryImage);
                                await _inventoryItemImageService.UpdateAsync(new List<InventoryImageModel>
                                {
                                    inventoryImage
                                });
                            }
                        }
                    }

                    //Insert representative image - take the first maximum representative photos
                    await InsertRepresentativeImages(inventory, filesNeedUpload, awsEndpoint, submissionDate, deviceDate);
                }

                if (updatedInventoryMainImages.Count > 0)
                {
                    await _inventoryService.UpdateMainImages(updatedInventoryMainImages)
                        .ConfigureAwait(false);
                }

                // Update inventories
                if (updateInventories.Count > 0)
                {
                    await _inventoryService.UpdateInventoryCodeAndQRcodes(updateInventories).ConfigureAwait(false);
                }

                // Update inventory items
                if (updateInventoryItems.Count > 0)
                {
                    await _inventoryItemService.UpdateBarcode(updateInventoryItems).ConfigureAwait(false);
                }
                //Save representatvie iamges into aws
                var invalidFiles = new List<string>();
                var invalidFiles1 = await SaveInventoryImages(inventories, filesNeedUpload,
                    individualUploadImages);
                invalidFiles.AddRange(invalidFiles1);

                // Save inventory item images into server physical or upload to AWS
                var invalidFiles2 = await SaveInventoryItemImages(inventories.SelectMany(x => x.InventoryItems).ToList(),
                    filesNeedUpload);
                invalidFiles.AddRange(invalidFiles2);

                // Send email the submission
                if (sendEmailSubmission && exportSubmissions.Count > 0)
                {
                    await SendEmailSubmissions(exportSubmissions);
                }

                if (invalidFiles.Count > 0)
                {
                    _logger.LogInformation($"Saved data success with invalid files: {string.Join(", ", invalidFiles)}");
                }

                return Ok(new { Message = "Saved data successfully", statusCode = StatusCodes.Status200OK, data = new { submissionId } });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private async Task AddRepresentativeImagesForInventory(InventoryModel inventory,
        List<Tuple<int, string>> filesNeedUpload,
        string awsEndpoint,
        DateTime submissionDate,
        DateTime deviceDate,
        int? inventoryId)
        {
            inventory.AmountOfRepresentiveImages = await _inventoryItemImageService.GetAmountOfRepresentativeImages((int)inventoryId);
            inventory.InventoryImageModels.ForEach(item =>
            {
                item.InventoryItemId = null;
                item.InventoryId = inventoryId;
            });

            var newReperesentativeImages = new List<InventoryImageModel>();
            var NewInventoryImages = inventory.InventoryImageModels.Where(i => i.InventoryImageId == null).ToList();

            foreach (var inventoryCondition in NewInventoryImages)
            {
                //if existed Representative photo, dont need to add more
                var conditionRepresentativeExisted = inventory.AmountOfRepresentiveImages.Where(i => i.Key == inventoryCondition.ConditionId).ToList();
                if (conditionRepresentativeExisted?.Count == 0)
                {
                    //Existing representative images
                    if (inventory.AmountOfRepresentiveImages?.Count > 0)
                    {
                        foreach (var item in inventory.AmountOfRepresentiveImages)
                        {
                            var restImages = item.Value == 0 ? ItemTypeConstants.MAX_REPRESENTATIVE_IMAGE : (ItemTypeConstants.MAX_REPRESENTATIVE_IMAGE - item.Value);
                            var representativeImageList = inventory.InventoryImageModels.Where(i => i.ConditionId == item.Key && i.InventoryImageId == null)
                                    .Take(restImages).OrderBy(i => i.InventoryImageId).ToList();
                            if (representativeImageList?.Count > 0)
                                newReperesentativeImages.AddRange(representativeImageList);
                        }
                    }
                }
            }
            //Not existing representative images
            newReperesentativeImages.AddRange(inventory.InventoryImageModels
                .Where(i => !inventory.AmountOfRepresentiveImages.Keys.Contains(i.ConditionId))
                .ToList());


            //newReperesentativeImages = newReperesentativeImages.Where(x => x.InventoryImageId == 0).ToList();
            if (newReperesentativeImages.Count > 0)
            {
                newReperesentativeImages.ForEach(x =>
                {
                    x.InventoryItemId = null;
                    x.InventoryImageId = 0;
                    x.InventoryId = inventoryId;
                });
                newReperesentativeImages = newReperesentativeImages.Distinct().ToList();
                foreach (var representativeImage in newReperesentativeImages)
                    SetInformationInventoryImages(inventory, filesNeedUpload, awsEndpoint, submissionDate, deviceDate, representativeImage);
                await _inventoryItemImageService.InsertAsync(newReperesentativeImages);
            }
        }

        private void SetInformationInventoryImages(InventoryModel inventory, List<Tuple<int, string>> filesNeedUpload, string awsEndpoint, DateTime submissionDate, DateTime deviceDate, InventoryImageModel representativeImage)
        {
            var imageGuid = Guid.NewGuid();
            var isExistingInventoryImageInTemp = CheckExistingImageInTempFolder(representativeImage.TempPhotoName);
            if (!string.IsNullOrWhiteSpace(representativeImage.ImageName) && isExistingInventoryImageInTemp)
            {
                var fileExt = new FileInfo(representativeImage.ImageName).Extension;
                representativeImage.ImageName = $"{imageGuid}{fileExt}";
            }

            representativeImage.ImageUrl = isExistingInventoryImageInTemp ? awsEndpoint : "";
            representativeImage.TempImageUrl = isExistingInventoryImageInTemp ? UploadOriginFolder : "";
            representativeImage.SubmissionDate = submissionDate;
            representativeImage.ImageGuid = imageGuid;
            representativeImage.CreateId = inventory.CreateId;
            representativeImage.CreateDateTime = deviceDate;
            representativeImage.UpdateDateTime = deviceDate;
            representativeImage.StatusId = InventoryImageStatus.Active;

            if (isExistingInventoryImageInTemp)
            {
                var tempFilePath = @$"{_webHostEnvironment.ContentRootPath}/{Path.Combine(UploadOriginFolder, representativeImage.ImageName)}";
                filesNeedUpload.Add(new Tuple<int, string>(inventory.ClientId, tempFilePath));
            }
        }

        private async Task InsertRepresentativeImages(InventoryModel inventory,
            List<Tuple<int, string>> filesNeedUpload,
            string awsEndpoint,
            DateTime submissionDate,
            DateTime deviceDate)
        {
            var newReperesentativeImages = new List<InventoryImageModel>();
            if (inventory.AmountOfRepresentiveImages?.Count > 0)
            {
                foreach (var item in inventory.AmountOfRepresentiveImages)
                {
                    var amountTaking = item.Value == 0 ? ItemTypeConstants.MAX_REPRESENTATIVE_IMAGE : (ItemTypeConstants.MAX_REPRESENTATIVE_IMAGE - item.Value);
                    var representativeImageList = inventory.InventoryItems.Where(i => i.ConditionId == item.Key)
                            .SelectMany(i => i.InventoryImages).OrderBy(i => i.InventoryImageId).Take(amountTaking).ToList();
                    if (representativeImageList?.Count > 0)
                        newReperesentativeImages.AddRange(representativeImageList);
                }
            }
            newReperesentativeImages.AddRange(inventory.InventoryItems.Where(i => !inventory.AmountOfRepresentiveImages.Keys.Contains(i.ConditionId))
                .SelectMany(i => i.InventoryImages).OrderBy(i => i.InventoryImageId).Take(5).ToList());

            if (newReperesentativeImages.Count > 0)
            {
                foreach (var representativeImage in newReperesentativeImages)
                {
                    representativeImage.InventoryItemId = null;
                    var imageGuid = Guid.NewGuid();
                    var isExistingInventoryImageInTemp = CheckExistingImageInTempFolder(representativeImage.TempPhotoName);
                    if (!string.IsNullOrWhiteSpace(representativeImage.ImageName) && isExistingInventoryImageInTemp)
                    {
                        var fileExt = new FileInfo(representativeImage.ImageName).Extension;
                        representativeImage.ImageName = $"{imageGuid}{fileExt}";
                    }
                    representativeImage.InventoryItemId = default;
                    representativeImage.SubmissionDate = submissionDate;
                    representativeImage.CreateId = inventory.CreateId;
                    representativeImage.UpdateDateTime = deviceDate;
                    representativeImage.CreateDateTime = deviceDate;

                    if (isExistingInventoryImageInTemp)
                    {
                        var tempFilePath = @$"{_webHostEnvironment.ContentRootPath}/{Path.Combine(UploadOriginFolder, representativeImage.ImageName)}";
                        filesNeedUpload.Add(new Tuple<int, string>(inventory.ClientId, tempFilePath));
                    }
                }
                await _inventoryItemImageService.InsertAsync(newReperesentativeImages);
            }
        }

        /// <summary>
        /// Re-upload failed images
        /// Post data via form
        /// </summary>
        /// <param name="photo" example="Test.jpg"></param>
        /// <param name="clientId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="406">File is not accepted</response>
        /// <response code="422">Process fail</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReUploadTempPhoto([FromForm] IFormFile photo, [FromForm] int clientId)
        {
            // Validate input
            if (photo is null || clientId < 1)
                return BadRequest("Invalid data");

            var fileExt = Path.GetExtension(photo.FileName);
            if (!CheckIfPhotoFile(fileExt))
                return Problem($"File {photo.FileName} is invalid extension", statusCode: StatusCodes.Status406NotAcceptable);

            var inventoryInfo = await _inventoryItemImageService.GetInventoryInfoByName(photo.FileName, clientId);
            if (inventoryInfo is null)
                return NotFound("Photo is not found");

            try
            {
                var imageName = $"{clientId}_{inventoryInfo.InventoryCode}_{inventoryInfo.InventoryId}_{inventoryInfo.InventoryItemId}_{inventoryInfo.NumberOfImages + 1}{fileExt}";
                var filePath = Path.Combine(UploadOriginFolder, imageName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }
                var result = await _inventoryItemImageService.UpdateImage(inventoryInfo.InventoryImageId, imageName);
                if (result)
                    return Ok("Uploaded photo successfully");
            }
            catch (System.FormatException ex)
            {
                _logger.LogError(ex, $"{photo.FileName} invalid content");
                return Problem("Error while processing data" + ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

            return Problem("Uploaded photo failed", statusCode: StatusCodes.Status422UnprocessableEntity);
        }

        /// <summary>
        /// Upload temporary photos
        /// </summary>
        /// <param name="photos"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="406">Photo extensions do not acceptable</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)] //200MB
        [RequestSizeLimit(209715200)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadTempPhotos(List<IFormFile> photos)
        {
            try
            {
                if (photos.Count > 0)
                {
                    var filesSize = photos.Sum(x => x.Length);
                    if (filesSize > 209715200)
                        return BadRequest("File sizes must smaller than 200MB");

                    foreach (var photo in photos)
                    {
                        var fileExt = Path.GetExtension(photo.FileName);
                        if (!CheckIfPhotoFile(fileExt))
                            return Problem($"File {photo.FileName} is invalid extension", statusCode: StatusCodes.Status406NotAcceptable);
                    }

                    CreateFolder(UploadTempFolder);

                    await SaveFile(photos, UploadTempFolder, _webHostEnvironment);

                    return Ok("Upload files success");
                }

                return BadRequest();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Save item type
        /// </summary>
        /// <param name="input"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveItemType(CreateOrUpdateItemTypeModel input)
        {
            // Current only accepted create new action
            try
            {
                if (input.ClientId < 1)
                {
                    return BadRequest("Client ID is invalid");
                }
                if (string.IsNullOrWhiteSpace(input.ItemTypeName) && string.IsNullOrWhiteSpace(input.ItemTypeCode))
                {
                    return BadRequest("Item Type is invalid");
                }
                input.ItemTypeId = 0;
                input.IsNewType = true;
                input.ItemTypeCode = input.ItemTypeCode.ConvertItemTypeName(input.ItemTypeName);
                if (string.IsNullOrEmpty(input.ItemTypeDesc))
                {
                    input.ItemTypeDesc = input.ItemTypeName;
                }
                var result = await _itemTypeOptionSetService.SaveAsync(input);

                return result is null ? BadRequest() : Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        #region Protected methods

        /// <summary>
        /// Delete item types
        /// </summary>
        /// <param name="itemTypeIds"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        protected async Task<IActionResult> DeleteItemTypes(List<int> itemTypeIds)
        {
            try
            {
                if (itemTypeIds.Count == 0) return BadRequest();

                return Ok(await _itemTypeOptionSetService.DeleteAsync(itemTypeIds));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        #endregion

        #region Private Methods


        private InventoryItemModel UpdateInventoryItem(List<ItemTypeOptionApiModel> options)
        {
            var inventoryItem = new InventoryItemModel();
            foreach (var itemOption in options)
            {
                if (itemOption.ItemTypeOptionReturnValue is null) continue;

                itemOption.CollectInventoryItemInfoFromJson(inventoryItem);
            }

            return inventoryItem;
        }

        private SelectItemOptionModel GetSelectedInventoryItem(object returnValue)
        {
            var selectedManyValues = returnValue.ParseSelectedDropdownValueOption();
            if (selectedManyValues is null || selectedManyValues?.Any() == false) return null;

            var selectedValues = selectedManyValues.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x?.ReturnName));
            return selectedValues ?? null;
        }

        private List<ItemTypeOptionReturnCondition> ParseDataCondition(object itemTypeOptionReturnValue)
            => JsonConvert.DeserializeObject<List<ItemTypeOptionReturnCondition>>(itemTypeOptionReturnValue.ToString());

        //private InventoryItemModel CopyInventoryItemData(InventoryItemModel inventoryItem)
        //    => new InventoryItemModel
        //    {
        //        ClientId = inventoryItem.ClientId,
        //        GpsLocation = inventoryItem.GpsLocation,
        //        Room = inventoryItem.Room,
        //        DamageNotes = inventoryItem.DamageNotes,
        //        Condition = inventoryItem.Condition,
        //        SubmissionId = inventoryItem.SubmissionId,
        //        InventoryBuildingId = inventoryItem.InventoryBuildingId,
        //        InventoryFloorId = inventoryItem.InventoryFloorId,
        //        InventoryItemId = inventoryItem.InventoryItemId,
        //        ConditionId = inventoryItem.ConditionId
        //    };

        #region Inventory

        private InventoryModel GetExistingOrNewInventory(IList<InventoryModel> existingInventories, DataItemType itemType, int submissionId, int clientId, DateTime deviceDate, DateTime submissionDate)
        {
            var inventory = (itemType.ParentRowId ?? 0) > 0 && (itemType.ConditionRender == (int)ConditionRender.Server)
                ? existingInventories.FirstOrDefault(x => x.InventoryId == itemType.ParentRowId)
                : new InventoryModel
                {
                    ItemTypeId = itemType.ItemTypeID,
                    ItemRowId = itemType.InventoryRowId,
                    SubmissionId = submissionId,
                    ItemCode = itemType.ItemCode,
                    Category = itemType.ItemCode,
                    ClientId = clientId,
                    MainImage = itemType.MainImage,
                    DeviceDate = deviceDate,
                    UpdateDateTime = submissionDate,
                    SubmissionDate = submissionDate,
                    ConditionRender = itemType.ConditionRender,
                    ParentRowId = itemType.ParentRowId,
                    InventoryImageModels = new List<InventoryImageModel>()
                };
            inventory.InventoryItems = new List<InventoryItemModel>();

            return inventory;
        }

        private void AddImagesToInventoryItem(InventoryItemModel conditionInventoryItem, List<DataConditionUrl> listUrl, int clientId)
        {
            listUrl.ForEach(url =>
            {
                conditionInventoryItem.InventoryImages.Add(new InventoryImageModel
                {
                    ImageName = url.Name,
                    ImageUrl = "",
                    Width = url.Width,
                    Height = url.Height,
                    ClientId = clientId,
                    ItemTypeAutomationId = url.ItemTypeAutomationId,
                    ItemTypeAutomationOptionId = url.ItemTypeAutomationOptionId,
                    TempPhotoName = url.TempPhotoName,
                    CreateDateTime = DateTime.Now,
                    CreateId = 0,
                    ConditionId = conditionInventoryItem.ConditionId,
                    InventoryItemId = null,
                    StatusId = InventoryImageStatus.Active
                });
            });
        }

        private static IEnumerable<InventoryImageModel> AddInventoryImages(List<DataConditionUrl> listUrl,
            int clientId, int conditionId, string damageNotes = "", bool isIndividual = false)
        {
            var counter = 0;
            foreach (var url in listUrl)
            {
                counter++;
                if (!isIndividual)
                {
                    if (counter < ItemTypeConstants.MAX_REPRESENTATIVE_IMAGE)
                        yield return new InventoryImageModel
                        {
                            ImageUrl = "",
                            ImageName = url.Name,
                            Width = url.Width,
                            Height = url.Height,
                            ClientId = clientId,
                            ItemTypeAutomationId = url.ItemTypeAutomationId,
                            ItemTypeAutomationOptionId = url.ItemTypeAutomationOptionId,
                            TempPhotoName = url.TempPhotoName,
                            CreateDateTime = DateTime.Now,
                            CreateId = 0,
                            ConditionId = conditionId,
                            InventoryItemId = isIndividual ? url.InventoryItemId : null,
                            DamageNotes = damageNotes,
                            StatusId = InventoryImageStatus.Active
                        };
                }
                else
                    yield return new InventoryImageModel
                    {
                        ImageUrl = "",
                        ImageName = url.Name,
                        Width = url.Width,
                        Height = url.Height,
                        ClientId = clientId,
                        ItemTypeAutomationId = url.ItemTypeAutomationId,
                        ItemTypeAutomationOptionId = url.ItemTypeAutomationOptionId,
                        TempPhotoName = url.TempPhotoName,
                        CreateDateTime = DateTime.Now,
                        CreateId = 0,
                        ConditionId = conditionId,
                        InventoryItemId = isIndividual ? url.InventoryItemId : null,
                        DamageNotes = damageNotes,
                        StatusId = InventoryImageStatus.Active
                    };
            }
        }

        #endregion

        #region InventoryItem

        private InventoryItemModel GetInventoryItemInfoForScanBarcode(DataItemType itemType)
        {
            var updatedInventoryItem = UpdateInventoryItem(itemType.ItemTypeOptions);
            updatedInventoryItem.Condition = itemType.Condition;
            updatedInventoryItem.StatusId = itemType.StatusId;
            updatedInventoryItem.DamageNotes = itemType.NoteForItem;
            updatedInventoryItem.InventoryItemId = itemType.InventoryItemId.Value;

            return updatedInventoryItem;
        }

        #endregion

        #region Files

        /// <summary>
        /// Save temporary file to physical folder then upload to AWS if the configure is enable
        /// </summary>
        /// <param name="saveInventoryItems"></param>
        /// <param name="filesNeedUpload"></param>
        /// <returns></returns>
        private async Task<List<string>> SaveInventoryItemImages(IEnumerable<InventoryItemModel> saveInventoryItems,
            List<Tuple<int, string>> filesNeedUpload)
        {
            var saveInventoryItemImages = saveInventoryItems.SelectMany(x => x.InventoryImages.Where(y => y.TempPhotoName != null)).ToList();
            var usedAws = _config.GetValue<bool>("ssSettings:UseAWS");
            // Save temporary file into another folder before process because it needs to rename
            // Sometimes throw file is processing by another program exception if try hard to rename in the current file
            // Files in this folder will be removed by background job
            var invalidFiles = SaveImagesToPhysicalFolder(saveInventoryItemImages);
            if (usedAws && filesNeedUpload.Count > 0)
            {
                await _fileService.UploadFilesToAWS(filesNeedUpload);
            }

            return invalidFiles;
        }

        private async Task<List<string>> SaveInventoryImages(List<InventoryModel> inventories,
            List<Tuple<int, string>> filesNeedUpload, List<InventoryImageModel> individualImages)
        {
            var saveInventoryImages = inventories.SelectMany(x => x.InventoryImageModels).ToList();
            var inventoryItemImages = inventories.SelectMany(x => x.InventoryItems.SelectMany(x => x.InventoryImages.Select(x => x.TempPhotoName))).ToList();
            var usedAws = _config.GetValue<bool>("ssSettings:UseAWS");
            // Save temporary file into another folder before process because it needs to rename
            // Sometimes throw file is processing by another program exception if try hard to rename in the current file
            // Files in this folder will be removed by background job

            saveInventoryImages.AddRange(individualImages);
            var invalidFiles = SaveImagesToPhysicalFolder(saveInventoryImages, inventoryItemImages);
            if (usedAws && filesNeedUpload.Count > 0)
            {
                await _fileService.UploadFilesToAWS(filesNeedUpload);
            }

            return invalidFiles;
        }

        private bool CheckIfPhotoFile(string fileExtension)
        {
            var extension = fileExtension.ToLower();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".gif" || extension == ".png" || extension == ".bmp" || extension == ".tiff";
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
            var existed = fileName.CheckExistingImageInTempFolder(_webHostEnvironment, UploadTempFolder);
            if (!existed)
            {
                _logger.LogDebug($"The file name {fileName} is not found in the temporary {UploadTempFolder} folder");
            }

            return existed;
        }

        private List<string> SaveImagesToPhysicalFolder(List<InventoryImageModel> saveInventoryImages,
            List<string> inventoryItemImages = null)
        {
            if (saveInventoryImages?.Any() == false) return new List<string>();

            var invalidFiles = new List<string>();
            foreach (var image in saveInventoryImages)
            {
                if (!string.IsNullOrWhiteSpace(image.TempPhotoName))
                {
                    try
                    {
                        var newFileName = $"{image.ImageName}";
                        var isCopy = inventoryItemImages?.Contains(image.TempPhotoName);
                        var success = RenameAndMoveFileFromTempToOrigin(image.TempPhotoName, newFileName, UploadTempFolder, UploadOriginFolder, isCopy);
                        if (!success)
                        {
                            if (!invalidFiles.Contains(image.TempPhotoName))
                            {
                                invalidFiles.Add(image.TempPhotoName);
                            }
                        }
                    }
                    catch
                    {
                        if (!invalidFiles.Contains(image.ImageName))
                        {
                            invalidFiles.Add(image.ImageName);
                        }
                    }
                }
            }

            return invalidFiles;
        }

        private bool RenameAndMoveFileFromTempToOrigin(string tempFileName, string originFileName,
            string tempPath, string originPath, bool? isCopy = false)
        {
            var sourcePath = @$"{_webHostEnvironment.ContentRootPath}/{Path.Combine(tempPath, tempFileName)}";
            if (System.IO.File.Exists(sourcePath))
            {
                CreateFolder(originPath);
                var destinationPath = @$"{_webHostEnvironment.ContentRootPath}/{Path.Combine(originPath, originFileName)}";
                if (isCopy == true)
                    System.IO.File.Copy(sourcePath, destinationPath, true);
                else
                    System.IO.File.Move(sourcePath, destinationPath, true);

                return true;
            }

            return false;
        }

        #endregion

        #region QRcode

        private async Task<string> GenerateInventoryToQRcode(InventoryModel inventory)
        {
            var dataItemTypes = await _inventoryService.GetInventorySearch(new SearchImageInfo
            {
                InventoryId = inventory.InventoryId.Value,
                ClientId = inventory.ClientId
            }, UploadOriginFolder, baseUrl: $"{Request.Scheme}://{Request.Host}{Request.PathBase}", includeTotalCountImages: true);

            if (dataItemTypes.Count < 1) return null;

            var payloadGenerator = new DataItemTypePayload(dataItemTypes.First());
            string fileName = $"Inventory-QR-{inventory.InventoryId}-{inventory.ClientId}.png";
            var path = $"Images/InventoryItemQRcode/{fileName}";
            var target = Path.Combine(_webHostEnvironment.ContentRootPath, path);

            await SaveBytesToFile(payloadGenerator.ToString().ToPngByteQRCode(), target).ConfigureAwait(false);

            return path;
        }

        private async Task<string> GenerateInventoryItemToQRcode(InventoryItemModel inventoryItem, int inventoryId, int clientId)
        {
            var dataItemTypes = await _inventoryService.GetInventorySearch(new SearchImageInfo
            {
                InventoryId = inventoryId,
                ClientId = clientId
            }, UploadOriginFolder, baseUrl: $"{Request.Scheme}://{Request.Host}{Request.PathBase}", includeTotalCountImages: true);

            if (dataItemTypes.Count < 1) return null;

            var selectedItemTypeOptions = new List<string> { "Building", "Floor", "GPS", "AreaOrRoom", "TotalCount" };
            var itemTypeOptionSets = await GetItemTypeOptionSet(dataItemTypes.First(), 2, selectedItemTypeOptions);
            if (itemTypeOptionSets.Count < 1) return "";

            var itemTypeOptionSet = itemTypeOptionSets.First();
            var QRcodeModel = new InventoryItemQRcodeModel
            {
                ClientId = clientId,
                InventoryItemId = inventoryItem.InventoryItemId,
                ItemTypeName = itemTypeOptionSet.ItemTypeName,
                ItemTypeOptions = ConvertToItemTypeOptions(itemTypeOptionSet.ItemTypeOptions, selectedItemTypeOptions)
            };

            var payloadGenerator = new InventoryItemPayload(QRcodeModel);

            string fileName = $"InventoryItem-QR-{inventoryItem.InventoryId}-{inventoryItem.InventoryItemId}-{inventoryItem.ClientId}.png";
            var path = $"Images/InventoryItemQRcode/{fileName}";
            var target = Path.Combine(_webHostEnvironment.ContentRootPath, path);

            await SaveBytesToFile(payloadGenerator.ToString().ToPngByteQRCode(), target).ConfigureAwait(false);

            return path;
        }

        private async Task<string> GenerateBarcodeToImage(int inventoryItemId, int inventoryId, int clientId)
        {
            try
            {
                var content = $"SSI.inv.{inventoryItemId}";
                var filePath = $"Images/Barcode/InventoryItem/Barcode-{clientId}-{inventoryId}-{inventoryItemId}.png";
                var target = Path.Combine(_webHostEnvironment.ContentRootPath, filePath);
                //var barcodeContent = content.CreateBarCode(target);

                var skData = content.Encode();
                var bitmapImageStream = System.IO.File.Open(target, FileMode.Create, FileAccess.Write, FileShare.None);
                skData.SaveTo(bitmapImageStream);
                await bitmapImageStream.FlushAsync();
                await bitmapImageStream.DisposeAsync();
                return filePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return "";
            }
        }

        #endregion

        private async Task<List<ItemTypeModel>> GetItemTypeOptionSet(DataItemType dataItemType, int clientGroupId, List<string> itemTypeOptions = null)
        {
            try
            {
                var data = await _itemTypeService.GetItemTypeOptionSetAsync(dataItemType.ClientId.Value, dataItemType.ItemTypeID);
                return ConvertToItemTypesModel(data, isSearch: true, dataItemType: dataItemType, fetchFirst: true, itemTypeOptionCodes: itemTypeOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return null;
        }

        private List<ItemTypeOption> ConvertToItemTypeOptions(List<ItemTypeOptionModel> itemTypeOptions, List<string> selectedItemTypeOptions)
        {
            var result = new List<ItemTypeOption>();
            foreach (var selectedOption in selectedItemTypeOptions)
            {
                var option = itemTypeOptions.FirstOrDefault(x => x.ItemTypeOptionCode.Equals(selectedOption));
                result.Add(new ItemTypeOption
                {
                    ItemTypeOptionCode = option is null ? selectedOption : option.ItemTypeOptionCode,
                    ItemTypeOptionReturnValue = option is null ? null : option.ItemTypeOptionReturnValue
                });
            }
            return result;
        }


        #region Submissions
        private async Task SendEmailSubmissions(List<ExportSubmissionModel> exportSubmissions)
        {
            Debug.Assert(true, "SendEmailSubmissions");
            var mailTo = string.Join(";", exportSubmissions.SelectMany(x => x.UserEmails.Select(y => y)).ToList());
            var cc = exportSubmissions.Where(x => x.ApproverEmails.Any(y => !string.IsNullOrWhiteSpace(y))).SelectMany(x => x.ApproverEmails.Select(y => y)).ToList();
            var ccTos = string.Join(";", cc);
            if (string.IsNullOrWhiteSpace(mailTo) && string.IsNullOrWhiteSpace(ccTos))
                return;

            var submissions = await _submissionService.GetSubmissionForExport(exportSubmissions);
            var exportExcel = new ExportExcel(_memoryCache);
            var exportFile = exportExcel.ExportSubmissionToFile(submissions, @$"{_webHostEnvironment.ContentRootPath}\\{UploadOriginFolder}\\");

            //var emailService = new MailService(_config);

            var firstSubmission = submissions.FirstOrDefault();
            var mailRequest = new MailRequest
            {
                Subject = $"Submission from client {firstSubmission?.Client}",
                ToEmail = !string.IsNullOrWhiteSpace(mailTo) ? mailTo : ccTos,
                Body = $"There is new submission from client: <b>{firstSubmission?.Client} (ID: {firstSubmission?.ClientId})</b> in {firstSubmission?.InventoryAppId?.ToString()}." +
                "<br/><br/>Please check the attachment for more details.<br/><br/>Regards.",
            };
            if (!string.IsNullOrWhiteSpace(mailTo) && !string.IsNullOrWhiteSpace(ccTos))
            {
                mailRequest.CCs.AddRange(cc);
            }
            if (exportFile.FileContent.Length > 0)
            {
                mailRequest.FileAttachments = new List<Models.Files.TempFileInfo>
                {
                    new Models.Files.TempFileInfo
                    {
                        FileName = exportFile.FileName,
                        FileType = exportFile.FileType,
                        File = exportFile.FileContent
                    }
                };
            }
            var netMailSender = new NetMailSender(_config);
            await netMailSender.SendEmailAsync(mailRequest);

            //await emailService.SendEmailAsync(mailRequest);
        }

        #endregion

        #endregion
    }
}