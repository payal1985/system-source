using LoginInventoryApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LoginInventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;

        private readonly ILoggerManagerRepository _loggerManagerRepository;


        public CountryController(ICountryRepository countryRepository, ILoggerManagerRepository loggerManagerRepository)
        {
            _loggerManagerRepository = loggerManagerRepository;
            _countryRepository = countryRepository;

            _loggerManagerRepository.LogInfo("Initialize the Country Controller");
        }

        [HttpPost("getcountries")]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var result = await _countryRepository.GetCountries();
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call getcountries Request=>{ex.Message} \n {ex.StackTrace}";
                _loggerManagerRepository.LogError($"getcountries  Request in Country Controller=>{errMsg}");
                return BadRequest(errMsg);
            }
        }
    }
}
