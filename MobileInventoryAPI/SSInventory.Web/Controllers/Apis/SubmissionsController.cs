using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSInventory.Business.Interfaces;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.Submission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using SSInventory.Share.Models.Search;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryItem;

namespace SSInventory.Web.Controllers.Apis
{
    /// <summary>
    /// Submissions controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubmissionsController : CommonController
    {
        private readonly ISubmissionService _submissionService;
        private readonly IDapperService _dapperService;
        private readonly IInventoryService _inventoryService;
        private readonly IConfiguration _config;
        private readonly ILogger<SubmissionsController> _logger;
        private readonly IItemTypeService _itemTypeService;
        private readonly IMapper _mapper;
        private readonly string UploadOriginFolder;

        /// <summary>
        /// Submissions controller constructor
        /// </summary>
        /// <param name="submissionService"></param>
        /// <param name="dapperService"></param>
        /// <param name="inventoryService"></param>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public SubmissionsController(ISubmissionService submissionService,
            IDapperService dapperService,
            IInventoryService inventoryService,
            IConfiguration config,
            IItemTypeService itemTypeService,
            ILogger<SubmissionsController> logger,
            IMapper mapper)
        {
            _submissionService = submissionService;
            _dapperService = dapperService;
            _inventoryService = inventoryService;
            _config = config;
            _logger = logger;
            _mapper = mapper;
            _itemTypeService = itemTypeService;
            UploadOriginFolder = _config["ssSettings:UploadOriginFolder"];
        }

        /// <summary>
        /// Get list submission which have been submitted
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Error when getting the data</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSubmissions()
        {
            try
            {
                var data = await _submissionService.ReadAsync(status: "Submitted");

                return Ok(data);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                return BadRequest();
            }
        }

        /// <summary>
        /// Get submissions based on clientID and InventoryApp paramters
        /// </summary>
        /// <param name="filter"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="500">Error when getting the data</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSubmission(SubmissionFilterModel filter)
        {
            if (filter.ClientId < 1)
                return BadRequest("Client ID is required");

            if (string.IsNullOrWhiteSpace(filter.InventoryAppId))
                return BadRequest("Inventory App is required");

            try
            {
                var submissions = await _submissionService.ReadAsync(inventoryAppId: filter.InventoryAppId, clientId: filter.ClientId);
                if (submissions.Count == 0)
                    return NotFound("No data found");

                var inventoryItems = await GroupInventoryItems(submissions.Select(x => x.SubmissionId).ToArray(), filter.ClientId);
                if(inventoryItems.Count == 0)
                    return NotFound("No data found");

                var inventories = await _inventoryService.ReadAsync(new SearchInventoryModel
                {
                    ClientId = filter.ClientId,
                    Ids = inventoryItems.ConvertAll(x => x.InventoryId).Distinct().ToList()
                });

                var result = new List<SubmissionResponseModel>();
                foreach (var inventory in inventories)
                {
                    var inventoryItem = inventoryItems.FirstOrDefault(x => x.InventoryId == inventory.InventoryId);
                    var dataItemTypes = await _inventoryService.SearchInventory(new SearchImageInfo
                    {
                        InventoryId = inventory.InventoryId.Value,
                        ClientId = inventory.ClientId,
                        InventorySubmissionId = inventory.SubmissionId,
                        InventoryItemSubmissionId = inventoryItem.SubmissionId 
                        //InventoryItemId = inventoryItem?.InventoryItemId
                    },
                    UploadOriginFolder,
                    baseUrl: $"{Request.Scheme}://{Request.Host}{Request.PathBase}",
                    includeTotalCountImages: true,
                    includeQRcode: true,
                    includeQuanlity: true,
                    isSearch: true);

                    if (dataItemTypes.Count > 0)
                    {
                        var itemTypeOptionSet = await GetItemTypeOptionSet(dataItemTypes[0], filter.clientGroupId);
                        if (itemTypeOptionSet != null)
                        {
                            foreach (var item in _mapper.Map<List<SubmissionResponseModel>>(itemTypeOptionSet))
                            {
                                item.SubmissionId = inventory.SubmissionId ?? 0;
                                item.InventoryId = dataItemTypes[0].InventoryId;
                                result.Add(item);
                            }
                        }
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return Problem("Error occurred while getting the information" + ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<List<ItemTypeModel>> GetItemTypeOptionSet(DataItemType dataItemType, int clientGroupId)
        {
            try
            {
                var data = await _itemTypeService.GetItemTypeOptionSetAsync(dataItemType.ClientId, dataItemType.ItemTypeID);
                return ConvertToItemTypesModel(data, isSearch: true, dataItemType: dataItemType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return null;
        }

       
        private async Task<List<SearchInventoryItemSubmissionModel>> GroupInventoryItems(int?[] submissionIds, int clientId)
        {
            var data = await _inventoryService.GroupInventoryItemsAsync(submissionIds, clientId);
            return data;
            
        }
    }
}
