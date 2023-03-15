using LoginInventoryApi.Models;
using LoginInventoryApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly ILoginRepository _loginRepository;
   
        private readonly ILoggerManagerRepository _loggerManagerRepository;


        public LoginController(ILoginRepository loginRepository, ILoggerManagerRepository loggerManagerRepository)
        {
            _loggerManagerRepository = loggerManagerRepository;
            _loginRepository = loginRepository;

            _loggerManagerRepository.LogInfo("Initialize the Login Controller");
        }

        [HttpGet("getclients")]
        public async Task<List<ClientModel>> GetClients(string userName, string password)
        {
            try
            {
                var result = await _loginRepository.getClients(userName, password);
                //var result = true;
                _loggerManagerRepository.LogInfo($"GetClients  Request in Login Controller return result with clients successfully...");

                return result;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetClients Request=>{ex.Message} \n {ex.StackTrace}";
                _loggerManagerRepository.LogError($"GetClients  Request in Login Controller=>{errMsg}");
                throw new Exception(errMsg);
                //return BadRequest(errMsg);
                //return false;
            }
        }

        [HttpGet("getusers")]
        public async Task<UserModel> GetUsers(string userName, string password)
        {
            try
            {
                var result = await _loginRepository.getUsers(userName, password);
                //var result = true;
                _loggerManagerRepository.LogInfo($"GetUsers  Request in Login Controller return result with user successfully...");

                return result;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetUsers Request=>{ex.Message} \n {ex.StackTrace}";
                _loggerManagerRepository.LogError($"GetUsers  Request in Login Controller=>{errMsg}");
                throw new Exception(errMsg);
                //return BadRequest(errMsg);
                //return false;
            }
        }
    }
}
