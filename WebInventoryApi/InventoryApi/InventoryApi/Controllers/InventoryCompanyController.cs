using InventoryApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryCompanyController : ControllerBase
    {
        private readonly ILoggerManagerRepository _logger;
        private readonly IInventoryCompanyRepository _inventoryCompanyRepository;

        public InventoryCompanyController(IInventoryCompanyRepository inventoryCompanyRepository, ILoggerManagerRepository logger)
        {
            _inventoryCompanyRepository = inventoryCompanyRepository;
            _logger = logger;
        }

        [HttpGet("getcompany")]
        public async Task<IActionResult> GetCompany()
        {
            try
            {
                var Companys = await _inventoryCompanyRepository.GetCompany();
                _logger.LogInfo($"GetCompany  Request in CompanyInventory Controller return result with inventory successfully...");
                
                return Ok(Companys);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetCompany Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetCompany  Request in CompanyInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }

        }

        [HttpGet("getusers/{companyid}")]
        public async Task<IActionResult> GetCompanyUsers(int companyid)
        {
            try
            {
                var users = await _inventoryCompanyRepository.GetCompanyUsers(companyid);
                _logger.LogInfo($"GetCompanyUsers  Request in CompanyInventory Controller return result successfully...");
                return Ok(users);
            }
            catch (Exception ex)
            {
                string errMsg = $"Exception occured due to call GetCompanyUsers Request=>{ex.Message} \n {ex.StackTrace}";
                _logger.LogError($"GetCompanyUsers  Request in CompanyInventory Controller=>{errMsg}");
                return Problem(errMsg, statusCode: (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
