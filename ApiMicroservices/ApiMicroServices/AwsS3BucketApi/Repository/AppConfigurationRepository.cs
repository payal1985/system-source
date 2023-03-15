using AwsS3BucketApi.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsS3BucketApi.Repository
{
    public class AppConfigurationRepository : IAppConfigurationRepository
    {
        //IConfiguration configuration;

        public AwsClientConfiguration GetConfigSection(IConfiguration configuration)
        {
            AwsClientConfiguration awsConfig = new AwsClientConfiguration();
           

            awsConfig.BucketName = configuration.GetValue<string>("Config:BuketName");
            awsConfig.Region = configuration.GetValue<string>("Config:Region");
            awsConfig.AwsAccessKey = configuration.GetValue<string>("Config:AwsAccessKey");
            awsConfig.AwsSecretAccessKey = configuration.GetValue<string>("Config:AwsSecretAccessKey");
            awsConfig.Folder = configuration.GetValue<string>("Config:Folder");

            return awsConfig;
        }
    }
}
