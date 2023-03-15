using Microsoft.AspNetCore.Mvc;
using RequestMilestoneAPI.Models;
using RequestMilestoneAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RequestMilestoneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MileStoneDatesController : ControllerBase
    {

        private readonly IMilestoneDatesRepository _milestoneDatesRepository;
       
       // private readonly ILoggerManagerRepository _logger;


        public MileStoneDatesController(IMilestoneDatesRepository milestoneDatesRepository
            //, ILoggerManagerRepository logger
                                            
                                            )
        {

            // _logger = logger;
            _milestoneDatesRepository = milestoneDatesRepository;
           

         //   _logger.LogInfo("Initialize the Inventory Item Order Controller");

        }

        [HttpGet("getmilestonedates")]
        public async Task<IActionResult> GetMilestoneDates(int request_id)
        {
            try
            {
                var result = await _milestoneDatesRepository.GetMilestoneDates(request_id);
               // _logger.LogInfo($"GetInventoryorderItem  Request in InventoryItemOrder Controller return result with inventory orders successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetMilestoneDates Request=>{ex.Message} \n {ex.StackTrace}";
                //_logger.LogError($"GetInventoryorderItem  Request in InventoryItemOrder Controller=>{errMsg}");
             
                return BadRequest(errMsg);
            }
        }

        // POST api/<MileStoneDatesController>
        [HttpPost("postmilestonedates")]
        public IActionResult PostMilestoneDates([FromBody] List<MilestoneDatesModel> model)
        {
            try
            {
                var result = _milestoneDatesRepository.InsertMilestoneDates(model);
                //_logger.LogInfo($"PostInventoryItemOrder  Request in InventoryItemOrder Controller return result successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call PostMilestoneDates in MileStoneDatesController Post Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                //_logger.LogError($"PostInventoryItemOrder  Request in InventoryItemOrder Controller=>{errMsg}");
                // var strErrMsg = $"Exception while send order {ex.Message} \n {ex.InnerException} \n {ex.StackTrace}";
                //return BadRequest(errMsg);
                throw new Exception($"Exception occured to post milestone dates \n {errMsg}");

            }
        }

        //// GET: api/<MileStoneDatesController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<MileStoneDatesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}


        //// POST api/<MileStoneDatesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<MileStoneDatesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<MileStoneDatesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
