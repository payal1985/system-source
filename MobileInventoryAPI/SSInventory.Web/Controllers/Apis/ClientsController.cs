using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SSInventory.Web.Models.Client.External;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SSInventory.Web.Controllers
{
    /// <summary>
    /// Clients controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : CommonController
    {
        private readonly ILogger<ClientsController> _logger;
        private readonly IConfiguration _config;

        /// <summary>
        /// Client controller constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="config"></param>
        public ClientsController(ILogger<ClientsController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        /// <summary>
        /// Get list clients
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="204">No content</response>
        /// <response code="400">Error when getting the data</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        public async Task<IActionResult> Get(int? userId)
        {
            try
            {
                if(userId.GetValueOrDefault(0) < 1)
                {
                    return BadRequest("Invalid data");
                }

                return await GetClientsFromSSIDB(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return Problem("Error occurred while getting image information" + ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<IActionResult> GetClientsFromSSIDB(int? userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_config["ExternalSystem:Domain"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"api/Login/getclients/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return Ok(JsonConvert.DeserializeObject<List<ClientExternalResponseModel>>(result));
                }
            }

            return NoContent();
        }
    }
}
