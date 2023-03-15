using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AwsS3BucketApi.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AwsS3BucketApi.Repository
{
    public class Aws3Repository : IAws3Repository
    {
        private BasicAWSCredentials credentials;
        private AmazonS3Client client;
        private AwsClientConfiguration _awsClientConfiguration;
        //private IAppConfigurationRepository _appConfigurationRepository;
        public Aws3Repository(IAppConfigurationRepository _appConfigurationRepository,IConfiguration _configuration)
        {
            _awsClientConfiguration = _appConfigurationRepository.GetConfigSection(_configuration);
            credentials = new BasicAWSCredentials(_awsClientConfiguration.AwsAccessKey, _awsClientConfiguration.AwsSecretAccessKey);
            client = new AmazonS3Client(credentials, RegionEndpoint.USWest2);
        }

        public async Task DeleteFileAsync(string fileName, string versionId = "")
        {
            int clientId = 12;

            DeleteObjectRequest request = new DeleteObjectRequest
            {
                BucketName = _awsClientConfiguration.BucketName,
                Key = _awsClientConfiguration.Folder + clientId + "/" + fileName
            };

            if (!string.IsNullOrEmpty(versionId))
                request.VersionId = versionId;

            await client.DeleteObjectAsync(request);
        }

        public async Task<byte[]> DownloadFileAsync(string file,int clientId)
        {
            MemoryStream ms = null;

            try
            {     
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = _awsClientConfiguration.BucketName,
                    Key = _awsClientConfiguration.Folder + clientId + "/" + file // //clientId need to pass dynamically from controller or business logic
                };

                using (var response = await client.GetObjectAsync(getObjectRequest))
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
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsFileExists(string fileName, string versionId ="")
        {
            try
            {
                int clientId = 12;

                GetObjectMetadataRequest request = new GetObjectMetadataRequest()
                {
                    BucketName = _awsClientConfiguration.BucketName,
                    Key = _awsClientConfiguration.Folder + clientId + "/" + fileName,
                    VersionId = !string.IsNullOrEmpty(versionId) ? versionId : null
                };

                var response = client.GetObjectMetadataAsync(request).Result;

                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is AmazonS3Exception awsEx)
                {
                    if (string.Equals(awsEx.ErrorCode, "NoSuchBucket"))
                        return false;

                    else if (string.Equals(awsEx.ErrorCode, "NotFound"))
                        return false;
                }

                throw;
            }
        }


        public async Task<bool> UploadFileAsync(IFormFile file)
        {
            try
            {
                int clientId = 12;
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = _awsClientConfiguration.Folder + clientId + "/" + file.FileName, // //clientId need to pass dynamically from controller or business logic
                        BucketName = _awsClientConfiguration.BucketName,
                        ContentType = file.ContentType
                    };

                    uploadRequest.CannedACL = S3CannedACL.PublicRead;

                    var fileTransferUtility = new TransferUtility(client);

                    await fileTransferUtility.UploadAsync(uploadRequest);

                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
