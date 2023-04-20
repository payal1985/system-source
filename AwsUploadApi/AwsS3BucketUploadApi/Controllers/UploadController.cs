using AwsS3BucketUploadApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AwsS3BucketUploadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadRepository _uploadRepository;
        private readonly ILogger<UploadController> _logger;

        public UploadController(IUploadRepository uploadRepository, ILogger<UploadController> logger)
        {
            _uploadRepository = uploadRepository;
            _logger = logger;
        }

       // [HttpPost("uploadfiles/{url}/{path}")]
        [HttpPost("uploadfiles")]
        public async Task<IActionResult> UploadFiles(string bucket, string path)
        {
            bool result = false;
            try
            {
                var files = Request.Form.Files;

                if (string.IsNullOrEmpty(bucket) || string.IsNullOrEmpty(path))
                {
                    return BadRequest("bucket / path is invalid");
                }

                if (files == null)
                {
                    return BadRequest("file is invalid");
                }

                bucket = WebUtility.UrlDecode(bucket);
                path = WebUtility.UrlDecode(path);

                foreach (var file in files)
                {
                    result = await _uploadRepository.FilesUpload(bucket, path,file);                   
                }
                _logger.LogInformation($"File Upload successfully , result is->{result}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                var exErr = $"Exception occured due to call upload files request- {ex.InnerException} \n {ex.Message} \n {ex}";
                _logger.LogError(exErr);
                return BadRequest($"{ex.InnerException} - {ex.Message}");
            }
        }
    }
}
