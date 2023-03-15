using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SSInventory.Business.Interfaces;
using SSInventory.Share.Models.Dto.Users.ExternalSystem;
using SSInventory.Web.Constants;
using SSInventory.Web.Models;
using SSInventory.Web.Models.Authentications;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using SSInventory.Share.Models.Dto.Users.Authentications;
using System.Net.Http.Json;
using SSInventory.Web.Models.Users;

namespace SSInventory.Web.Controllers.Apis
{
    /// <summary>
    /// User controller
    /// </summary>
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : CommonController
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        /// <summary>
        /// User controller constructor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="userService"></param>
        public UserController(IConfiguration config,
            ILogger<UserController> logger,
            IUserService userService)
        {
            _config = config;
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="login"></param>
        /// <response code="200">Success</response>
        /// <response code="500">Error when getting the data</response>
        /// <response code="401">Unauthorized</response>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (string.IsNullOrWhiteSpace(login.LoginEmail) || string.IsNullOrWhiteSpace(login.LoginPW))
            {
                return Problem("Username or password invalid");
            }

            try
            {
                return await SSIAppLogin(login);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error occurred while getting the information" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Refresh token
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Error when getting the data</response>
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = HttpContext.Request.Headers["refreshToken"];
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var userId = HttpContext.Request.Headers["userId"];
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Unauthorized(new { message = "User Id is required" });
                }

                var user = await GetExternalUser(int.Parse(userId));
                if (user is null)
                {
                    return Unauthorized(new { message = "User not found" });
                }

                var response = await _userService.RefreshToken(user.UserId, refreshToken, _config["Jwt:Key"], ConfigurationHelps.ExpiredTimeTokenDefault, IpAddress());

                if (response == null)
                    return Unauthorized(new { message = "Invalid token" });

                SetTokenToCookie(response.RefreshToken, ConfigurationHelps.ExpiredTimeTokenDefault);

                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Revoke user token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("revoke-token")]
        protected async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequestModel model)
        {
            // accept token from request body or header
            try
            {
                var token = model.Token ?? HttpContext.Request.Headers["refreshToken"];

                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token is required");

                var response = await _userService.RevokeToken(token, IpAddress());

                return !response ? NotFound("Token not found") : Ok("Token revoked");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Token is required</response>
        /// <response code="404">Token is not found</response>
        [HttpPost]
        public async Task<IActionResult> Logout([FromBody] LogoutModel model)
        {
            try
            {
                // accept token from request body or header
                var token = model.Token ?? HttpContext.Request.Headers["refreshToken"];

                if (string.IsNullOrEmpty(token))
                    return BadRequest("Token is required");

                var response = await _userService.RevokeToken(token, IpAddress());

                return !response ? NotFound("Token is not found") : Ok("Sign out successful");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Update user accept rules
        /// </summary>
        /// <param name="input"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="404">Data not found</response>
        /// <response code="500">Error when getting the data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserAcceptRules([FromBody] AcceptRuleInputModel input)
        {
            if (string.IsNullOrWhiteSpace(input.Rules))
                return BadRequest("Invalid data");

            try
            {
                using var client = new HttpClient();
                HttpClientConfigure(client);
                var postData = new
                {
                    userId = input.UserId,
                    rules = input.Rules
                };
                JsonContent content = JsonContent.Create(postData);
                HttpResponseMessage response = await client.PostAsync("api/Login/updateAcceptRules", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseResult = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<int>(responseResult);
                    if (data > 0)
                        return Ok("Updated accept rules successfully");
                    if (data == 0)
                        return NotFound("Not found data");
                    if (data == -1)
                        return NotFound("Invalid user");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return Problem("Error while processing data" + ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

            return BadRequest();
        }


        #region Private Methods

        /// <summary>
        /// Generate user token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="minutesExpired"></param>
        /// <returns>json string</returns>

        private void SetTokenToCookie(string token, int minutesExpired)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(minutesExpired)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private async Task<UserExternalInfoModel> GetExternalUser(int userId)
        {
            using var client = new HttpClient();
            HttpClientConfigure(client);

            HttpResponseMessage response = await client.GetAsync($"api/Login/getuser/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var responseResult = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UserExternalInfoModel>(responseResult);
                if (data != null)
                    return data;
            }
            return null;
        }

        private void HttpClientConfigure(HttpClient client)
        {
            client.BaseAddress = new Uri(_config["ExternalSystem:Domain"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task<IActionResult> SSIAppLogin(LoginModel login)
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (request, certificate, chain, errors) =>
                {

                    return true;
                }
            };

            try
            {
                using var client = new HttpClient(handler);
                HttpClientConfigure(client);
                var postData = new
                {
                    Uname = login.LoginEmail,
                    Upwd = login.LoginPW
                };
                JsonContent content = JsonContent.Create(postData);
                HttpResponseMessage response = await client.PostAsync("api/Login/getusers", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseResult = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<UserLoginResponseModel>(responseResult);
                    if (data is null || string.IsNullOrWhiteSpace(data?.PermissionLevel))
                        return Unauthorized("Anauthenticated");

                    var result = await _userService.AuthenticateFromExternal(new UserAuthenticationExternalModel
                    {
                        User = data,
                        Secret = _config["Jwt:Key"],
                        MinutesExpired = ConfigurationHelps.ExpiredTimeTokenDefault
                    }, IpAddress());

                    result.Inventory_user_accept_rules_reqd = data.Inventory_user_accept_rules_reqd;
                    result.FristName = data.FirstName;
                    result.PermissionLevel = data.PermissionLevel;

                    return Ok(result);
                }

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return BadRequest();
            }

            return BadRequest();
        }

        private async Task<IActionResult> MainAppLogin(LoginModel login)
        {
            var loginUser = await _userService.Authenticate(new UserAuthenticationModel
            {
                LoginEmail = login.LoginEmail,
                LoginPW = login.LoginPW,
                Secret = _config["Jwt:Key"],
                MinutesExpired = ConfigurationHelps.ExpiredTimeTokenDefault
            }, IpAddress());
            if (loginUser != null)
            {
                _logger.LogInformation($"Logging in {JsonConvert.SerializeObject(loginUser)}");
                return Ok(loginUser);
            }

            return Unauthorized("Anauthenticated");
        }


        #endregion
    }
}
