using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.AwsConfiguration
{
    public class AwsClientConfiguration
    {
        public string BucketName { get; set; }
        public string Region { get; set; }
        public string AwsAccessKey { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public string Folder { get; set; }

        public AwsClientConfiguration(IConfiguration configuration)
        {
            BucketName = configuration.GetValue<string>("AwsConfig:BuketName");
            Region = configuration.GetValue<string>("AwsConfig:Region");
            AwsAccessKey = configuration.GetValue<string>("AwsConfig:AwsAccessKey");
            AwsSecretAccessKey = configuration.GetValue<string>("AwsConfig:AwsSecretAccessKey");
            Folder = configuration.GetValue<string>("AwsConfig:Folder");
        }
    }
}
