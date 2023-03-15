using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using SSInventory.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class AwsS3FileRepository: IAwsS3FileRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public AwsS3FileRepository(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<bool> UploadImageAsync(IFormFile file, string bucket, string path)
        {
            bool result = false;

            try
            {
                string remorturi = _httpClient.BaseAddress + "AwsS3UploadApi/api/upload/uploadfiles?bucket=" + bucket + "&path=" + path;

                byte[] fileBytes;
                using (var ms = new System.IO.MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }

                ByteArrayContent bytes = new ByteArrayContent(fileBytes);



                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                multiContent.Add(bytes, "file", path.Split('/').LastOrDefault());


                using (var response = await _httpClient.PostAsync(remorturi, multiContent))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();

                    result = Convert.ToBoolean(apiResponse) ? true : false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public async Task<string> DownloadFileAsync(string bucket, string path)
        {
            string imgData = "";
            try
            {
                var signedUrlValidMinutes = Convert.ToInt32(_config["ExternalSystem:SignedUrlValidMinutes"]);
                string remorturi = _httpClient.BaseAddress + "AwsS3DownloadApi/api/download/getfiles?bucket=" + bucket + "&path=" + path + "&signedUrlValidMinutes=" + signedUrlValidMinutes;

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

        public async Task<bool> DeleteFileAsync(string bucket, string path)
        {
            bool result = false;
            try
            {
                string remorturi = _httpClient.BaseAddress + "AwsS3DeleteApi/api/delete/deletefile?bucket=" + bucket + "&path=" + path;

                using (var response = await _httpClient.DeleteAsync(remorturi))
                {
                    result = response.IsSuccessStatusCode;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
