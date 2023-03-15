using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SSInventory.Share.Models.Aws.Configurations;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace SSInventory.Web.Services.Aws
{
    #region aws s3 old class
    /*
    /// <summary>
    /// AWS service
    /// </summary>
    public class Aws3Service
    {
        private readonly IConfiguration _config;
        private readonly BasicAWSCredentials BasicAWSCredentials;
        private readonly AmazonS3Client AmazonS3Client;
        private readonly AwsClientConfiguration AwsClientConfiguration;
        private readonly ILogger<Aws3Service> _logger;

        /// <summary>
        /// AWS service constructor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        public Aws3Service(IConfiguration config, ILogger<Aws3Service> logger)
        {
            _config = config;
            _logger = logger;

            AwsClientConfiguration = GetConfigSection(_config);
            BasicAWSCredentials = new BasicAWSCredentials(AwsClientConfiguration.AwsAccessKey, AwsClientConfiguration.AwsSecretAccessKey);
            AmazonS3Client = new AmazonS3Client(BasicAWSCredentials, RegionEndpoint.USWest2);
        }

        /// <summary>
        /// Get AWS endpoint
        /// </summary>
        /// <returns></returns>
        public string GetAwsEndpoint()
        {
            return $"https://{AwsClientConfiguration.BucketName}.{AmazonS3Client.Config.AuthenticationServiceName}.{AwsClientConfiguration.Region}.amazonaws.com/{AwsClientConfiguration.Folder}";
        }

        /// <summary>
        /// Delete file from AWS
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="clientId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFileAsync(string fileName, int clientId, string versionId = "")
        {
            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest
                {
                    BucketName = AwsClientConfiguration.BucketName,
                    Key = AwsClientConfiguration.Folder + "/" + clientId + "/" + fileName
                };

                if (!string.IsNullOrEmpty(versionId))
                    request.VersionId = versionId;

                await AmazonS3Client.DeleteObjectAsync(request);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Download file from AWS
        /// </summary>
        /// <param name="file"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<byte[]> DownloadFileAsync(string file, int clientId)
        {
            MemoryStream ms = null;

            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = AwsClientConfiguration.BucketName,
                    Key = AwsClientConfiguration.Folder + "/" + clientId + "/" + file
                };

                using (var response = await AmazonS3Client.GetObjectAsync(getObjectRequest))
                {
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        using (ms = new MemoryStream())
                        {
                            await response.ResponseStream.CopyToAsync(ms);
                        }
                    }
                }

                if (ms is null || ms.ToArray().Length < 1)
                    throw new FileNotFoundException(string.Format("The document '{0}' is not found", file));

                return ms.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Check file is existed in AWS
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="clientId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public bool IsFileExists(string fileName, int clientId, string versionId = "")
        {
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest()
                {
                    BucketName = AwsClientConfiguration.BucketName,
                    Key = AwsClientConfiguration.Folder + "/" + clientId + "/" + fileName,
                    VersionId = !string.IsNullOrEmpty(versionId) ? versionId : null
                };

                var response = AmazonS3Client.GetObjectMetadataAsync(request).Result;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                if (ex.InnerException != null && ex.InnerException is AmazonS3Exception awsEx)
                {
                    if (string.Equals(awsEx.ErrorCode, "NoSuchBucket"))
                        _logger.LogInformation($"{nameof(IsFileExists)} error NoSuchBucket");

                    else if (string.Equals(awsEx.ErrorCode, "NotFound"))
                        _logger.LogInformation($"{nameof(IsFileExists)} error NotFound");
                }

                return false;
            }
        }

        /// <summary>
        /// Upload file to AWS
        /// </summary>
        /// <param name="file"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<bool> UploadFileAsync(IFormFile file, int clientId)
        {
            try
            {
                using var newMemoryStream = new MemoryStream();
                file.CopyTo(newMemoryStream);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = AwsClientConfiguration.Folder + "/" + clientId + "/" + file.FileName,
                    BucketName = AwsClientConfiguration.BucketName,
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicReadWrite
                };

                var fileTransferUtility = new TransferUtility(AmazonS3Client);

                await fileTransferUtility.UploadAsync(uploadRequest);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return false;
        }

        /// <summary>
        /// Upload files to AWS
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UploadFilesToAWS(List<Tuple<int, string>> data)
        {
            if (data.Count > 0)
            {
                Parallel.ForEach(data, item => UploadFileAsync(item.Item2, item.Item1));
            }
        }

        /// <summary>
        /// Upload file to AWS
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<bool> UploadFileAsync(string filePath, int clientId)
        {
            try
            {
                if (!File.Exists(filePath))
                    return false;

                using (var stream = File.OpenRead(filePath))
                {
                    var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
                    using var newMemoryStream = new MemoryStream();
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = AwsClientConfiguration.Folder + "/" + clientId + "/" + file.FileName,
                        BucketName = AwsClientConfiguration.BucketName,
                        CannedACL = S3CannedACL.PublicReadWrite
                    };

                    var fileTransferUtility = new TransferUtility(AmazonS3Client);

                    await fileTransferUtility.UploadAsync(uploadRequest);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return false;
        }

        private AwsClientConfiguration GetConfigSection(IConfiguration configuration) => new AwsClientConfiguration
        {
            BucketName = configuration.GetValue<string>("ExternalSystem:AwsSettings:BucketName"),
            Region = configuration.GetValue<string>("ExternalSystem:AwsSettings:Region"),
            AwsAccessKey = configuration.GetValue<string>("ExternalSystem:AwsSettings:AwsAccessKey"),
            AwsSecretAccessKey = configuration.GetValue<string>("ExternalSystem:AwsSettings:AwsSecretAccessKey"),
            Folder = configuration.GetValue<string>("ExternalSystem:AwsSettings:Folder")
        };
    }

    */
    #endregion
}
