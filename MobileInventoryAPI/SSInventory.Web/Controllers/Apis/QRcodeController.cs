using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSInventory.Business.Interfaces;
using SSInventory.Core.Models;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.InventoryItem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SSInventory.Web.Controllers.Apis
{
    /// <summary>
    /// QRcode API
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QRcodeController : ControllerBase
    {
        private readonly ILogger<QRcodeController> _logger;
        private readonly IInventoryService _inventoryService;
        private readonly IInventoryItemService _inventoryItemService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// QRcode constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inventoryService"></param>
        /// <param name="inventoryItemService"></param>
        /// <param name="webHostEnvironment"></param>
        public QRcodeController(ILogger<QRcodeController> logger,
            IInventoryService inventoryService,
            IInventoryItemService inventoryItemService,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _inventoryService = inventoryService;
            _inventoryItemService = inventoryItemService;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Download inventory information as QRcode
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <response code="404">Data is not found</response>
        /// <response code="500">Data is not found</response>
        /// <response code="200">Image file</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        protected async Task<IActionResult> InventoryQRcode(int inventoryId)
        {
            try
            {
                var inventories = await _inventoryService.ReadAsync(new SearchInventoryModel
                {
                    Ids = new List<int>
                {
                    inventoryId
                }
                });
                if (inventories?.Any() != true)
                    return NotFound("Not found inventory");

                byte[] bytes = Convert.FromBase64String(inventories[0].QRcode);

                return File(bytes, "image/png", $"Inventory-{inventoryId}.png");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Download inventory item information as QRcode
        /// </summary>
        /// <param name="inventoryItemId"></param>
        /// <response code="200">Image file</response>
        /// <response code="400">Invalid data</response>
        /// <response code="404">Data is not found</response>
        /// <response code="500">Error while processing data</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InventoryItemQRcode(int inventoryItemId)
        {
            try
            {
                if (inventoryItemId < 1 || inventoryItemId > int.MaxValue)
                    return BadRequest("Invalid inventory item ID");

                var filter = new InventoryItemFilterModel
                {
                    Ids = new List<int> { inventoryItemId }
                };
                var inventoryItems = await _inventoryItemService.ReadAsync(filter);
                if(inventoryItems?.Any() != true)
                    return NotFound("Not found inventory item data");

                var path = Path.Combine(_webHostEnvironment.ContentRootPath, inventoryItems[0].QRcode);

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, MediaTypeNames.Image.Jpeg, Path.GetFileName(path));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
