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
using System.Text.Json;
using System.Threading.Tasks;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemWarrantyController : ControllerBase
    {
        private readonly IInventoryItemWarrantyRepository _inventoryItemWarrantyRepository;      
        private readonly ILoggerManagerRepository _logger;


        public InventoryItemWarrantyController(IInventoryItemWarrantyRepository inventoryItemWarrantyRepository
                                               ,ILoggerManagerRepository logger
                                            )
        {
            _logger = logger;
            _inventoryItemWarrantyRepository = inventoryItemWarrantyRepository;

            _logger.LogInfo("Initialize the Inventory Item Order Controller");

        }
        [HttpPost("savewarrantyrequest/{apicallurl}")]
        //public async Task<IActionResult> SaveWarrantyRequest(string apicallurl,[FromBody] List<InventoryItemWarrantyModel> listInventoryItemWarranty)
        public async Task<IActionResult> SaveWarrantyRequest(string apicallurl, IFormCollection data)
        {
            // bool result = false;
            try
            {
                apicallurl = WebUtility.UrlDecode(apicallurl);

                var warrantdata = data["warrantydatasource"];
                var files = Request.Form.Files;
                Dictionary<int, List<IFormFile>> dictFilePairs = new Dictionary<int, List<IFormFile>>();

                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].Length > 0)
                    {
                        if (dictFilePairs.ContainsKey(Convert.ToInt16(data["rowindex"][i])))
                            dictFilePairs[Convert.ToInt16(data["rowindex"][i])].Add(files[i]);
                        else
                        {
                            var newList = new List<IFormFile>();
                            newList.Add(files[i]);
                            dictFilePairs.Add(Convert.ToInt16(data["rowindex"][i]), newList);
                        }
                    }
                }

                var listInventoryItemWarranty = JsonConvert.DeserializeObject<List<InventoryItemWarrantyModel>>(warrantdata);

                for (int item = 0; item < listInventoryItemWarranty.Count; item++)
                {
                    listInventoryItemWarranty[item].FileData = dictFilePairs.GetValueOrDefault(item).ToList();
                }

                var result = await _inventoryItemWarrantyRepository.InsertWarrantyRequest(apicallurl,listInventoryItemWarranty);
                //result = true;
                 //var result = 1244;
                _logger.LogInfo($"SaveWarrantyRequest  Request in InventoryItemWarranty Controller return result successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call SaveWarrantyRequest in InventoryItemWarranty Controller Post Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"SaveWarrantyRequest  Request in InventoryItemWarranty Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }

        [HttpPost("fixedwarrantyrequest")]
        public async Task<IActionResult> FixedWarrantyRequest([FromBody] JsonElement body)
        {
            bool result = false;
            try
            {
                //string isFixed,string requestId
               // string json = System.Text.Json.JsonSerializer.Serialize(body);
                var listBody = JsonConvert.DeserializeObject<InventoryItemWarrantyFixedModel>(body.ToString());

                result = true;
                // result = await _inventoryItemWarrantyRepository.FixedWarrantyRequest(isFixed, Convert.ToInt32(requestId));
                _logger.LogInfo($"FixedWarrantyRequest  Request in InventoryItemWarranty Controller return result successfully...");
 
                return Ok(await Task.Run(() => result));
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call FixedWarrantyRequest in InventoryItemWarranty Controller Post Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"FixedWarrantyRequest  Request in InventoryItemWarranty Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }

        [HttpPost("uploadwarrantyattachment/{requestid}")]
        public async Task<IActionResult> UploadWarrantyAttachment(int requestid)
        {
            bool result = false;
            try
            {
                var files = Request.Form.Files;

                result = await _inventoryItemWarrantyRepository.UploadWarrantyAttachment(requestid,files);
                //result = true;
                _logger.LogInfo($"UploadWarrantyAttachment  Request in InventoryItemWarranty Controller return result successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call UploadWarrantyAttachment in InventoryItemWarranty Controller Post Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"UploadWarrantyAttachment  Request in InventoryItemWarranty Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpPost("savewarrantyrequesttest")]
        public async Task<IActionResult> savewarrantyrequesttest(IFormCollection data)
        //public async Task<IActionResult> savewarrantyrequesttest([FromForm] List<InventoryItemWarrantyModelTest> viewModel)
        {
            bool result = false;
            try
            {
                //var datalist = JsonConvert.DeserializeObject<InventoryItemWarrantyModel>(viewModel.FirstOrDefault().BucketName);

                 var name = data["warrantydatasource"];
                 //var files = data["file"];
                //files = Request.Form["file"];
                //IFormFileCollection file = (IFormFileCollection)data["file"].ToList();
                var files = Request.Form.Files;
                // IFormFileCollection files
                //List<IFormFile> listfile = new List<IFormFile>();

                Dictionary<int, List<IFormFile>> dictFilePairs = new Dictionary<int, List<IFormFile>>();

                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].Length > 0)
                    {
                        if (dictFilePairs.ContainsKey(Convert.ToInt16(data["rowindex"][i])))
                            dictFilePairs[Convert.ToInt16(data["rowindex"][i])].Add(files[i]);
                        else
                        {
                            var newList = new List<IFormFile>();
                            newList.Add(files[i]);
                            dictFilePairs.Add(Convert.ToInt16(data["rowindex"][i]), newList);
                        }
                    }
                }

               var listwarrantydata = JsonConvert.DeserializeObject<List<InventoryItemWarrantyModel>>(name);

                for (int item = 0; item < listwarrantydata.Count; item++)
                {  
                    listwarrantydata[item].FileData = dictFilePairs.GetValueOrDefault(item).ToList();
                }
                //var jsonarray = name;

                //dynamic response = JsonConvert.DeserializeObject(name[0]);

                // var keyValues = new Dictionary<string, string>
                //{
                //    { "emailSend", "abc@email.com" },
                //    { "toEmail", "xyz@email.com" }
                //};

                //JavaScriptSerializer js = new JavaScriptSerializer();
                //string json = JsonConvert.SerializeObject(keyValues);

                //var dataList = name.OfType<InventoryItemWarrantyModel>().ToList();

                //var jarray = Newtonsoft.Json.Linq.JArray.FromObject(name[0]);

                //string jsonstring = JsonConvert.SerializeObject(name[0]);
                // var datalist = JsonConvert.DeserializeObject<InventoryItemWarrantyModel>(name[0]);

                //List<InventoryItemWarrantyModel> list = new List<InventoryItemWarrantyModel>();

                //var newdata = string.Join(", ", videogames.ToArray());

                //var warrantydata = JsonConvert.DeserializeObject<List<InventoryItemWarrantyModel>>(jsonarray);
                // var files = data["file"];


                return Ok(true);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }


    }
}
