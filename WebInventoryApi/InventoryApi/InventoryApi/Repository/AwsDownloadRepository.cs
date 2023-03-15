using InventoryApi.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InventoryApi.Repository
{
    public class AwsDownloadRepository : IAwsDownloadRepository
    {
        private readonly HttpClient _httpClient;
        public AwsDownloadRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> DownloadFileAsync(string bucket, string path)
        {
            //MemoryStream ms = null;
            //byte[] imgData= new byte[0];
            string imgData= "";
            try
            {
                string remorturi = _httpClient.BaseAddress + "getfiles?bucket=" + bucket + "&path=" + path;

                using (var response = await _httpClient.GetAsync(remorturi))
                {
                    imgData = await response.Content.ReadAsStringAsync();                    
                }

                return imgData;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
