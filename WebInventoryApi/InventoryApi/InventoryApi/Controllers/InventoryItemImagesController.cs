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
    public class InventoryItemImagesController : ControllerBase
    {

        private readonly IInventoryItemImageRepository _inventoryItemImageRepository;
        private readonly ILoggerManagerRepository _logger;


        public InventoryItemImagesController(IInventoryItemImageRepository inventoryItemImageRepository, ILoggerManagerRepository logger)
        {
            //logger = LogManager.GetCurrentClassLogger();
            _logger = logger;
            _inventoryItemImageRepository = inventoryItemImageRepository;

            _logger.LogInfo("Initialize the Inventory Item Image Controller");
        }

        [HttpGet("getinventoryitemimages")]
        public async Task<IActionResult> GetInventoryItemImages(int inv_id)
        {
            try
            {
                var invItemImages = await _inventoryItemImageRepository.GetInventoryItemImages(inv_id);
                _logger.LogInfo($"GetInventoryItemImages  Request in InventoryItemImages Controller return result with inventory item images successfully...");

                return Ok(invItemImages);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetInventoryItemImages Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetInventoryItemImages  Request in InventoryItemImages Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //throw new Exception($"Exception occured to load inventory item images \n {errMsg}");
                
            }
        }

        [HttpGet("getinvitemimagesforcond/{inv_item_id}/{conditionid}/{inventoryid}")]
        public async Task<IActionResult> GetInventoryItemImagesForCond(int inv_item_id,int conditionid,int inventoryid)
        {
            try
            {
                var invItemImages = await _inventoryItemImageRepository.GetInventoryItemImagesForCondition(inv_item_id,conditionid, inventoryid);
                _logger.LogInfo($"GetInventoryItemImagesForCond  Request in InventoryItemImages Controller return result with inventory item images successfully...");

                return Ok(invItemImages);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetInventoryItemImagesForCond Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetInventoryItemImagesForCond  Request in InventoryItemImages Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //throw new Exception($"Exception occured to load inventory item images \n {errMsg}");

            }
        }
    }
}
