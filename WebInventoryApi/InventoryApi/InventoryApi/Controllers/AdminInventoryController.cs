using InventoryApi.Models;
using InventoryApi.Repository;
using InventoryApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminInventoryController : ControllerBase
    {
        private readonly IAdminInventoryRepository _adminInventoryRepository;
        private readonly ILoggerManagerRepository _logger;
        public AdminInventoryController(IAdminInventoryRepository adminInventoryRepository, ILoggerManagerRepository logger)
        {
            _logger = logger;
            _adminInventoryRepository = adminInventoryRepository;

            _logger.LogInfo("Initialize the Admin Inventory Controller");
        }

        [HttpGet("getadmininventory/{clientId}/{currentPage}/{perPageRows}/{startIndex}")]
        public async Task<IActionResult> GetAdminInventory(int clientId,int currentPage,int perPageRows, int startIndex)
        {
            try
            {
                var adminInventory = await _adminInventoryRepository.GetAdminInventory(clientId, currentPage,perPageRows, startIndex);
                _logger.LogInfo($"GetAdminInventory  Request in AdminInventory Controller return result with inventory successfully...");

                return Ok(adminInventory);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetAdminInventory Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetAdminInventory  Request in AdminInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

                //throw new Exception($"Exception occured to load admin inventory \n {errMsg}");
            }
        }

        [HttpGet("getadmininventoryitem/{clientId}/{inventoryId}/{buildingid}/{floorid}/{room?}")]
        public async Task<IActionResult> GetAdminInventoryItem(int clientId, int inventoryId, int buildingid, int floorid, string room)
        {
            try
            {
                var adminInventoryItem = await _adminInventoryRepository.GetAdminInventoryItem(clientId, inventoryId, buildingid, floorid, room);
                _logger.LogInfo($"GetAdminInventoryItem  Request in AdminInventory Controller return result with inventory successfully...");

                return Ok(adminInventoryItem);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetAdminInventory Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetAdminInventoryItem  Request in AdminInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //throw new Exception($"Exception occured to load admin inventory \n {errMsg}");
            }
        }

        [HttpGet("getinvuicontrolset/{clientid}/{itemtypeid}")]
        public async Task<IActionResult> GetInvUIControlSet(int clientid, int itemtypeid)
        {
            try
            {
                var adminInventoryItemTypes = await _adminInventoryRepository.GetInventoryItemTypes(clientid, itemtypeid);
                _logger.LogInfo($"GetInventoryItemTypes  Request in AdminInventory Controller return result with item types successfully...");

                return Ok(adminInventoryItemTypes);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetInventoryItemTypes Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetInventoryItemTypes  Request in AdminInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //return BadRequest($"Exception occured to load admin inventory item types \n {errMsg}");
            }
        }

        [HttpGet("getstatus")]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                var status = await _adminInventoryRepository.GetStatus();
                return Ok(status);
            }
            catch(Exception ex)
            {
                string errMsg = $"Exception occured due to call GetStatus Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetStatus  Request in AdminInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //return BadRequest(errMsg);
            }

        }

        [HttpGet("getitemtypes/{clientid}")]
        public async Task<IActionResult> GetItemTypes(int clientid)
        {
            try
            {
                var itemtypes = await _adminInventoryRepository.GetItemTypes(clientid);
                return Ok(itemtypes);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetItemTypes Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetItemTypes  Request in AdminInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);                
            }

        }

        [HttpGet("searchadmininventory/{clientId}/{currentPage}/{perPageRows}/{startIndex}")]
        public async Task<IActionResult> SearchAdminInventory(int clientId, int currentPage, int perPageRows, int startIndex, string search)
        {
            try
            {
                search = WebUtility.UrlDecode(search);

                var adminInventory = await _adminInventoryRepository.SearchAdminInventory(clientId, currentPage, perPageRows, startIndex, search);
                _logger.LogInfo($"GetAdminInventory  Request in AdminInventory Controller return result with inventory successfully...");

                return Ok(adminInventory);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetAdminInventory Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetAdminInventory  Request in AdminInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //throw new Exception($"Exception occured to load admin inventory \n {errMsg}");
            }
        }
                
        [HttpGet("getadminchildinventoryitem/{inventoryid}/{buildingid}/{floorid}/{room}/{conditionid}")]
        public async Task<IActionResult> GetAdminChildInventoryItem(int inventoryid,int buildingid, int floorid,string room,int conditionid)
        {            
            try
            {
                var adminInventoryItem = await _adminInventoryRepository.GetAdminChildInventoryItem(inventoryid,buildingid, floorid, room, conditionid);
                _logger.LogInfo($"GetAdminInventoryItem  Request in AdminInventory Controller return result with inventory successfully...");

                return Ok(adminInventoryItem);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetAdminInventory Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetAdminInventoryItem  Request in AdminInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //throw new Exception($"Exception occured to load admin inventory \n {errMsg}");
            }
        }

        [HttpGet("getadmininventorybylocation/{clientid}/{currentpage}/{perpagerows}/{startindex}/{buildingid}/{floorid}/{room?}")]
        public async Task<IActionResult> GetAdminInventoryByLocation(int clientid, int currentpage, int perpagerows, int startindex,int buildingid,int floorid,string room)
        {
            try
            {
                var adminInventory = await _adminInventoryRepository.GetAdminInventoryByLocation(clientid, currentpage, perpagerows, startindex, buildingid, floorid, room);
                _logger.LogInfo($"GetAdminInventoryByLocation  Request in AdminInventory Controller return result with inventory successfully...");

                return Ok(adminInventory);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetAdminInventory Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetAdminInventoryByLocation  Request in AdminInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //return BadRequest($"Exception occured to load admin inventory \n {errMsg}");
            }
        }

        [HttpPost("fileupload/{clientId}")]
        public async Task<IActionResult> FileUpload(int clientId)
        {
            bool result = false;
            try
            {
                var files = Request.Form.Files;
                foreach(var file in files)
                {
                    result = await _adminInventoryRepository.UploadFileToS3Bucket((IFormFile)file, clientId);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call FileUpload Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"FileUpload  Request in AdminInventory Controller=>{errMsg}");
               // throw new Exception($"Exception occured to upload file \n {errMsg}");

                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //return BadRequest(errMsg);
            }
        }

        [HttpPost("imageupload/{clientId}/{inventoryId}/{userId}")]
        public async Task<IActionResult> ImageUpload(int clientId,int inventoryId, int userId)
        {
            string result="";
            try
            {
                var files = Request.Form.Files;

                foreach(var file in files)
                {
                    result = await _adminInventoryRepository.UploadImageToS3Bucket(file, clientId, inventoryId, userId);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call ImageUpload Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"ImageUpload  Request in AdminInventory Controller=>{errMsg}");
                // throw new Exception($"Exception occured to upload file \n {errMsg}");

                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //return BadRequest(errMsg);
            }
        }

        [HttpPost("createinventory/{userid}/{clientid}")]
        public async Task<IActionResult> CreateInventory(int userid,int clientid,[FromBody]CreateInventoryModel createInventory)
        {
            try
            {
                //var createInventory = JsonConvert.DeserializeObject<CreateInventoryModel>(jsonbody);              

                var result = await _adminInventoryRepository.CreateInventory(userid,clientid, createInventory);
                _logger.LogInfo($"CreateInventory  Request in AdminInventory Controller return result successfully...");

                return Ok(result);
            }
            catch(Exception ex)
            {
                string errMsg = $"Exception occured due to call CreateInventory Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"CreateInventory  Request in AdminInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //return BadRequest($"Exception occured to update admin inventory \n {errMsg}");
            }
        }

        //[HttpPut("updateadmininventory/{inventoryId}/{column}/{value}")]
        [HttpPut("updateadmininventory/{inventoryId}/{column}/{userId}")]
        public async Task<IActionResult> UpdateAdminInventory(int inventoryId,string column, int userId, [FromBody] string value)
        {
            try
            {
                var adminInventory = await _adminInventoryRepository.UpdateAdminInventory(inventoryId, column, value,userId);
                _logger.LogInfo($"UpdateAdminInventory  Request in AdminInventory Controller return result successfully...");

                return Ok(adminInventory);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call UpdateAdminInventory Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"UpdateAdminInventory  Request in AdminInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //throw new Exception($"Exception occured to update admin inventory \n {errMsg}");
            }
        }

        [HttpPut("updateadmininventoryitem/{column}/{value}/{userId}")]
        public async Task<IActionResult> UpdateAdminInventoryItem(string column, int value,int userId,[FromBody] InventoryItemModel inventoryItemModel)
        {
            try
            {
                var adminInventory = await _adminInventoryRepository.UpdateAdminInventoryItem(inventoryItemModel, column, value,userId);
                _logger.LogInfo($"UpdateAdminInventoryItem  Request in AdminInventory Controller return result successfully...");

                return Ok(adminInventory);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call UpdateAdminInventoryItem Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"UpdateAdminInventoryItem  Request in AdminInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //throw new Exception($"Exception occured to update admin inventory \n {errMsg}");
            }
        }

        [HttpPut("editinventory/{userid}/{apicallurl}")]
        public async Task<IActionResult> EditInventory(int userid, string apicallurl, [FromBody]AdminInventory adminInventory)
        {
            try
            {

                // var adminInventory = JsonConvert.DeserializeObject<AdminInventory>(jsonbody);              
                apicallurl = WebUtility.UrlDecode(apicallurl);

                var result = await _adminInventoryRepository.EditInventory(userid, apicallurl, adminInventory);
                _logger.LogInfo($"EditInventory  Request in AdminInventory Controller return result successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call EditInventory Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"EditInventory  Request in AdminInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //throw new Exception($"Exception occured to update admin inventory \n {errMsg}");
            }

        }

        [HttpPut("editinventoryitem/{userid}/{prevQty}/{apicallurl}")]
        public async Task<IActionResult> EditInventoryItem(int userid,int prevQty,string apicallurl, [FromBody] InventoryItemModel inventoryItemModel)
        {
            try
            {
                apicallurl = WebUtility.UrlDecode(apicallurl);
                // var adminInventory = JsonConvert.DeserializeObject<AdminInventory>(jsonbody);              

                var result = await _adminInventoryRepository.EditInventoryItem(userid,prevQty, apicallurl, inventoryItemModel);
                _logger.LogInfo($"EditInventoryItem  Request in AdminInventory Controller return result successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call EditInventoryItem Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"EditInventoryItem  Request in AdminInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //throw new Exception($"Exception occured to update admin inventory items\n {errMsg}");
            }

        }
    }
}
