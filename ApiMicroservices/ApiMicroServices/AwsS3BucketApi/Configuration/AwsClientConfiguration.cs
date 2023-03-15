using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsS3BucketApi.Configuration
{

    public class AwsClientConfiguration
    {

        public string BucketName { get; set; }
        public string Region { get; set; }
        public string AwsAccessKey { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public string Folder { get; set; }

        //public AwsClientConfiguration(IConfiguration configuration)
        //{            
        //    BucketName = configuration.GetValue<string>("Config:BuketName");
        //    Region = configuration.GetValue<string>("Config:Region");
        //    AwsAccessKey = configuration.GetValue<string>("Config:AwsAccessKey");
        //    AwsSecretAccessKey = configuration.GetValue<string>("Config:AwsSecretAccessKey");
        //    Folder = configuration.GetValue<string>("Config:Folder");
        //}

    }
}
