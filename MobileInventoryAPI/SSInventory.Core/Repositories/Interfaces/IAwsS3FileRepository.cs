using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories.Interfaces
{
    public interface IAwsS3FileRepository
    {
        Task<bool> DeleteFileAsync(string bucket, string path);
        Task<bool> UploadImageAsync(IFormFile file, string bucket, string path);
        Task<string> DownloadFileAsync(string bucket, string path);
    }
}
