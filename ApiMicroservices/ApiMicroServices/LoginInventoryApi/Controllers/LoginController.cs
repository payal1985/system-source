using LoginInventoryApi.Models;
using LoginInventoryApi.Repository;
using LoginInventoryApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
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

        //[HttpPost("getclients")]
        ////public async Task<IActionResult> GetClients(string userName, string password)
        //public async Task<IActionResult> GetClients([FromBody] LoginModel loginModel)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(loginModel.Uname) || string.IsNullOrEmpty(loginModel.Upwd))
        //        {
        //            return BadRequest("username / password are invalid");
        //        }

        //       string userName = WebUtility.UrlDecode(loginModel.Uname);
        //       string password = WebUtility.UrlDecode(loginModel.Upwd);

        //        var result = await _loginRepository.getClients(userName, password);
        //        //var result = true;
        //        _loggerManagerRepository.LogInfo($"GetClients  Request in Login Controller return result with clients successfully...");

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        string errMsg = $"Exception occured due to call GetClients Request=>{ex.Message} \n {ex.StackTrace}";
        //        _loggerManagerRepository.LogError($"GetClients  Request in Login Controller=>{errMsg}");
        //        //throw new Exception(errMsg);
        //        return BadRequest(errMsg);
        //        //return false;
        //    }
        //}

        [HttpGet("getclients/{userId}")]
        public async Task<List<ClientModel>> GetClients(int userId)
        {
            try
            {
                var result = await _loginRepository.getClients(userId);
                _loggerManagerRepository.LogInfo($"GetClients  Request in Login Controller return result with clients successfully...");

                return result;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetClients Request=>{ex.Message} \n {ex.StackTrace}";
                _loggerManagerRepository.LogError($"GetClients  Request in Login Controller=>{errMsg}");
                throw new Exception(errMsg);
            }
        }

        [HttpPost("getusers")]
        //public async Task<IActionResult> GetUsers(string userName, string password)
        public async Task<IActionResult> GetUsers([FromBody]LoginModel loginModel)
        {
            try
            {
                if (string.IsNullOrEmpty(loginModel.Uname) || string.IsNullOrEmpty(loginModel.Upwd))
                {
                    return Problem("username / password are invalid", statusCode: (int)HttpStatusCode.BadRequest);
                }
                
                string userName = WebUtility.UrlDecode(loginModel.Uname);
                string password = WebUtility.UrlDecode(loginModel.Upwd);

                var result = await _loginRepository.getUsers(userName, password);
                //var result = true;
                _loggerManagerRepository.LogInfo($"GetUsers  Request in Login Controller return result with user successfully...");

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetUsers Request=>{ex.Message} \n {ex.StackTrace}";
                _loggerManagerRepository.LogError($"GetUsers  Request in Login Controller=>{errMsg}");
                //throw new Exception(errMsg);
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
                //return false;
            }
        }

        [HttpGet("getuser/{userId}")]
        public async Task<UserModel> GetUsers(int userId)
        {
            try
            {
                var result = await _loginRepository.getUser(userId);
                //var result = true;
                _loggerManagerRepository.LogInfo($"GetUsers  Request in Login Controller return result with user successfully...");

                return result;
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetUsers Request=>{ex.Message} \n {ex.StackTrace}";
                _loggerManagerRepository.LogError($"GetUsers  Request in Login Controller=>{errMsg}");
                throw new Exception(errMsg);
            }
        }

        [HttpPost("updateAcceptRules")]
        public async Task<int> UpdateAcceptRules([FromBody] UpdateAcceptRulesModel model)
        {
            try
            {
                return await _loginRepository.UpdateAcceptRules(model.UserId, model.Rules);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call UpdateAcceptRules Request=>{ex.Message} \n {ex.StackTrace}";
                _loggerManagerRepository.LogError($"UpdateAcceptRules  Request in Login Controller=>{errMsg}");
                throw new Exception(errMsg);
            }
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return Problem("email is should not empty", statusCode: (int)HttpStatusCode.BadRequest);
                }
                else if(!new EmailAddressAttribute().IsValid(email))
                {
                    return Problem("email address is invalid", statusCode: (int)HttpStatusCode.BadRequest);
                }

                string passstr = await _loginRepository.ForgotPassword(email);

                return Ok(passstr);
            }
            catch(Exception ex)
            {
                _loggerManagerRepository.LogError($"Exception Occured due to retrive forgot passwordd-{ex.Message}-{ex.InnerException}-{ex.StackTrace}");
                return Problem($"Exception Occured due to retrive forgot password-{ex.Message}-{ex.InnerException}-{ex.StackTrace}", statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        [HttpPost("insertuser")]
        public async Task<IActionResult> InsertUser([FromBody]UserModel usermodel)
        {
            try
            {
                if (usermodel == null)
                {
                    return Problem("model is invalid",statusCode: (int)HttpStatusCode.BadRequest);
                }

                var result = await _loginRepository.InsertUser(usermodel);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _loggerManagerRepository.LogError($"Exception Occured due to insert user-{ex.Message}-{ex.InnerException}-{ex.StackTrace}");
                return Problem($"Exception Occured due to insert user-{ex.Message}-{ex.InnerException}-{ex.StackTrace}", statusCode: (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
