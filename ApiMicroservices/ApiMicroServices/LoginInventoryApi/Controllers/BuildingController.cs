using LoginInventoryApi.Models;
using LoginInventoryApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LoginInventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {

        private readonly IBuildingRepository _buildingRepository;

        private readonly ILoggerManagerRepository _loggerManagerRepository;


        public BuildingController(IBuildingRepository buildingRepository, ILoggerManagerRepository loggerManagerRepository)
        {
            _loggerManagerRepository = loggerManagerRepository;
            _buildingRepository = buildingRepository;

            _loggerManagerRepository.LogInfo("Initialize the Building Controller");
        }

        [HttpGet("getbuildings/{clientid}")]
        public async Task<IActionResult> GetBuildings(int clientid)
        {
            try
            {
                if (clientid <= 0)
                {
                    return Problem($"invalid client id is->{clientid}", statusCode: (int)HttpStatusCode.BadRequest);
                }

                var buildings = await _buildingRepository.GetBuildings(clientid);
                return Ok(buildings);

            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetBuildings Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _loggerManagerRepository.LogError($"GetBuildings  Request in Building Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpGet("getdestbuildings/{clientid}")]
        public async Task<IActionResult> GetDestBuildings(int clientid)
        {
            try
            {
                if (clientid <= 0)
                {
                    return Problem($"invalid client id is->{clientid}", statusCode: (int)HttpStatusCode.BadRequest);
                }

                var buildings = await _buildingRepository.GetDestinationBuilding(clientid);
                return Ok(buildings);

            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetDestBuildings Request=>{ex.Message} \n {ex.StackTrace} \n {ex.InnerException}";
                _loggerManagerRepository.LogError($"GetDestBuildings  Request in Building Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }
    }
}