using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSInventory.Business.Interfaces;
using System;
using System.Threading.Tasks;

namespace SSInventory.Web.Controllers.Apis
{
    /// <summary>
    /// Inventory Floors controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InventoryFloorsController : ControllerBase
    {
        private readonly IInventoryFloorsService _inventoryFloorsService;
        private readonly ILogger<InventoryFloorsController> _logger;

        /// <summary>
        /// Inventory Floors controller constructor
        /// </summary>
        /// <param name="inventoryFloorsService"></param>
        /// <param name="logger"></param>
        public InventoryFloorsController(IInventoryFloorsService inventoryFloorsService,
            ILogger<InventoryFloorsController> logger)
        {
            _inventoryFloorsService = inventoryFloorsService;
            _logger = logger;
        }

        /// <summary>
        /// Get floors
        /// </summary>
        /// <param name="clientId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Error when getting the data</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFloors(int clientId)
        {
            try
            {
                var data = await _inventoryFloorsService.ReadAsync(clientId);
                return Ok(data);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return BadRequest();
            }
        }
    }
}
