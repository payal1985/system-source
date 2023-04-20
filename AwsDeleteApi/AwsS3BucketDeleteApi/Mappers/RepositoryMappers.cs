using AwsS3BucketDeleteApi.Repository;
using AwsS3BucketDeleteApi.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsS3BucketDeleteApi.Mappers
{
    public static class RepositoryMappers
    {
        public static void Register(IServiceCollection services)
        {
            //services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IDeleteRepository, DeleteRepository>();
        }
    }
}
