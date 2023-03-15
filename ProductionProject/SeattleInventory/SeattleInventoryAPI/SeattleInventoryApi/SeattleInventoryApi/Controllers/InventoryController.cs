using Microsoft.AspNetCore.Mvc;
using NLog;
using SeattleInventoryApi.DBModels;
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
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository  _inventoryRepository;
        
        private readonly ILoggerManagerRepository _logger;

        public InventoryController(IInventoryRepository inventoryRepository, ILoggerManagerRepository logger)
        {
            //logger = LogManager.GetCurrentClassLogger();
            _logger = logger;
            _inventoryRepository = inventoryRepository;

            _logger.LogInfo("Initialize the Inventory Controller");

        }

        [HttpGet("getinventories")]
        public async Task<List<InventoryModel>> GetInventories(string category,int client_id,string building,string floor,string room,string cond)
        {
            

            try
            {
               // var inventories = _inventoryRepository.GetInventory1(category,building,floor).ToList();
                var inventories = await _inventoryRepository.GetInventory(category, client_id,building, floor,room, cond);
                _logger.LogInfo($"GetInventories  Request in Inventory Controller return result with inventories successfully...");

                return inventories;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call Get Inventories Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetInventories  Request in Inventory Controller=>{errMsg}");
                throw new Exception($"Exception occured to load inventories \n {errMsg}");
            }
        }

        [HttpGet("getcategory")]
        public async Task<List<string>> GetInventoryCategory(int client_id)
        {
            try
            {
                var inventoryCategories = await _inventoryRepository.GetInventoryCategory(client_id);
                _logger.LogInfo($"GetInventoryCategory  Request in Inventory Controller return result with inventories categories successfully...");

                return inventoryCategories;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call Get Inventory Category Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetInventoryCategory  Request in Inventory Controller=>{errMsg}");
                throw new Exception($"Exception occured to load category \n {errMsg}");
            }
        }

        [HttpGet("searchinventories")]
        public async Task<List<InventoryModel>> SearchInventories(string search, int client_id, string building, string floor, string room, string cond)
        {


            try
            {
                // var inventories = _inventoryRepository.GetInventory1(category,building,floor).ToList();
                var inventories = await _inventoryRepository.SearchInventory(search, client_id, building, floor, room, cond);
                _logger.LogInfo($"SearchInventories  Request in Inventory Controller return result with inventories successfully...");

                return inventories;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call Search Inventories Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"SearchInventories  Request in Inventory Controller=>{errMsg}");
                throw new Exception($"Exception occured to load inventories \n {errMsg}");
            }
        }

    }
}
