using AwsS3BucketUploadApi.DBContext;
using AwsS3BucketUploadApi.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsS3BucketUploadApi.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        AwsInfoContext _dbContext;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, AwsFeatures awsFeatureMap, AwsInfoContext dbContext)
        {
            _dbContext = dbContext;

            if (awsFeatureMap.AwsKeys == null)
            {
                awsFeatureMap.AwsKeys = new List<string>();
            }

            if (awsFeatureMap.AwsKeys.Count <= 0)
            {
                var features = GetCurrentAwsFeatures();
                awsFeatureMap.AwsKeys = features;
            }

            await _next(httpContext);
        }

        public List<string> GetCurrentAwsFeatures()
        {
            List<string> awsFeatures = new List<string>();

            //_dbContext = dbContext;
            var awsEntity = _dbContext.awsInfos.FirstOrDefault(aws => aws.IsActive == true);

            awsFeatures.Add(awsEntity.AwsKey);
            awsFeatures.Add(awsEntity.AwsKeyValue);

            return awsFeatures;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }
    }
}
