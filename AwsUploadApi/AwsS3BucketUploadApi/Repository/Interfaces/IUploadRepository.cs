using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsS3BucketUploadApi.Repository.Interfaces
{
    public interface IUploadRepository
    {
        Task<bool> FilesUpload(string bucket, string path, IFormFile file);
        //Task<Image> FilesDownloadHttp(string url, string path);
    }
}
