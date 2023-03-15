using Microsoft.AspNetCore.Mvc;
using NLog;
using InventoryApi.DBModels;
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
        public async Task<IActionResult> GetInventories(int itemTypeId,int client_id,int building,int floor,string room,int cond,int startindex)
        {
            try
            {
               // var inventories = _inventoryRepository.GetInventory1(category,building,floor).ToList();
                var inventories = await _inventoryRepository.GetInventory(itemTypeId, client_id,building, floor,room, cond, startindex);
                _logger.LogInfo($"GetInventories  Request in Inventory Controller return result with inventories successfully...");

                return Ok(inventories);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call Get Inventories Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetInventories  Request in Inventory Controller=>{errMsg}");
                //throw new Exception($"Exception occured to load inventories \n {errMsg}");

                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpGet("getcategory")]
        public async Task<IActionResult> GetInventoryCategory(int client_id)
        {
            try
            {
                var inventoryCategories = await _inventoryRepository.GetInventoryCategory(client_id);
                _logger.LogInfo($"GetInventoryCategory  Request in Inventory Controller return result with inventories categories successfully...");

                return Ok(inventoryCategories);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call Get Inventory Category Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetInventoryCategory  Request in Inventory Controller=>{errMsg}");
                //throw new Exception($"Exception occured to load category \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }

        [HttpGet("searchinventories")]
        public async Task<IActionResult> SearchInventories(string search, int client_id, int building, int floor, string room, int cond)
        {


            try
            {
                // var inventories = _inventoryRepository.GetInventory1(category,building,floor).ToList();
                var inventories = await _inventoryRepository.SearchInventory(search, client_id, building, floor, room, cond);
                _logger.LogInfo($"SearchInventories  Request in Inventory Controller return result with inventories successfully...");

                return Ok(inventories);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call Search Inventories Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"SearchInventories  Request in Inventory Controller=>{errMsg}");
               // throw new Exception($"Exception occured to load inventories \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpGet("getitemtypeslist/{clientid}")]
        public async Task<IActionResult> GetItemsTypesList(int clientid)
        {
            try
            {
                var inventorySpaceTypes = await _inventoryRepository.GetSpecificInventoryItemsTypes(clientid);
                _logger.LogInfo($"GetSpaceTypes  Request in Inventory Controller return result with inventories categories successfully...");

                return Ok(inventorySpaceTypes);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetSpaceTypes Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetSpaceTypes  Request in Inventory Controller=>{errMsg}");
               // throw new Exception($"Exception occured to load space types \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpGet("getitemtypesinventories/{spacetypeid}/{ownershipid}/{clientid}/{buildingid}/{floorid}")]
        public async Task<IActionResult> GetItemTypesInventories(int spacetypeid,int ownershipid,int clientid, int buildingid, int floorid, string room, int condition)
        {
            try
            { 
                var inventories = await _inventoryRepository.GetItemTypesInventoy(spacetypeid, ownershipid, clientid, buildingid, floorid, room,condition);
                _logger.LogInfo($"GetItemTypesInventories  Request in Inventory Controller return result with inventories successfully...");

                return Ok(inventories);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call Get GetItemTypesInventories Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetItemTypesInventories  Request in Inventory Controller=>{errMsg}");
                //throw new Exception($"Exception occured to load inventories \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }
    
        [HttpPost("getavailtooltip")]
        public async Task<IActionResult> GetAvailTooltip([FromBody]List<InventoryItemModel> model)
        {
            try
            {
                var tooltip = await _inventoryRepository.GetAvailTooltip(model);
                _logger.LogInfo($"GetAvailTooltip  Request in Inventory Controller return result successfully...");

                return Ok(tooltip);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetAvailTooltip Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetAvailTooltip  Request in Inventory Controller=>{errMsg}");
                //return BadRequest($"Exception occured to load tooltip \n {errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpPost("getrestooltip")]
        public async Task<IActionResult> GetResTooltip([FromBody] List<InventoryItemModel> model)
        {
            try
            {
                var tooltip = await _inventoryRepository.GetResTooltip(model);
                _logger.LogInfo($"GetResTooltip  Request in Inventory Controller return result successfully...");

                return Ok(tooltip);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetResTooltip Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetResTooltip  Request in Inventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //return BadRequest($"Exception occured to load tooltip \n {errMsg}");
            }
        }

        [HttpPost("getcartlist")]
        public async Task<IActionResult> GetCartList([FromBody]InventoryModel model)
        {
            try
            {
                var cartlist = await _inventoryRepository.GetCartList(model);
                _logger.LogInfo($"GetCartList  Request in Inventory Controller return result successfully...");

                return Ok(cartlist);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetCartList Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetCartList  Request in Inventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //return BadRequest($"Exception occured to load cart list \n {errMsg}");
            }
        }
    }
}
