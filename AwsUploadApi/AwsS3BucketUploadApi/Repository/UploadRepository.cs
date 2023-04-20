using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AwsS3BucketUploadApi.DBContext;
using AwsS3BucketUploadApi.Mappers;
using AwsS3BucketUploadApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AwsS3BucketUploadApi.Repository
{
    public class UploadRepository : IUploadRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _accessKey;
        private readonly string _secretAccessKey;
        private readonly BasicAWSCredentials credentials;
        private readonly AmazonS3Client client;
        //AwsInfoContext _dbContext;
        private readonly AwsFeatures _awsFeature;


        public UploadRepository(IConfiguration configuration, AwsFeatures awsFeatures)
        {
            _configuration = configuration;
            _awsFeature = awsFeatures;

            //_dbContext = dbContext;

            //var awsEntity= _dbContext.awsInfos.FirstOrDefault(aws => aws.IsActive == true);

            //_accessKey = awsEntity.AwsKey;
            //_secretAccessKey = awsEntity.AwsKeyValue;

            //_accessKey = configuration.GetValue<string>("AwsConfig:AwsAccessKey");
            //_secretAccessKey = configuration.GetValue<string>("AwsConfig:AwsSecretAccessKey");

            _accessKey = _awsFeature.AwsKeys[0];
            _secretAccessKey = _awsFeature.AwsKeys[1];

            credentials = new BasicAWSCredentials(_accessKey, _secretAccessKey);
            client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
        }
        public async Task<bool> FilesUpload(string bucket, string path, IFormFile file)
        {
            try
            {
              
                //var bucketName = url.Replace("/", "").Split(':').LastOrDefault().Split('.').FirstOrDefault();

                /* //block of commented code is useful when  implement the DNS Url approach
                 
                //string splitUrl = url.Replace("/","").Split(':').LastOrDefault();               
                //var splitString = splitUrl.Split('.');
                //var bucketName = $"{splitString[0]}-" +
                //                $"{_configuration.GetValue<string>("AwsConfig:Region")}-" +
                //                $"assest-" +
                //                $"{_configuration.GetValue<string>("AwsConfig:env")}-" +
                //                $"{splitString[2]}";
                */

                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = string.Format("{0}/{1}", path, file.FileName),
                        //Key = path, 
                        BucketName = bucket,
                        ContentType = file.ContentType
                    };

                    //uploadRequest.CannedACL = S3CannedACL.PublicRead; // final release need to comment this line.
                    uploadRequest.CannedACL = S3CannedACL.Private; 

                    var fileTransferUtility = new TransferUtility(client);

                    await fileTransferUtility.UploadAsync(uploadRequest);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
