using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsS3BucketDeleteApi.Repository.Interfaces
{
    public interface IDeleteRepository
    {
        Task DeleteFileAsync(string bucket, string path);
    }
}
