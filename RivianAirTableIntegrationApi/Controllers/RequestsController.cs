using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RivianAirtableIntegrationApi.Models;
using RivianAirtableIntegrationApi.Repository;
using RivianAirtableIntegrationApi.Repository.Interfaces;
using System.Net;

namespace RivianAirtableIntegrationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestsRepository _requestsRepository;
        private readonly IEmailNotificationRepository _emailNotificationRepository;
        private readonly ILogger<RequestsController> _logger;


        public RequestsController(IRequestsRepository requestsRepository
                                  ,ILogger<RequestsController> logger
                                ,IEmailNotificationRepository emailNotificationRepository
                                  )
        {
            _logger = logger;
            _requestsRepository = requestsRepository;
            _emailNotificationRepository= emailNotificationRepository;
            _logger.LogInformation("Initialize the Requests Controller");
        }

        [HttpGet("recordstest")]
        public async Task<IActionResult> GetRecordsTestAsync()
        {
            try
            {
               // bool result = false;
                var records = await _requestsRepository.GetRequestRecords();
                var envConnectionString = _requestsRepository.GetConnectionString();

                if (records == null)
                {
                    _logger.LogInformation($"Requested Records response null ");
                    return Problem("requested records are not found-", statusCode: (int)HttpStatusCode.NotFound);
                }
                else if (records.Count > 0)
                {
                    var jsonrecords = JsonConvert.SerializeObject(records);
                    _logger.LogInformation($"Requested Records response-\n{jsonrecords}");
                    //result = await _requestsRepository.InsertUpdateRequestRecords(records);
                    // result = true;
                }

                _logger.LogInformation($"Connection String-{envConnectionString}");
                return Ok(records);
                //return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception Occured due to GET Request of Records-\n{ex.Message}\n{ex.InnerException}\n{ex.StackTrace}";
                _logger.LogError(errMsg);
//                _emailNotificationRepository.SendErrorEmail("Rivian AirTable Integration Error", errMsg);

                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }

        }

        [HttpGet("records")]  
        public async Task<IActionResult> GetRecordsAsync()
        {
            try
            {
                bool result = false;
                var records = await _requestsRepository.GetRequestRecords();
                if (records == null)
                {
                    _logger.LogInformation($"Requested Records response null ");
                    return Problem("requested records are not found-", statusCode: (int)HttpStatusCode.NotFound);
                }
                else if (records.Count > 0)
                {
                    var jsonrecords = JsonConvert.SerializeObject(records);
                    _logger.LogInformation($"Requested Records response-\n{jsonrecords}");
                    result = await _requestsRepository.InsertUpdateRequestRecords(records);
                   // result = true;
                }
                //return Ok(records);
                return Ok(result);
            }
            catch(Exception ex)
            {
                string errMsg = $"Exception Occured due to GET Request of Records-\n{ex.Message}\n{ex.InnerException}\n{ex.StackTrace}";
                _logger.LogError(errMsg);
                _emailNotificationRepository.SendErrorEmail("Rivian AirTable Integration Error", errMsg);

                return Problem(errMsg,statusCode:(int)HttpStatusCode.BadRequest);
            }
  
        }
    }
}
