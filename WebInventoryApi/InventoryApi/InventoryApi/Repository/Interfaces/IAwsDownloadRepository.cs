using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IAwsDownloadRepository
    {
        Task<string> DownloadFileAsync(string bucket, string path);
    }
}
