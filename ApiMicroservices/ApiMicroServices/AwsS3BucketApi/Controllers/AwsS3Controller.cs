using AwsS3BucketApi.Configuration;
using AwsS3BucketApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AwsS3BucketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwsS3Controller : ControllerBase
    {       
        private IAws3Repository _aws3Repository;
        private IConfiguration _configuration;
        public AwsS3Controller(IAws3Repository aws3Repository)
        {
            //_configuration = configuration;
            _aws3Repository = aws3Repository;
        }

        [HttpGet("{documentName}")]
        public IActionResult GetDocumentFromS3(string documentName)
        {
            try
            {
                if (string.IsNullOrEmpty(documentName))
                    return Ok($"The 'documentName' parameter is required {(int)HttpStatusCode.BadRequest}");

                int clientId = 12; //clientId need to pass dynamically from controller or business logic

                 var document = _aws3Repository.DownloadFileAsync(documentName,clientId).Result;

                 return File(document, "application/octet-stream", documentName);
               // return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult UploadDocumentToS3(IFormFile file)
        {
            try
            {
                if (file is null || file.Length <= 0)
                    return Ok($"The 'documentName' parameter is required {(int)HttpStatusCode.BadRequest}");

                var result = _aws3Repository.UploadFileAsync(file);

                return Ok($"{string.Empty} -> {(int)HttpStatusCode.Created}");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} -> {(int)HttpStatusCode.InternalServerError}");
            }
        }

        [HttpDelete("{documentName}")]
        public IActionResult DeletetDocumentFromS3(string documentName)
        {
            try
            {
                if (string.IsNullOrEmpty(documentName))
                    return Ok($"The 'documentName' parameter is required {(int)HttpStatusCode.BadRequest}");
                var result = _aws3Repository.IsFileExists(documentName);
                if(result)
                    _aws3Repository.DeleteFileAsync(documentName);

                return Ok($"The document {documentName} is deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
