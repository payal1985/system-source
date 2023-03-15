using InventoryApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApi.Repository
{
    public class AwsUploadRepository : IAwsUploadRepository
    {
        private readonly HttpClient _httpClient;

        public AwsUploadRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> UploadImageAsync(IFormFile file, string bucket, string path)
        {
            bool result = false;

            try
            {
                string remorturi = _httpClient.BaseAddress + "uploadfiles?bucket=" + bucket + "&path=" + path;

                byte[] fileBytes;
                //using (var br = new BinaryReader(file.OpenReadStream()))
                //    data = br.ReadBytes((int)file.OpenReadStream().Length);

                using (var ms = new System.IO.MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                    //string s = Convert.ToBase64String(fileBytes);
                    // act on the Base64 data
                }

                ByteArrayContent bytes = new ByteArrayContent(fileBytes);

               
               // fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                multiContent.Add(bytes, "file", path.Split('/').LastOrDefault());

               // StringContent content = new StringContent(JsonConvert.SerializeObject(file), Encoding.UTF8, "application/form-data");

                using (var response = await _httpClient.PostAsync(remorturi, multiContent))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();

                    result = Convert.ToBoolean(apiResponse) ? true : false;
                    //receivedReservation = JsonConvert.DeserializeObject<Reservation>(apiResponse);
                }
            }
            catch(Exception ex)
            {
                throw;
            }

            return result;
        }
    }
}
