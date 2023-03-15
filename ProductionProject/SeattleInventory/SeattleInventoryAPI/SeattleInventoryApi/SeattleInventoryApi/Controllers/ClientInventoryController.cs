using Microsoft.AspNetCore.Mvc;
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
    public class ClientInventoryController : ControllerBase
    {
        private readonly IClientInventoryRepository _clientInventoryRepository;

        private readonly ILoggerManagerRepository _logger;

        public ClientInventoryController(IClientInventoryRepository clientInventoryRepository, ILoggerManagerRepository logger)
        {
            //logger = LogManager.GetCurrentClassLogger();
            _logger = logger;
            _clientInventoryRepository = clientInventoryRepository;

            _logger.LogInfo("Initialize the Inventory Controller");

        }

        [HttpGet("getclients")]
        public async Task<List<ClientInventoryModel>> GetClients()
        {


            try
            {
                var clients = await _clientInventoryRepository.getClients();
                _logger.LogInfo($"GetClients  Request in ClientInventoryController return result with clients successfully...");

                return clients;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetClients() Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetClients()  Request in ClientInventoryController=>{errMsg}");
                throw new Exception($"Exception occured to load clients \n {errMsg}");
            }
        }

        //// GET: api/<ClientInventoryController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<ClientInventoryController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<ClientInventoryController>
        [HttpPost("postclient")]
        public async Task<IActionResult> PostClient([FromBody] ClientInventoryModel model)
        {
            try
            {
                var result = await _clientInventoryRepository.insertClient(model);
                _logger.LogInfo($"InsertClient  Request in ClientInventoryController inserted successfully =>{result}");


                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call PostClient in ClientInventoryController Post Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _logger.LogError($"PostClient  Request in ClientInventoryController =>{errMsg}");
                // var strErrMsg = $"Exception while send order {ex.Message} \n {ex.InnerException} \n {ex.StackTrace}";
                //return BadRequest(errMsg);
                throw new Exception($"Exception occured due to post client \n {errMsg}");

            }
        }

        // PUT api/<ClientInventoryController>/5
        [HttpPut("updateclienthasinventory")]
        public async Task<IActionResult> UpdateClientHasInventory([FromBody] ClientInventoryModel model)
        {
            try
            {
                _logger.LogInfo($"GetClients  Request in ClientInventoryController return result with clients successfully...");
                var result = await _clientInventoryRepository.updateClientHasInventory(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetClients() Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetClients()  Request in ClientInventoryController=>{errMsg}");

                return BadRequest(errMsg);
            }
        }

        //// DELETE api/<ClientInventoryController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
