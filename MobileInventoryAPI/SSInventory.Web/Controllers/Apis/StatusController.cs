using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SSInventory.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Web.Controllers.Apis
{
    /// <summary>
    /// Status API
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;
        private readonly IStatusService _statusService;

        /// <summary>
        /// Status API constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="statusService"></param>
        public StatusController(ILogger<StatusController> logger,
            IStatusService statusService)
        {
            _logger = logger;
            _statusService = statusService;
        }

        /// <summary>
        /// Search status group
        /// </summary>
        /// <param name="statusType"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="404">Data not found</response>
        /// <response code="500">Error when getting the data</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStatusGroup(string statusType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(statusType))
                    return BadRequest("Status can not empty");

                var statuses = await _statusService.ReadAsync(types: new List<string> { statusType });
                if (statuses?.Any() != true)
                    return NotFound("No data found");

                return Ok(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return Problem("Error occurred while getting the information" + ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
