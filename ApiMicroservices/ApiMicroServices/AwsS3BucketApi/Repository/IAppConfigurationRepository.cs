using AwsS3BucketApi.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsS3BucketApi.Repository
{
    public interface IAppConfigurationRepository
    {
        AwsClientConfiguration GetConfigSection(IConfiguration configuration);
    }
}
