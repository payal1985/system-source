using Microsoft.AspNetCore.Mvc;
using NLog;
using InventoryApi.Models;
using InventoryApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryApi.Repository.Interfaces;
using Newtonsoft.Json;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryApi.Controllers
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
        public async Task<IActionResult> GetBuildings(int client_id)
        {
            try
            {
                var buldings = await _inventoryItemRepository.GetBuilding(client_id);
                _logger.LogInfo($"GetBuildings  Request in InventoryItem Controller return result with buldings successfully...");

                return Ok(buldings);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetBuildings Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetBuildings  Request in InventoryItem Controller=>{errMsg}");
                //throw new Exception($"Exception occured to load buldings \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);                
            }
        }

        [HttpGet("getcartbuildings/{clientid}")]
        public async Task<IActionResult> GetCartBuildings(int clientid)
        {
            try
            {
                var buldings = await _inventoryItemRepository.GetCartBuilding(clientid);
                _logger.LogInfo($"GetCartBuildings  Request in InventoryItem Controller return result with buldings successfully...");

                return Ok(buldings);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetCartBuildings Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetCartBuildings  Request in InventoryItem Controller=>{errMsg}");
                //throw new Exception($"Exception occured to load buldings \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }


        [HttpGet("getfloor")]
        public async Task<IActionResult> GetFloor(int client_id)
        {
            try
            {
                var floor = await _inventoryItemRepository.GetFloor(client_id);
                _logger.LogInfo($"GetFloor  Request in InventoryItem Controller return result with floor successfully...");

                return Ok(floor);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetFloor Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetFloor  Request in InventoryItem Controller=>{errMsg}");
                //throw new Exception($"Exception occured to load floor \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }

        [HttpGet("getrooms")]
        public async Task<IActionResult> GetRooms(int client_id)
        {
            try
            {
                var floor = await _inventoryItemRepository.GetRoom(client_id);
                _logger.LogInfo($"GetFloor  Request in InventoryItem Controller return result with floor successfully...");

                return Ok(floor);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetFloor Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetFloor  Request in InventoryItem Controller=>{errMsg}");
                //throw new Exception($"Exception occured to load floor \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }

        [HttpGet("getconditions")]
        public async Task<IActionResult> GetConditions()
        {
            try
            {
                var conditions = await _inventoryItemRepository.GetCondition();
                _logger.LogInfo($"GetConditions  Request in InventoryItem Controller return result with conditions successfully...");

                return Ok(conditions);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetConditions Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetConditions  Request in InventoryItem Controller=>{errMsg}");
                //throw new Exception($"Exception occured to load conditions \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }


        [HttpGet("getdepcostcenters")]
        public IActionResult GetDepCostCenters()
        {
            try
            {
                var depcostcenter = _inventoryItemRepository.GetDepCostCenter();
                _logger.LogInfo($"GetDepCostCenters  Request in InventoryItem Controller return result with department/cost centers successfully...");

                return Ok(depcostcenter);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetDepCostCenters Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetDepCostCenters  Request in InventoryItem Controller=>{errMsg}");
                // throw new Exception($"Exception occured to load department/cost centers \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }

        [HttpPost("insertbuilding")]
        public async Task<IActionResult> InsertBuilding([FromBody] InventoryBuildingsModel inventoryBuildingsModel)
        {
            //bool result = false;

            try
            {
                var result = await _inventoryItemRepository.InsertBuilding(inventoryBuildingsModel);
                //result = true;
                _logger.LogInfo($"InsertBuilding  Request in InventoryItem Controller return result...{result}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call InsertBuilding Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"InsertBuilding  Request in InventoryItem Controller=>{errMsg}");
                // throw new Exception($"Exception occured to insert building \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }

        [HttpPost("insertfloor")]
        public async Task<IActionResult> InsertFloor([FromBody] InventoryFloorsModel inventoryFloorsModel)
        {
            //bool result = false;

            try
            {
                var result = await _inventoryItemRepository.InsertFloor(inventoryFloorsModel);
                //result = true;
                _logger.LogInfo($"InsertFloor  Request in InventoryItem Controller return result...{result}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call InsertFloor Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"InsertFloor  Request in InventoryItem Controller=>{errMsg}");
                //throw new Exception($"Exception occured to insert floor \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }

        [HttpGet("getwarrantyinventoryitems/{inventoryid}/{warrantyyear}")]
        public async Task<IActionResult> GetWarrantyInventoryItems(int inventoryid,int warrantyyear)
        {
            try
            {
                var warrantyInventoryItems = await _inventoryItemRepository.GetWarranytDisplayItems(inventoryid, warrantyyear);
                _logger.LogInfo($"GetWarrantyInventoryItems  Request in InventoryItem Controller return result with warranty inventoryitems successfully...");

                return Ok(warrantyInventoryItems);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetWarrantyInventoryItems Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetWarrantyInventoryItems  Request in InventoryItem Controller=>{errMsg}");
                //throw new Exception($"Exception occured to load warranty inventoryitems \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }

        //[HttpPut("updateinventoryitems/{inventoryitemid}/{totalrow}")]
        //public async Task<bool> UpdateInventoryItems(int inventoryitemid,int totalrow)
        [HttpPut("updateinventoryitems")]
        public async Task<IActionResult> UpdateInventoryItems([FromBody] List<InventoryCartOrderItemModel> cartOrderItemModels)
        {
            string result = "";
            try
            {
                foreach(var item in cartOrderItemModels)
                {
                    if(item.ChildInventoryItemModels != null && item.ChildInventoryItemModels.Count > 0)
                    {
                        var entity = item.ChildInventoryItemModels.Where(ch => ch.IsSelected).ToList();

                        foreach (var e in entity)
                        {
                            result = await _inventoryItemRepository.UpdateInventoryItems(e.InventoryItemID, e.Qty);
                            if (!result.Contains("success"))
                                break;
                        }
                    }
                    else
                    {
                        result = await _inventoryItemRepository.UpdateInventoryItems(item.InventoryItemID, item.PullQty, item.ConditionId);
                        if (!result.Contains("success"))
                            break;
                    }

                    //result = await _inventoryItemRepository.UpdateInventoryItems(item.InventoryItemID,item.PullQty);
                    if (!result.Contains("success"))
                        break;
                }
               // var result = await _inventoryItemRepository.UpdateInventoryItems(inventoryitemid, totalrow);
                //var result = await _inventoryItemRepository.UpdateInventoryItems(inventoryitemid, totalrow);
                _logger.LogInfo($"UpdateInventoryItems  Request in InventoryItem Controller return result successfully...{result}");
                //return JsonResult(result);
               
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call UpdateInventoryItems Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"UpdateInventoryItems  Request in InventoryItem Controller=>{errMsg}");
                //throw new Exception($"Exception occured to update inventoryitems \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }
    
        [HttpGet("getstatus")]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                var status = await _inventoryItemRepository.GetStatus();
                return Ok(status);
            }
            catch(Exception ex)
            {
                string errMsg = $"Exception occured due to call GetStatus Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetStatus  Request in InventoryItem Controller=>{errMsg}");
                //throw new Exception($"Exception occured to load status \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }

        [HttpGet("getchildinventoryitem")]
        public async Task<IActionResult> GetChildInventoryItems(string inventoryitemsjson)
        {
            try
            {
                var inventoryitems = JsonConvert.DeserializeObject<InventoryCartOrderItemModel>(inventoryitemsjson);

                var childInventoryItems = await _inventoryItemRepository.GetChildInventoryItems(inventoryitems);
                return Ok(childInventoryItems);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetChildInventoryItems Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetChildInventoryItems  Request in InventoryItem Controller=>{errMsg}");
                //throw new Exception($"Exception occured to load child inventory \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }
    
    }
}
