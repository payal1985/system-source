using AwsS3BucketUploadApi.Repository;
using AwsS3BucketUploadApi.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsS3BucketUploadApi.Mappers
{
    public static class RepositoryMapper
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IUploadRepository, UploadRepository>();
        }
    }
}
