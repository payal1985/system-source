using AwsS3BucketDeleteApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AwsS3BucketDeleteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController : ControllerBase
    {
        private readonly IDeleteRepository _deleteRepository;
        private readonly ILogger<DeleteController> _logger;
        public DeleteController(IDeleteRepository deleteRepository, ILogger<DeleteController> logger)
        {
            _deleteRepository = deleteRepository;
            _logger = logger;
        }

        [HttpDelete("deletefile")]
        public async Task<IActionResult> DeleteFile(string bucket,string path)
        {
            //bool result = false;
            try
            {
                if (string.IsNullOrEmpty(bucket) || string.IsNullOrEmpty(path))
                {
                    return BadRequest("bucket/path is invalid");
                }

                bucket = WebUtility.UrlDecode(bucket);
                path = WebUtility.UrlDecode(path);

                await _deleteRepository.DeleteFileAsync(bucket, path);
            }
            catch(Exception ex)
            {
                var exErr = $"Exception occured due to call delete file request- {ex.InnerException} \n {ex.Message} \n {ex}";
                _logger.LogError(exErr);
                return BadRequest(exErr);
            }

            //return Ok();
            return Ok($"The file {path.Split('/').LastOrDefault()} is deleted successfully");


        }


    }
}
