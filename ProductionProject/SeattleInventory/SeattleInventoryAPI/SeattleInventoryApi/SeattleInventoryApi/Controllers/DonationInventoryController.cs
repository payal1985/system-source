using Microsoft.AspNetCore.Mvc;
using NLog;
using SeattleInventoryApi.Models;
using SeattleInventoryApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeattleInventoryApi.Controllers
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


        [HttpGet("getinventories")]
        public List<DonationInventoryModel> GetInventories(string category)
        {
            try
            {
                // var inventories = _inventoryRepository.GetInventory1(category,building,floor).ToList();
                var inventories = _donationInventoryRepository.GetInventory(category);
                _logger.LogInfo($"GetInventories  Request in DonationInventoryController return result with inventories successfully...");

                return inventories;
            }
            catch (Exception ex)
            {
                //throw new Exception($"Exception occured to load category {ex.Message} \n {ex.StackTrace}");
                string errMsg = $"Exception occured due to call GetInventories Request in DonationInventoryController=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetInventories  Request in Inventory Controller=>{errMsg}");
                throw new Exception($"Exception occured to load category \n {errMsg}");
            }
        }

        [HttpGet("getcategory")]
        public List<string> GetInventoryCategory()
        {
            try
            {
                var inventoryCategories = _donationInventoryRepository.GetInventoryCategory();
                _logger.LogInfo($"GetInventoryCategory  Request in DonationInventoryController return result with inventories categories successfully...");

                return inventoryCategories;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call Get Inventory Category Request in DonationInventoryController=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetInventoryCategory  Request in DonationInventoryController=>{errMsg}");
                throw new Exception($"Exception occured to load category \n {errMsg}");

            }
        }


        
    }
}
