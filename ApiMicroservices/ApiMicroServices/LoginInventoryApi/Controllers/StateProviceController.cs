using LoginInventoryApi.Models;
using LoginInventoryApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LoginInventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateProvinceController : ControllerBase
    {
        private readonly IStateProvinceRepository _stateProvinceRepository;

        private readonly ILoggerManagerRepository _loggerManagerRepository;


        public StateProvinceController(IStateProvinceRepository stateProvinceRepository, ILoggerManagerRepository loggerManagerRepository)
        {
            _loggerManagerRepository = loggerManagerRepository;
            _stateProvinceRepository = stateProvinceRepository;

            _loggerManagerRepository.LogInfo("Initialize the StateProvice Controller");
        }

        [HttpPost("getstateprovinces")]
        public async Task<IActionResult> GetStateProvinces([FromBody] GetStateProvincesRequestModel requestModel)
        {
            try
            {
                if (requestModel.CountryId < 0)
                {
                    return BadRequest("Country Id is invalid");
                }

                var result = await _stateProvinceRepository.GetStateProvinces(requestModel.CountryId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call getstateprovices Request=>{ex.Message} \n {ex.StackTrace}";
                _loggerManagerRepository.LogError($"getstateprovices  Request in StateProvices Controller=>{errMsg}");
                return BadRequest(errMsg);
            }
        }
    }
}