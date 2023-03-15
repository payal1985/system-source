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
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;
        //private readonly IEmailNotificationRepository _emailNotificationRepository;
        //private readonly IHDTicketRepository _hDTicketRepository;
        private readonly ILoggerManagerRepository _logger;


        public LoginController(ILoginRepository loginRepository, ILoggerManagerRepository logger)
        {
            _logger = logger;
            _loginRepository = loginRepository;            

            _logger.LogInfo("Initialize the Login Controller");
        }

        // GET: api/<LoginController>
        [HttpGet("getusers")]
        public async Task<LoginModel> GetUsers(string user,string pwd)
        {
            try
            {
                var result = await _loginRepository.getUsers(user,pwd);
                //var result = true;
                _logger.LogInfo($"GetClients  Request in Login Controller return result with client successfully...");

                return result;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetClients Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetClients  Request in Login Controller=>{errMsg}");
                throw new Exception(errMsg);
                //return BadRequest(errMsg);
                //return false;
            }
        }

        //[HttpGet("gettests")]
        //public async Task<LoginModel> gettests(string user)
        //{
        //    var result = await _loginRepository.getUsers(user, "test");
        //    return result;
        //}

        //// GET api/<LoginController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<LoginController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<LoginController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<LoginController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
