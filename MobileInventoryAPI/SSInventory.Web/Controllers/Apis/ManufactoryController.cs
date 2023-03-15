using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SSInventory.Business.Interfaces;
using SSInventory.Share.Models.Dto.Inventory;
using SSInventory.Share.Models.Dto.ItemTypeOptionLines;
using SSInventory.Share.Models.Dto.Manufactory;
using SSInventory.Share.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Web.Controllers.Apis
{
    /// <summary>
    /// Manufactory controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ManufactoryController : CommonController
    {
        private readonly IManufactoryService _manufactoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IItemTypeOptionService _itemTypeOptionService;
        private readonly ILogger<ManufactoryController> _logger;

        /// <summary>
        /// Manufactory controller constructor
        /// </summary>
        /// <param name="manufactoryService"></param>
        /// <param name="itemTypeOptionService"></param>
        /// <param name="logger"></param>
        /// <param name="manufacturerService"></param>
        public ManufactoryController(IManufactoryService manufactoryService,
            IItemTypeOptionService itemTypeOptionService,
            ILogger<ManufactoryController> logger,
            IManufacturerService manufacturerService)
        {
            _manufactoryService = manufactoryService;
            _itemTypeOptionService = itemTypeOptionService;
            _logger = logger;
            _manufacturerService = manufacturerService;
        }

        /// <summary>
        /// Get manufacturers
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Error when getting the data</response>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetManufacturers()
        {
            try
            {
                return Ok(await _manufacturerService.ReadAsync(pmTypes: new List<string> { "P", "F", "X" }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return BadRequest();
        }

        /// <summary>
        /// Save manufacturer
        /// Currently just support create new action
        /// </summary>
        /// <param name="input"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data</response>
        /// <response code="409">Existing manufacturer</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>List data which had inserted into database to client side</returns>
        [HttpPost]
        public async Task<IActionResult> SaveManufacturer(CreateOrEditManufacturerModel input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.ManufacturerName))
                    return BadRequest("The manufacturer name is required");

                var response = await _manufacturerService.CreateAsync(input);
                if (!response.Success)
                {
                    return Conflict(response.Message);
                }

                return Ok(response.Data);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Save manufacturer
        /// Currently just support create new action
        /// </summary>
        /// <param name="input"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Error when getting the data</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>List data which had inserted into database to client side</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        protected async Task<IActionResult> SaveManufacturer_backup(CreateManufacturerModel input)
        {
            if (input.ItemTypeOptionId < 1 || input.ItemTypeOptionId > int.MaxValue)
            {
                return BadRequest("Item type option is invalid");
            }
            if (input.ClientId < 1 || string.IsNullOrWhiteSpace(input.OptionName))
                return BadRequest("Client or option name is required");

            try
            {
                // This function supports insert multiple records but in this case only one Item type option line should be saved to the database
                var itemTypeOptions = await _itemTypeOptionService.ReadAsync(ids: new List<int> { input.ItemTypeOptionId });
                if (itemTypeOptions?.Any() == false)
                {
                    return BadRequest("Item type does not match");
                }
                var itemTypeOptionIds = itemTypeOptions.Select(x => x.ItemTypeOptionId).Distinct();
                if (itemTypeOptionIds?.Any() == true)
                {
                    var manufactories = new List<CreateOrUpdateItemTypeOptionLineModel>();
                    foreach (var itemTypeOptionId in itemTypeOptionIds)
                    {
                        manufactories.Add(new CreateOrUpdateItemTypeOptionLineModel
                        {
                            ClientId = input.ClientId,
                            ItemTypeOptionId = itemTypeOptionId,
                            ItemTypeOptionLineCode = input.OptionCode.ConvertItemTypeName(input.OptionName),
                            ItemTypeOptionLineName = input.OptionName,
                            ItemTypeOptionLineDesc = !string.IsNullOrWhiteSpace(input.OptionDesc) ? input.OptionDesc : input.OptionName,
                            CreateId = 0,
                            CreateDateTime = System.DateTime.Now
                        });
                    }
                    var result = await _manufactoryService.InsertAsync(manufactories);
                    if (result.Count == itemTypeOptionIds.Count())
                    {
                        return input.ItemTypeOptionId > 0
                            ? Ok(result.Find(x => x.ItemTypeOptionID == input.ItemTypeOptionId))
                            : Ok(JsonConvert.SerializeObject(result));
                    }
                    return BadRequest("Save failured");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return Problem("Error while processing data" + exception.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

            return BadRequest("Invalid data");
        }
    }
}