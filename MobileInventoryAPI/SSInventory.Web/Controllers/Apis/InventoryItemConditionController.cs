using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSInventory.Business.Interfaces;
using System.Threading.Tasks;

namespace SSInventory.Web.Controllers.Apis
{
    /// <summary>
    /// Inventory Item Condition API
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InventoryItemConditionController : ControllerBase
    {
        private readonly IInventoryItemConditionService _inventoryItemConditionService;
        private readonly ILogger<InventoryItemConditionController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inventoryItemConditionService"></param>
        /// <param name="logger"></param>
        public InventoryItemConditionController(IInventoryItemConditionService inventoryItemConditionService,
            ILogger<InventoryItemConditionController> logger)
        {
            _inventoryItemConditionService = inventoryItemConditionService;
            _logger = logger;
        }

        /// <summary>
        /// Get inventory item conditions
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Error when getting the data</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetConditions()
        {
            try
            {
                var data = await _inventoryItemConditionService.ReadAsync();
                return Ok(data);
            }
            catch (System.Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return BadRequest();
            }
        }
    }
}
