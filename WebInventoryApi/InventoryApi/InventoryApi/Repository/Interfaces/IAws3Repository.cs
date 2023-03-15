using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IAws3Repository
    {
        //Task<byte[]> DownloadFileAsync(string bucket, string path);
        Task<long> DownloadFileAsync(string bucket, string path,string savepath);
        Task<bool> IsFileExists(string fileName, int clientId, string versionId);
        Task<bool> UploadFileAsync(IFormFile file, int clientId);
        Task<List<string>> ListS3FileNames(int clientId,int inventoryId);
        Task<bool> UploadImageAsync(IFormFile file, int clientId, string existingImgName);
        Task<bool> IsFileExists(string bucket, string path);
        Task<List<string>> ListS3FileNames(string bucket, string path);
    }
}
