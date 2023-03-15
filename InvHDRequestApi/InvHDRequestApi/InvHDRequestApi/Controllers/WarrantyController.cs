using InvHDRequestApi.Models;
using InvHDRequestApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InvHDRequestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarrantyController : ControllerBase
    {
        private readonly IWarrantyRepository _warrantyRepository;
        private readonly ILoggerManagerRepository _logger;


        public WarrantyController(IWarrantyRepository warrantyRepository, ILoggerManagerRepository logger)
        {
            _logger = logger;
            _warrantyRepository = warrantyRepository;

            _logger.LogInfo("Initialize the Warranty Controller");

        }

        [HttpPost("submitwarranty/{apicallurl}")]
        public async Task<IActionResult> SubmitWarrantyRequest(string apicallurl, IFormCollection data)
        {
            // bool result = false;
            try
            {
                if (data == null || data.Count <= 0)
                {
                    return Problem("warranty submission model is invalid", statusCode: (int)HttpStatusCode.BadRequest);
                }

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

                var listInventoryItemWarranty = JsonConvert.DeserializeObject<List<GenericModel>>(warrantdata);

                for (int item = 0; item < listInventoryItemWarranty.Count; item++)
                {
                    listInventoryItemWarranty[item].FileData = dictFilePairs.GetValueOrDefault(item).ToList();
                }

                var result = await _warrantyRepository.InsertWarrantyRequest(apicallurl, listInventoryItemWarranty);
                //result = true;
                //var result = 1244;
                _logger.LogInfo($"SubmitWarrantyRequest  in Warranty Controller return result successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call SubmitWarrantyRequest in Warranty Controller Post Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"SubmitWarrantyRequest  Request in Warranty Controller=>{errMsg}");
                // var strErrMsg = $"Exception while send order {ex.Message} \n {ex.InnerException} \n {ex.StackTrace}";
                //return BadRequest(errMsg);
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }

    }
}
