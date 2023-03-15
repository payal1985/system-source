using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SSInventory.Business.Interfaces;
using SSInventory.Share.Models;
using SSInventory.Share.Models.Dto.ItemTypes;
using SSInventory.Share.Models.Search;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Web.Controllers.Apis
{
    /// <summary>
    /// Search controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SearchController : CommonController
    {
        private readonly IInventoryService _inventoryService;
        private readonly IDapperService _dapperService;
        private readonly IConfiguration _config;
        private readonly ILogger<SearchController> _logger;
        private readonly IItemTypeService _itemTypeService;

        private readonly string UploadOriginFolder;
        /// <summary>
        /// Search controller constructor
        /// </summary>
        /// <param name="inventoryService"></param>
        /// <param name="dapperService"></param>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        public SearchController(IInventoryService inventoryService,
            IDapperService dapperService,
            IItemTypeService itemTypeService,
            IConfiguration config,
            ILogger<SearchController> logger)
        {
            _inventoryService = inventoryService;
            _dapperService = dapperService;
            _config = config;
            _logger = logger;
            _itemTypeService = itemTypeService;
            UploadOriginFolder = _config["ssSettings:UploadOriginFolder"];
        }

        /// <summary>
        /// Get list of images with search condition
        /// </summary>
        /// <param name="input"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="500">Error when getting the data</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchImages(SearchModel input)
        {
            if (input.SearchType == Share.Enums.SearchEnums.Server && (input.ClientId ?? 0) == 0)
            {
                return BadRequest("Client is required");
            }
            if (input.CurrentPage < 1)
            {
                input.CurrentPage = 1;
            }
            if (input.ItemsPerPage > 500)
            {
                input.ItemsPerPage = 500;
            }
            try
            {
                var usedAws = _config.GetValue<bool>("ssSettings:UseAWS");
                var rootFilePath = usedAws ? "" : UploadOriginFolder;
                return Ok(await _inventoryService.SearchByImage(input, rootFilePath, usedAws));
            }
            catch (System.Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error occurred while getting image information" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get image information
        /// </summary>
        /// <param name="input"></param>
        /// <response code="200">Success</response>
        /// <response code="204">No content</response>
        /// <response code="400">Error when getting the data</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Data not found</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImageInfo(SearchImageInfo input)
        {
            if (input.InventoryId == 0)
                return ValidationProblem("Invalid data", statusCode: StatusCodes.Status403Forbidden);

            if (input.ClientId == 1 && input.SearchType == Share.Enums.SearchEnums.Server)
                return ValidationProblem("Client is required", statusCode: StatusCodes.Status403Forbidden);

            try
            {
                var dataItemTypes = await _inventoryService.SearchInventory(input, UploadOriginFolder);
                return dataItemTypes?.Count == 0
                    ? NotFound("No data found")
                    : Ok(await GetItemTypeOptionSet(dataItemTypes.First(), input.ClientGroupId ?? 0));
            }
            catch (System.Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error occurred while getting image information" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        #region Private Methods

        private async Task<List<ItemTypeModel>> GetItemTypeOptionSet(DataItemType dataItemType, int clientGroupId = 0)
        {
            try
            {
                var data = await _itemTypeService.GetItemTypeOptionSetAsync(dataItemType.ClientId, dataItemType.ItemTypeID);
                return ConvertToItemTypesModel(data, isSearch: true, dataItemType: dataItemType);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return null;
        }

        #endregion
    }
}
