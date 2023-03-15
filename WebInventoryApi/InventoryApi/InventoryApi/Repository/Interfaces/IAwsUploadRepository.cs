using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IAwsUploadRepository
    {
        Task<bool> UploadImageAsync(IFormFile file, string bucket, string path);
    }
}
