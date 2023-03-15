using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSInventory.Business.Interfaces;
using SSInventory.Share.Models.Dto.InventoryBuildings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Web.Controllers.Apis
{
    /// <summary>
    /// Inventory Building controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InventoryBuildingsController : ControllerBase
    {
        private readonly IInventoryBuildingsService _inventoryBuildingsService;
        private readonly ILogger<InventoryBuildingsController> _logger;

        /// <summary>
        /// Inventory Building controller constructor
        /// </summary>
        /// <param name="inventoryBuildingsService"></param>
        /// <param name="logger"></param>
        public InventoryBuildingsController(IInventoryBuildingsService inventoryBuildingsService,
            ILogger<InventoryBuildingsController> logger)
        {
            _inventoryBuildingsService = inventoryBuildingsService;
            _logger = logger;
        }

        /// <summary>
        /// Get buildings
        /// </summary>
        /// <param name="clientId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBuildings(int clientId)
        {
            try
            {
                if (clientId < 1)
                {
                    return BadRequest("Client ID is invalid");
                }
                var data = await _inventoryBuildingsService.ReadAsync(clientId);
                return Ok(data);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
