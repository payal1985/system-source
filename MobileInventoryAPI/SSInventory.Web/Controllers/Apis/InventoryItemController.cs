using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSInventory.Business.Interfaces;
using SSInventory.Share.Models.Dto.InventoryItem;
using SSInventory.Share.Models.Search;
using SSInventory.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Web.Controllers.Apis
{
    /// <summary>
    /// Inventory Item API
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InventoryItemController : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService;
        private readonly ILogger<InventoryItemController> _logger;

        /// <summary>
        /// Inventory Item constructor
        /// </summary>
        /// <param name="inventoryItemService"></param>
        /// <param name="logger"></param>
        public InventoryItemController(IInventoryItemService inventoryItemService,
            ILogger<InventoryItemController> logger)
        {
            _inventoryItemService = inventoryItemService;
            _logger = logger;
        }

        /// <summary>
        /// Update Inventory Item locations
        /// </summary>
        /// <param name="input"></param>
        /// <response code="200">Success</response>
        /// <response code="304">Data updates failure</response>
        /// <response code="400">Invalid data</response>
        /// <response code="500">Error occurred while processing data</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLocations([FromBody] UpdateLocationInputModel input)
        {
            try
            {
                if (input.ClientId < 1 || input.ClientId > int.MaxValue)
                    return BadRequest("Invalid Client ID");

                if (input.Locations is null)
                    return BadRequest("Location is required");

                if (input.InventoryItemId.Count < 1)
                    return BadRequest("Inventory item Id is required");

                var response = await _inventoryItemService.UpdateLocations(input);
                if (response.Success)
                    return Ok(response.Message);

                return Problem(response.Message, statusCode: StatusCodes.Status304NotModified);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return Problem("Error while processing data" + ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Search inventory items
        /// </summary>
        /// <param name="input"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="404">Data not found</response>
        /// <response code="500">Error occurred while processing data</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchInventoryItems(SearchByLocationModel input)
        {
            try
            {
                if (input.CurrentPage < 1)
                    input.CurrentPage = 1;
                if (input.ItemsPerPage > 500)
                    input.ItemsPerPage = 500;
                var filter = new InventoryItemFilterModel
                {
                    ClientId = input.ClientId,
                    BuildingIds = (input.BuildingId ?? 0) > 0 ? new List<int> { input.BuildingId.Value } : null,
                    FloorIds = (input.FloorId ?? 0) > 0 ? new List<int> { input.FloorId.Value } : null,
                    Room = input.Room,
                    ItemTypeId = (input.ItemTypeId ?? 0) > 0 ? input.ItemTypeId.Value : 0,
                    SearchString = input.SearchString,
                    ConditionId = input.ConditionId,
                    ItemsPerPage = input.ItemsPerPage,
                    CurrentPage = input.CurrentPage
                };
                var inventoryItems = await _inventoryItemService.SearchByConditions(filter);
                return Ok(inventoryItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem("Error while processing data" + ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Search inventory items
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="inventoryId"></param>
        /// <response code="200">Success</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInventoryItemDetailInfo_NotUsed(int clientId, int inventoryId)
        {
            var inventoryItems = await _inventoryItemService.ReadAsync(new InventoryItemFilterModel
            {
                ClientId = clientId,
                inventoryIds = new List<int> { inventoryId },
                IncludeItemImages = true
            });

            return Ok(inventoryItems);
        }

        /// <summary>
        /// Search simple inventory items
        /// </summary>
        /// <param name="input"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="404">Data not found</response>
        /// <response code="500">Error occurred while processing data</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchSimpleInventoryItems(SearchSimpleInventoryItem input)
        {
            if ((input.ClientId ?? 0) == 0)
                return BadRequest("Client ID is required");

            if (input.InventoryID < 1)
                return BadRequest("Inventory ID is required");
            if (input.CurrentPage < 1)
                input.CurrentPage = 1;
            if (input.ItemsPerPage > 500)
                input.ItemsPerPage = 500;

            try
            {
                var inventoryItems = await _inventoryItemService.SearchSimpleInventoryItems(new SearchSimpleInventoryItem
                {
                    ClientId = input.ClientId,
                    InventoryID = input.InventoryID,
                    CurrentPage = input.CurrentPage,
                    ItemsPerPage = input.ItemsPerPage
                });
                return Ok(inventoryItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                return Problem("Error while processing data" + ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update inventory item
        /// </summary>
        /// <param name="input"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="500">Error while processing data</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateInventoryItem([FromBody] UpdateInventoryModel input)
        {
            if (input == null)
                return BadRequest("Inventory item is required");

            if (input.InventoryItem.ClientId < 1 || input.InventoryItem.InventoryItemId < 1)
                return BadRequest("Invalid data");

            try
            {
                var inventoryItems = await _inventoryItemService.ReadAsync(new InventoryItemFilterModel
                {
                    Ids = new List<int> { input.InventoryItem.InventoryItemId.Value }
                });
                if (inventoryItems?.Any() != true)
                    return BadRequest("Inventory is not found");

                var inventoryItem = inventoryItems.First();
                if (inventoryItem.InventoryId != input.InventoryItem.InventoryId)
                    return BadRequest("Inventory info does not matches");

                foreach (var option in input.InventoryItem.ItemTypeOptions)
                {
                    if (option.ItemTypeOptionReturnValue is null) continue;

                    option.CollectInventoryItemInfoFromJson(inventoryItem);
                }

                inventoryItem.StatusId = input.InventoryItem.StatusId;
                inventoryItem.Condition = input.InventoryItem.Condition;
                inventoryItem.ConditionId = input.InventoryItem.ConditionId;
                inventoryItem.DamageNotes = input.InventoryItem.NoteForItem;
                inventoryItem.UpdateDateTime = DateTime.Now;
                inventoryItem.UpdateId = input.UserId;
                await _inventoryItemService.UpdateEntityAsync(inventoryItem);
                return Ok("Updated inventory item successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return Problem("Error while processing data" + ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
