using Amazon.Runtime;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3;
using AwsS3Download.Models;
using AwsS3Download.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text.Json;

namespace AwsS3Download.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwsDownloadController : ControllerBase
    {
        private IDownloadRepository _downloadRepository;
        private readonly ILogger<AwsDownloadController> _logger;

        public AwsDownloadController(IDownloadRepository downloadRepository, ILogger<AwsDownloadController> logger)
        {
            _downloadRepository = downloadRepository;
            _logger = logger;
        }

        [HttpGet("getfilessample")]
        public async Task<IActionResult> GetFilesSample()
        {
            var files = new List<string>();
            _logger.LogInformation($"File download Start Process ");

            files = await _downloadRepository.ListS3FilesSample("inventory/12", 15);
            _logger.LogInformation($"File download End Process , total count {files.Count}");
            return Ok(files);
        }

        //single file generate signed url...
        [HttpGet("getfile")]
        public async Task<IActionResult> GetFile(string bucket, string prefix, int signedUrlValidMinutes)
        {
            try
            {
                _logger.LogInformation($"File download Start Process for request GetFile() belong to AwsDownloadController");
                string signedurl = await _downloadRepository.GetS3File(bucket, prefix, signedUrlValidMinutes);
                return Ok(signedurl);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured while run the request GetFile() belong to AwsDownloadController\n {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
                return BadRequest($"Exception Occured due to GetFiles {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        [HttpGet("getfilebytes")]
        public async Task<IActionResult> GetFileBytes(string bucket, string prefix)
        {
            //string base64Str = "";
            try
            {
                _logger.LogInformation($"File download Start Process for request GetFileBytes() belong to AwsDownloadController");
                if (string.IsNullOrEmpty(bucket) || string.IsNullOrEmpty(prefix))
                {
                    _logger.LogError($"File download End Process for request GetFileBytes() belong to AwsDownloadController, get error - bucket/path is invalid");
                    return BadRequest("bucket/path is invalid");
                }

                bucket = WebUtility.UrlDecode(bucket);
                prefix = WebUtility.UrlDecode(prefix);

                var files = await _downloadRepository.GetFileBytes(bucket, prefix);
                //string base64Str = Convert.ToBase64String(files);

                return Ok(files);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured while run the request GetFileBytes() belong to AwsDownloadController\n {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
                return BadRequest($"Exception Occured due to GetFileBytes {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        [HttpGet("downloadfile")]
        public async Task<IActionResult> DownloadFile(string bucket, string prefix,string savepath)
        {
            //string base64Str = "";
            try
            {
                _logger.LogInformation($"File download Start Process for request DownloadFile() belong to AwsDownloadController");
                if (string.IsNullOrEmpty(bucket) || string.IsNullOrEmpty(prefix))
                {
                    _logger.LogError($"File download End Process for request DownloadFile() belong to AwsDownloadController, get error - bucket/path is invalid");
                    return BadRequest("bucket/path is invalid");
                }

                bucket = WebUtility.UrlDecode(bucket);
                prefix = WebUtility.UrlDecode(prefix);

                var files = await _downloadRepository.DownloadFile(bucket, prefix, savepath);
                //string base64Str = Convert.ToBase64String(files);

               // string sz = string.Format("{0:n1} KB", (files / 1024f));


                return Ok(files);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured while run the request DownloadFile() belong to AwsDownloadController\n {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
                return BadRequest($"Exception Occured due to DownloadFile {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        [HttpGet("downloadbase64")]
        public async Task<IActionResult> DownloadBase64(string bucket, string prefix)
        {
            try
            {
                _logger.LogInformation($"Base64 download Start Process for request downloadbase64() belong to AwsDownloadController");
                if (string.IsNullOrEmpty(bucket) || string.IsNullOrEmpty(prefix))
                {
                    _logger.LogError($"Base64 download End Process for request downloadbase64() belong to AwsDownloadController, get error - bucket/path is invalid");
                    return BadRequest("bucket/path is invalid");
                }

                bucket = WebUtility.UrlDecode(bucket);
                prefix = WebUtility.UrlDecode(prefix);

                var files = await _downloadRepository.DownloadBase64(bucket, prefix);
                //string base64Str = Convert.ToBase64String(files);

                // string sz = string.Format("{0:n1} KB", (files / 1024f));


                return Ok(files);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured while run the request downloadbase64() belong to AwsDownloadController\n {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
                return BadRequest($"Exception Occured due to downloadbase64 {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
            }
        }

        [HttpPost("listS3filenames")]
        public async Task<IActionResult> ListS3FileNames([FromBody] JsonElement body)
        {
            //List<string> files = new List<string>();
            try
            {                
                _logger.LogInformation($"File download Start Process for request ListS3FileNames() belong to AwsDownloadController");

                var awsModel = JsonConvert.DeserializeObject<AwsModel>(body.ToString());

                var files = await _downloadRepository.ListS3FileNames(awsModel);
                _logger.LogInformation($"File download End Process for request ListS3FileNames() belong to AwsDownloadController, total count {files.Count}");
                return Ok(files);

            }
            catch (Exception ex)
            {
                var exErr = $"Exception occured due to call check files exists status request- {ex.InnerException} \n {ex.Message} \n {ex}";
                _logger.LogError(exErr);
                return Problem(exErr, statusCode: (int)HttpStatusCode.BadRequest);
                //return BadRequest($"{ex.InnerException} - {ex.Message}",HttpStatusCode:);
            }


        }

        //list of files generate signed url...        
        //[HttpGet("getfiles")]
        ////[RequestSizeLimit(40000000)]
        //public async Task<IActionResult> GetFiles([FromQuery] AwsModel awsModel)
        [HttpPost("getfiles")]
        //[RequestSizeLimit(40000000)]
        public async Task<IActionResult> GetFilesPost([FromBody] JsonElement body)
        {
            try
            {
                var files = new List<string>();
                _logger.LogInformation($"File download Start Process for request GetFiles() belong to AwsDownloadController");

                var awsModel = JsonConvert.DeserializeObject<AwsModel>(body.ToString());

                files = await _downloadRepository.ListS3Files(awsModel);
                _logger.LogInformation($"File download End Process for request GetFiles() belong to AwsDownloadController, total count {files.Count}");
                return Ok(files);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured while run the request GetFiles() belong to AwsDownloadController\n {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
                return BadRequest($"Exception Occured due to GetFiles {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
            }
        }


        //[HttpPost("getfilespost")]
        ////[RequestSizeLimit(40000000)]
        //public async Task<IActionResult> GetFilesPost([FromBody] JsonElement body)
        //{
        //    try
        //    {
        //        var files = new List<string>();
        //        _logger.LogInformation($"File download Start Process for request GetFiles() belong to AwsDownloadController");

        //        var awsModel = JsonConvert.DeserializeObject<AwsModel>(body.ToString());

        //        files = await _downloadRepository.ListS3Files(awsModel);
        //        _logger.LogInformation($"File download End Process for request GetFiles() belong to AwsDownloadController, total count {files.Count}");
        //        return Ok(files);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Exception occured while run the request GetFiles() belong to AwsDownloadController\n {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
        //        return BadRequest($"Exception Occured due to GetFiles {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
        //    }
        //}

        [HttpGet("downloadfilestofolder")]
        public async Task<IActionResult> DownloadFileToLocalFolder(string bucket, string prefix)
        {
            try
            {
                _logger.LogInformation($"File download Start Process for request DownloadFileToLocalFolder() belong to AwsDownloadController");
                if (string.IsNullOrEmpty(bucket) || string.IsNullOrEmpty(prefix))
                {
                    _logger.LogError($"File download End Process for request DownloadFileToLocalFolder() belong to AwsDownloadController, get error - bucket/path is invalid");
                    return BadRequest("bucket/path is invalid");
                }

                bucket = WebUtility.UrlDecode(bucket);
                prefix = WebUtility.UrlDecode(prefix);

                await _downloadRepository.DownloadDirectoryAsync(bucket, prefix);
                //string base64Str = Convert.ToBase64String(files);

                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured while run the request DownloadFileToLocalFolder() belong to AwsDownloadController\n {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
                return BadRequest($"Exception Occured due to DownloadFileToLocalFolder {ex.InnerException} \n {ex.Message} \n {ex.StackTrace}");
            }
        }

    }
}
