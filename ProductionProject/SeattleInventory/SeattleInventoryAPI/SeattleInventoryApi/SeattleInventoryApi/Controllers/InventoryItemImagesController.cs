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
        public List<InventoryItemImagesModel> GetInventoryItemImages(int inv_id)
        {
            try
            {
                var invItemImages = _inventoryItemImageRepository.GetInventoryItemImages(inv_id).ToList();
                _logger.LogInfo($"GetInventoryItemImages  Request in InventoryItemImages Controller return result with inventory item images successfully...");

                return invItemImages;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetInventoryItemImages Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetInventoryItemImages  Request in InventoryItemImages Controller=>{errMsg}");
                throw new Exception($"Exception occured to load inventory item images \n {errMsg}");
                
            }
        }

        [HttpGet("getinvitemimagesforcond")]
        public List<InventoryItemImagesModel> GetInventoryItemImagesForCond(int inv_item_id,string condition)
        {
            try
            {
                var invItemImages = _inventoryItemImageRepository.GetInventoryItemImagesForCondition(inv_item_id,condition).ToList();
                _logger.LogInfo($"GetInventoryItemImagesForCond  Request in InventoryItemImages Controller return result with inventory item images successfully...");

                return invItemImages;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetInventoryItemImagesForCond Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetInventoryItemImagesForCond  Request in InventoryItemImages Controller=>{errMsg}");
                throw new Exception($"Exception occured to load inventory item images \n {errMsg}");

            }
        }
    }
}
