using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using AwsS3BucketDeleteApi.Mappers;
using AwsS3BucketDeleteApi.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AwsS3BucketDeleteApi.Repository
{
    public class DeleteRepository : IDeleteRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _accessKey;
        private readonly string _secretAccessKey;
        private readonly BasicAWSCredentials credentials;
        private readonly AmazonS3Client client;
        //AwsInfoContext _dbContext;
        readonly AwsFeatures _awsFeature;

        public DeleteRepository(IConfiguration configuration, AwsFeatures awsFeatures)
        {
            _configuration = configuration;
            _awsFeature = awsFeatures;

            _accessKey = _awsFeature.AwsKeys[0];
            _secretAccessKey = _awsFeature.AwsKeys[1];

            //_accessKey = configuration.GetValue<string>("AwsConfig:AwsAccessKey");
            //_secretAccessKey = configuration.GetValue<string>("AwsConfig:AwsSecretAccessKey");

            credentials = new BasicAWSCredentials(_accessKey, _secretAccessKey);
            client = new AmazonS3Client(credentials, RegionEndpoint.USWest2);
        }

        public async Task DeleteFileAsync(string bucket, string path)
        {          

            DeleteObjectRequest request = new DeleteObjectRequest
            {
                BucketName = bucket,
                Key = path
            };
                       
            await client.DeleteObjectAsync(request);            
        }

    }
}
