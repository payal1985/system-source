using Microsoft.AspNetCore.Mvc;
using NLog;
using InventoryApi.Models;
using InventoryApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryApi.Repository.Interfaces;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationInventoryController : ControllerBase
    {

        private readonly IDonationInventoryRepository _donationInventoryRepository;
        private readonly ILoggerManagerRepository _logger;


        public DonationInventoryController(IDonationInventoryRepository donationInventoryRepository, ILoggerManagerRepository logger)
        {
            //logger = LogManager.GetCurrentClassLogger();
            _logger = logger;
            _donationInventoryRepository = donationInventoryRepository;
       
            _logger.LogInfo("Initialize the Donation Inventory Controller");
        }


        [HttpGet("getinventories/{categoryid}")]
        public async Task<IActionResult> GetInventories(int categoryid)
        {
            try
            {
                // var inventories = _inventoryRepository.GetInventory1(category,building,floor).ToList();
                var inventories = await _donationInventoryRepository.GetInventory(categoryid);
                _logger.LogInfo($"GetInventories  Request in DonationInventoryController return result with inventories successfully...");

                return Ok(inventories);
            }
            catch (Exception ex)
            {
                //throw new Exception($"Exception occured to load category {ex.Message} \n {ex.StackTrace}");
                string errMsg = $"Exception occured due to call GetInventories Request in DonationInventoryController=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetInventories  Request in Inventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //throw new Exception($"Exception occured to load category \n {errMsg}");
            }
        }

        [HttpGet("getcategory")]
        public async Task<IActionResult> GetInventoryCategory()
        {
            try
            {
                var inventoryCategories = await _donationInventoryRepository.GetInventoryCategory();
                _logger.LogInfo($"GetInventoryCategory  Request in DonationInventoryController return result with inventories categories successfully...");

                return Ok(inventoryCategories);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call Get Inventory Category Request in DonationInventoryController=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetInventoryCategory  Request in DonationInventoryController=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //throw new Exception($"Exception occured to load category \n {errMsg}");

            }
        }


        
    }
}
