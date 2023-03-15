using InventoryApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryManufacturerController : ControllerBase
    {
        private readonly ILoggerManagerRepository _logger;
        private readonly IInventoryManufacturerRepository _inventoryManufacturerRepository;

        public InventoryManufacturerController(IInventoryManufacturerRepository inventoryManufacturerRepository, ILoggerManagerRepository logger)
        {
            _inventoryManufacturerRepository = inventoryManufacturerRepository;
            _logger = logger;
        }

        [HttpGet("getmanufacturers")]
        public async Task<IActionResult> GetManufacturers(string search)
        {
            try
            {
                //pmTypes: new List<string> { "P", "F", "X" }
                var manufacturers = await _inventoryManufacturerRepository.GetManufacturers(search);

                _logger.LogInfo($"GetManufacturers  Request in ManufacturersInventory Controller return result with inventory successfully...");

                
                return Ok(manufacturers);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetManufacturers Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetManufacturers  Request in ManufacturersInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }

        }
    }
}
