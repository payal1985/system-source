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
    public class InventoryItemController : ControllerBase
    {

        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly ILoggerManagerRepository _logger;


        public InventoryItemController(IInventoryItemRepository inventoryItemRepository, ILoggerManagerRepository logger)
        {
            //logger = LogManager.GetCurrentClassLogger();
            _logger = logger;
            _inventoryItemRepository = inventoryItemRepository;

            _logger.LogInfo("Initialize the Inventory Item Controller");
        }

        [HttpGet("getbuildings")]
        public List<string> GetBuildings(int client_id)
        {
            try
            {
                var buldings = _inventoryItemRepository.GetBuilding(client_id).ToList();
                _logger.LogInfo($"GetBuildings  Request in InventoryItem Controller return result with buldings successfully...");

                return buldings;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetBuildings Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetBuildings  Request in InventoryItem Controller=>{errMsg}");
                throw new Exception($"Exception occured to load buldings \n {errMsg}");               
            }
        }

        [HttpGet("getfloor")]
        public List<string> GetFloor(int client_id)
        {
            try
            {
                var floor = _inventoryItemRepository.GetFloor(client_id).ToList();
                _logger.LogInfo($"GetFloor  Request in InventoryItem Controller return result with floor successfully...");

                return floor;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetFloor Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetFloor  Request in InventoryItem Controller=>{errMsg}");
                throw new Exception($"Exception occured to load floor \n {errMsg}");
                
            }
        }

        [HttpGet("getrooms")]
        public List<string> GetRooms(int client_id)
        {
            try
            {
                var floor = _inventoryItemRepository.GetRoom(client_id).ToList();
                _logger.LogInfo($"GetFloor  Request in InventoryItem Controller return result with floor successfully...");

                return floor;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetFloor Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetFloor  Request in InventoryItem Controller=>{errMsg}");
                throw new Exception($"Exception occured to load floor \n {errMsg}");

            }
        }

        [HttpGet("getconditions")]
        public List<string> GetConditions()
        {
            try
            {
                var conditions = _inventoryItemRepository.GetCondition();
                _logger.LogInfo($"GetConditions  Request in InventoryItem Controller return result with conditions successfully...");

                return conditions;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetConditions Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetConditions  Request in InventoryItem Controller=>{errMsg}");
                throw new Exception($"Exception occured to load conditions \n {errMsg}");

            }
        }


        [HttpGet("getdepcostcenters")]
        public List<string> GetDepCostCenters()
        {
            try
            {
                var depcostcenter = _inventoryItemRepository.GetDepCostCenter();
                _logger.LogInfo($"GetDepCostCenters  Request in InventoryItem Controller return result with department/cost centers successfully...");

                return depcostcenter;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetDepCostCenters Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetDepCostCenters  Request in InventoryItem Controller=>{errMsg}");
                throw new Exception($"Exception occured to load department/cost centers \n {errMsg}");

            }
        }

        [HttpPost("insertbuilding")]
        public async Task<IActionResult> InsertBuilding([FromBody] InventoryBuildingsModel inventoryBuildingsModel)
        {
            bool result = false;

            try
            {
                result = await _inventoryItemRepository.InsertBuilding(inventoryBuildingsModel);
                //result = true;
                _logger.LogInfo($"InsertBuilding  Request in InventoryItem Controller return result...{result}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call InsertBuilding Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"InsertBuilding  Request in InventoryItem Controller=>{errMsg}");
                throw new Exception($"Exception occured to insert building \n {errMsg}");

            }
        }

        [HttpPost("insertfloor")]
        public async Task<IActionResult> InsertFloor([FromBody] InventoryFloorsModel inventoryFloorsModel)
        {
            bool result = false;

            try
            {
                result = await _inventoryItemRepository.InsertFloor(inventoryFloorsModel);
                //result = true;
                _logger.LogInfo($"InsertFloor  Request in InventoryItem Controller return result...{result}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call InsertFloor Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"InsertFloor  Request in InventoryItem Controller=>{errMsg}");
                throw new Exception($"Exception occured to insert floor \n {errMsg}");
            }
        }

    }
}
