using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SSInventory.Web.Services.Aws;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SSInventory.Web.Controllers.Apis
{
    /// <summary>
    /// File management controller
    /// </summary>
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class FileController : CommonController
    {
        private readonly IConfiguration _config;
        private readonly ILogger<FileController> _logger;
        private readonly FileService _fileService;

        /// <summary>
        /// File management controller constructor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="awebs3Service"></param>
        public FileController(IConfiguration config,
            ILogger<FileController> logger,
            FileService awebs3Service)
        {
            _config = config;
            _logger = logger;
            _fileService = awebs3Service;
        }

        /// <summary>
        /// Download photos
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="clientId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Error when getting the data</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DownloadPhoto(string fileName, int clientId)
        {

            if (clientId == 0 || string.IsNullOrWhiteSpace(fileName))
                return BadRequest("Parameters are invalid");

            var result = await _fileService.DownloadFileAsync(clientId.ToString(), fileName);
            return Ok(result);

        }

        /// <summary>
        /// Delete photos
        /// </summary>
        /// <param name="photos"></param>
        /// <param name="clientId"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Error when getting the data</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePhotos(List<string> photos, int clientId)
        {
            try
            {
                var saveFileRootPath = _config["ssSettings:SaveFileRootPath"];
                if (photos?.Any() == true)
                {
                    var usedAws = _config.GetValue<bool>("ssSettings:UseAWS");
                    foreach (var photo in photos)
                    {
                        if (usedAws)
                        {
                            await _fileService.DeleteFileAsync(photo, clientId);
                        }
                        else
                        {
                            foreach (var file in Directory.GetFiles(saveFileRootPath, photo, SearchOption.AllDirectories))
                            {
                                System.IO.File.Delete(file);
                            }
                        }
                    }

                    return Ok("Deleted file succesfully");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }

            return BadRequest("Invalid photo name");
        }
    }
}
