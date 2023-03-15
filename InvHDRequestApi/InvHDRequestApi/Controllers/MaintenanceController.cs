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
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceRepository _maintenanceRepository;
        private readonly ILoggerManagerRepository _logger;


        public MaintenanceController(IMaintenanceRepository maintenanceRepository, ILoggerManagerRepository logger)
        {
            _logger = logger;
            _maintenanceRepository = maintenanceRepository;

            _logger.LogInfo("Initialize the Maintenance Controller");

        }

        [HttpPost("submitmaintenance/{apicallurl}")]
        public async Task<IActionResult> SubmitMaintenanceRequest(string apicallurl, IFormCollection data)
        {
            // bool result = false;
            try
            {
                if (data == null || data.Count <= 0)
                {
                    return Problem("maintenance submission model is invalid", statusCode: (int)HttpStatusCode.BadRequest);
                }

                apicallurl = WebUtility.UrlDecode(apicallurl);

                var warrantdata = data["maintenancedatasource"];
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

                var listInventoryItemMaintenance = JsonConvert.DeserializeObject<List<GenericModel>>(warrantdata);

                for (int item = 0; item < listInventoryItemMaintenance.Count; item++)
                {
                    listInventoryItemMaintenance[item].FileData = dictFilePairs.GetValueOrDefault(item).ToList();
                }

                var result = await _maintenanceRepository.InsertMaintenanceRequest(apicallurl, listInventoryItemMaintenance);
                //result = true;
                //var result = 1244;
                _logger.LogInfo($"SubmitMaintenanceRequest  in Maintenance Controller return result successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call SubmitMaintenanceRequest in Maintenance Controller Post Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"SubmitMaintenanceRequest  Request in Maintenance Controller=>{errMsg}");
                // var strErrMsg = $"Exception while send order {ex.Message} \n {ex.InnerException} \n {ex.StackTrace}";
                //return BadRequest(errMsg);
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);

            }
        }


    }
}
