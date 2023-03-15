using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsS3BucketApi.Repository
{
    public interface IAws3Repository
    {
        Task<byte[]> DownloadFileAsync(string file, int clientId);
        Task<bool> UploadFileAsync(IFormFile file);
        Task DeleteFileAsync(string fileName, string versionId = "");
        bool IsFileExists(string fileName, string versionId = "");
    }
}
