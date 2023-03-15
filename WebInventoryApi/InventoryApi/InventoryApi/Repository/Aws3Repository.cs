using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using InventoryApi.AwsConfiguration;
using InventoryApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace InventoryApi.Repository
{
    public class Aws3Repository : IAws3Repository
    {
        private BasicAWSCredentials credentials;
        private AmazonS3Client client; 
        private AwsClientConfiguration _awsClientConfiguration;
        private IConfiguration _configuration;
        public Aws3Repository(IConfiguration configuration)
        {
            _configuration = configuration;
            _awsClientConfiguration = new AwsClientConfiguration(configuration);
            credentials = new BasicAWSCredentials(_awsClientConfiguration.AwsAccessKey, _awsClientConfiguration.AwsSecretAccessKey);
            client = new AmazonS3Client(credentials, RegionEndpoint.USWest2);
        }
        //public async Task<byte[]> DownloadFileAsync(string bucket, string path)
        public async Task<long> DownloadFileAsync(string bucket, string path,string savepath)
        {
            long result = 0;
           // byte[] result = new byte[0];
            //MemoryStream ms = null;
            //string url= "https://systemsource.s3.us-west-2.amazonaws.com/";
            //string path = _awsClientConfiguration.Folder + clientId + "/" + file;
            try
            {
                using (var client = new HttpClient())
                {
                   
                    client.BaseAddress = new Uri(_configuration.GetValue<string>("AwsDownloadUrl"));
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync($"downloadfile?bucket={bucket}&prefix={path}&savepath={savepath}");
                    if (response.IsSuccessStatusCode)
                    {
                        string resStr = await response.Content.ReadAsStringAsync();
                        result = long.Parse(resStr);
                        //result = await response.Content.ReadAsByteArrayAsync();
                        //var result = await response.Content.ReadAsStringAsync();

                        //using (ms = new MemoryStream())
                        //{
                        //    await response.Content.CopyToAsync(ms);
                        //}
                    }
                }
                return result;
               
                //GetObjectRequest getObjectRequest = new GetObjectRequest
                //{
                //    BucketName = _awsClientConfiguration.BucketName,
                //    Key = _awsClientConfiguration.Folder + clientId + "/" + file // //clientId need to pass dynamically from controller or business logic
                //};

                //using (var response = await client.GetObjectAsync(getObjectRequest))
                //{
                //    if (response.HttpStatusCode == HttpStatusCode.OK)
                //    {
                //        using (ms = new MemoryStream())
                //        {
                //            await response.ResponseStream.CopyToAsync(ms);
                //        }
                //    }
                //}


                //if (ms is null || ms.ToArray().Length < 1)
                //    throw new FileNotFoundException(string.Format("The document '{0}' is not found", file));

                //return ms.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsFileExists(string fileName,int clientId, string versionId = "")
        {
            try
            {
                //int clientId = 127;

                GetObjectMetadataRequest request = new GetObjectMetadataRequest()
                {
                    BucketName = _awsClientConfiguration.BucketName,
                    Key = _awsClientConfiguration.Folder + clientId + "/" + fileName,
                    VersionId = !string.IsNullOrEmpty(versionId) ? versionId : null
                };

                var response = await client.GetObjectMetadataAsync(request);

                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is AmazonS3Exception awsEx)
                {
                    if (string.Equals(awsEx.ErrorCode, "NoSuchBucket"))
                        return false;

                    else if (string.Equals(awsEx.ErrorCode, "NotFound"))
                        return false;
                }

                return false;
               // throw;
            }
        }

        public async Task<bool> UploadFileAsync(IFormFile file,int clientId)
        {
            try
            {
             
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = _awsClientConfiguration.Folder + clientId + "/" + "fimg/" + file.FileName, // //clientId need to pass dynamically from controller or business logic
                        BucketName = _awsClientConfiguration.BucketName,
                        ContentType = file.ContentType
                    };

                    uploadRequest.CannedACL = S3CannedACL.PublicRead;

                    var fileTransferUtility = new TransferUtility(client);

                    await fileTransferUtility.UploadAsync(uploadRequest);

                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> UploadImageAsync(IFormFile file, int clientId,string existingImgName)
        {
            try
            {

                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        //Key = _awsClientConfiguration.Folder + clientId + "/" + file.FileName, // //clientId need to pass dynamically from controller or business logic
                        Key = _awsClientConfiguration.Folder + clientId + "/" + existingImgName, // //clientId need to pass dynamically from controller or business logic
                        BucketName = _awsClientConfiguration.BucketName,
                        ContentType = file.ContentType
                    };

                    uploadRequest.CannedACL = S3CannedACL.PublicRead;

                    var fileTransferUtility = new TransferUtility(client);

                    await fileTransferUtility.UploadAsync(uploadRequest);

                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<string>> ListS3FileNames(int clientId, int inventoryId)
        {

            //GetObjectRequest getObjectRequest = new GetObjectRequest
            //{
            //    BucketName = _awsClientConfiguration.BucketName,
            //    Key = _awsClientConfiguration.Folder + clientId + "/" + file // //clientId need to pass dynamically from controller or business logic
            //};

            //using (var response = await client.GetObjectAsync(getObjectRequest))
            //{ }

                //var awsAccessKey = "dasdasda";
                //var awsSecretKey = "asdadasdsecret";
                //var region = Amazon.RegionEndpoint.USEast1;
                //using (var client = new AmazonS3Client(awsAccessKey, awsSecretKey, region))
                //{

            string bucket = _awsClientConfiguration.BucketName;
            string prefix = _awsClientConfiguration.Folder + clientId + "/" + "fimg/"+ inventoryId;

            //Key = _awsClientConfiguration.Folder + clientId + "/" + "fimg/" + file.FileName, // //clientId need to pass dynamically from controller or business logic
              //          BucketName = _awsClientConfiguration.BucketName,
              
                var response = await client.ListObjectsAsync(bucket, prefix);
                return response.S3Objects.Count == 0 ? new List<string>() : response.S3Objects.Select(x => x.Key.Split('/').LastOrDefault()).ToList();
           // }
        }


        public async Task<bool> IsFileExists(string bucket,string path)
        {
            bool result = false;

            try
            {
                bucket = _awsClientConfiguration.BucketName;

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44318/api/download/fileexists?bucket=" + bucket + "&path=" +path))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<bool>(apiResponse);
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }

            return await Task.Run(() => result);
        }

        public async Task<List<string>> ListS3FileNames(string bucket, string path)
        {
            List<string> result = new List<string>();

            try
            {
                bucket = _awsClientConfiguration.BucketName;

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44318/api/download/listS3filenames?bucket=" + bucket + "&path=" + path))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<List<string>>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return await Task.Run(() => result);
        }

    }
}
