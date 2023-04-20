using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AwsS3Download.DBContext;
using AwsS3Download.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Sockets;
using System.Runtime;
using System.Security.Cryptography.Xml;
using System.Text.Encodings.Web;

namespace AwsS3Download.Repository
{
    public class DownloadRepository : IDownloadRepository
    {
        private static IConfiguration _configuration { get; set; }
      //  private static string bucketName { get; set; }
        private static IAmazonS3 awsclient { get; set; }

       // private static AwsInfoContext dbContext { get; set; }

        public DownloadRepository(IConfiguration configuration, AwsInfoContext _dbContext)
        {
            _configuration = configuration;
            //dbContext = _dbContext;
            S3settingsModel s3settings = GetS3Settings(_dbContext);

            //bucketName = s3settings.BucketName; // for the sample get list of file used this line...

            //awsclient = new AmazonS3Client(s3settings.AccessKey, s3settings.SecretKey, RegionEndpoint.USWest2);
            awsclient = new AmazonS3Client(s3settings.AccessKey, s3settings.SecretKey, RegionEndpoint.USEast1);

        }

        static S3settingsModel GetS3Settings(AwsInfoContext _dbContext)
        {
            // for the sample get list of file used this below lines...
            //IConfigurationSection s3BucketSettings = _configuration.GetSection("S3BucketSettings");

            //S3settingsModel s3settings = new S3settingsModel()
            //{
            //    BucketName = s3BucketSettings["bucketName"],
            //    AccessKey = s3BucketSettings["accesskey"],
            //    SecretKey = s3BucketSettings["secretkey"]
            //};

            //from Databse getting the aws key and secrete
            S3settingsModel s3settings = new S3settingsModel();
           
            using (AwsInfoContext dbContext = _dbContext)
            {
                var awsEntity = dbContext.awsInfos.Where(aws => aws.IsActive == true).FirstOrDefault();

                if (awsEntity != null)
                {
                    s3settings.AccessKey = awsEntity.AwsKey;
                    s3settings.SecretKey = awsEntity.AwsKeyValue;
                }
            }

           

            
            
            return s3settings;
        }
        public async Task<List<string>> ListS3Files(AwsModel awsModel)
        {
            List<string> entries = new List<string>();
            try
            {
                if (awsModel.ImageNames != null && awsModel.ImageNames.Count > 0)
                {
                    //var prefix = awsModel.ImageNames[0].Substring(0, awsModel.ImageNames[0].Split('/').LastOrDefault().Length);

                    ListObjectsV2Request request = new ListObjectsV2Request()
                    {
                        BucketName = awsModel.Bucket,
                        Prefix = awsModel.Prefix,
                        //MaxKeys = 1000,
                    };

                    ListObjectsV2Response response;

                    response = await awsclient.ListObjectsV2Async(request);

                    foreach (var file in awsModel.ImageNames)
                    {
                        foreach (S3Object entry in response.S3Objects)
                        {
                            //if (prefix != null)
                            //{
                            if (entry.Key.StartsWith(file) && entry.Size > 0)
                            {
                                entries.Add(GetSignedUrl(awsModel.Bucket, entry.Key, awsModel.SignedUrlValidMinutes));
                                break;
                            }
                            //}

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }           
      

            return entries;
        }

        public async Task<List<string>> ListS3FilesSample(string Prefix, int SignedUrlValidMinutes)
        {
            IConfigurationSection s3BucketSettings = _configuration.GetSection("S3BucketSettings");
            var bucketName = s3BucketSettings["bucketName"];

            ListObjectsV2Request request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                Prefix = Prefix,
                //MaxKeys = 1000,
            };
            List<string> entries = new List<string>();
            ListObjectsV2Response response;

            response = await awsclient.ListObjectsV2Async(request);
            foreach (S3Object entry in response.S3Objects)
            {
                if (Prefix != null)
                {
                    if (entry.Key.StartsWith(Prefix) && entry.Size > 0)
                    {
                        entries.Add(GetSignedUrl(bucketName, entry.Key, SignedUrlValidMinutes));
                        //break;
                    }
                }

            }

            return entries;
        }

        public async Task<string> GetS3File(string bucket, string prefix, int signedUrlValidMinutes)
        {
            string signedUrl = "";
            try
            {   
                var response = await awsclient.ListObjectsAsync(bucket, prefix);
                
                foreach (S3Object entry in response.S3Objects)
                {
                    if (entry.Size > 0)
                    {
                        signedUrl = GetSignedUrl(bucket, entry.Key, signedUrlValidMinutes);
                        break;
                    }
                }

                return signedUrl;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetSignedUrl(string bucketName, string Filename, int ValidMinutes)
        {
            //return awsclient.GeneratePreSignedURL(bucketName, Filename,  DateTime.Now.AddMinutes(ValidMinutes), null);
            return awsclient.GeneratePreSignedURL(bucketName, Filename, DateTime.Now.AddDays(1), null);
        }

        public async Task<byte[]> GetFileBytes(string bucket, string path)
        {
            MemoryStream ms = null;

            try
            {               
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = bucket,
                    Key = path
                };

                using (var response = await awsclient.GetObjectAsync(getObjectRequest))
                {
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {

                        using (ms = new MemoryStream())
                        {                            
                            await response.ResponseStream.CopyToAsync(ms);
                        }
                    }
                }

                if (ms is null || ms.ToArray().Length < 1)
                    throw new FileNotFoundException($"The document '{bucket}{path}' is not found");


                return ms.ToArray();

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<long> DownloadFile(string bucket, string path,string savepath)
        {
            long result=0;
            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = bucket,
                    Key = path
                };

                using (var response = await awsclient.GetObjectAsync(getObjectRequest))
                {
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        CancellationTokenSource source = new CancellationTokenSource();
                        CancellationToken token = source.Token;
                        //string objectKey = Path.getFileName(objectKey);
                        await response.WriteResponseStreamToFileAsync(savepath,true, token);

                        result = response.ContentLength;
                        //result = true;
                    }
                }

               

            }
            catch (Exception ex)
            {
                throw;
            }

            return result;

        }

        public async Task DownloadDirectoryAsync(string bucket, string prefix)
        {
            //var bucketRegion = RegionEndpoint.USEast2;
            //var credentials = new BasicAWSCredentials("AKIAQDRSIWHC7ZVVGN5U", "+FxbarLfFf2nkYDnbnXjWeBeaKXGEpy2eDEEbzHJ");
            //var client = new AmazonS3Client(credentials, bucketRegion);


            ListObjectsV2Request request = new ListObjectsV2Request()
            {
                BucketName = bucket,
                Prefix = prefix,
                //MaxKeys = 1000,
            };

            ListObjectsV2Response response;

            response = await awsclient.ListObjectsV2Async(request);

            //var bucketName = "bucketName";
            //var request = new ListObjectsV2Request
            //{
            //    BucketName = bucket,
            //    Prefix = "prefix"//,
            //    //MaxKeys = 1000
            //};          


            //var response = await client.ListObjectsV2Async(request);
            var utility = new TransferUtility(awsclient);
            var downloadPath = "C:\\ssi_upload\\s3bucket";
            foreach (var obj in response.S3Objects)
            {
                if (obj.Key.Contains(".jpg") || obj.Key.Contains(".png"))
                {
                    utility.Download($"{downloadPath}\\{obj.Key}", bucket, obj.Key);

                }
            }
        }

        public async Task<string> DownloadBase64(string bucket, string path)
        {
            MemoryStream ms = null;

            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = bucket,
                    Key = path
                };

                using (var response = await awsclient.GetObjectAsync(getObjectRequest))
                {
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {

                        using (ms = new MemoryStream())
                        {
                            await response.ResponseStream.CopyToAsync(ms);
                        }
                    }
                }

                if (ms is null || ms.ToArray().Length < 1)
                    throw new FileNotFoundException($"The document '{bucket}{path}' is not found");

                
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<List<string>>> ListS3FileNames(AwsModel awsModel)
        {
            List<List<string>> listFiles = new List<List<string>>();

            try
            {
                if (awsModel.ImageNames != null && awsModel.ImageNames.Count > 0)
                {
                    foreach(var file in awsModel.ImageNames)
                    {
                        var response = await awsclient.ListObjectsAsync(awsModel.Bucket, file);
                        //var list = response.S3Objects.Count == 0 ? new List<string>() : response.S3Objects.Select(x => x.Key.Split('/').LastOrDefault()).ToList();
                        //listFiles.Add(list);
                        List<string> entries = new List<string>();

                        foreach (S3Object entry in response.S3Objects)
                        {
                            if (entry.Size > 0)
                            {
                                var signedUrl = GetSignedUrl(awsModel.Bucket, entry.Key, awsModel.SignedUrlValidMinutes);
                                //break;
                                entries.Add(signedUrl);

                            }
                        }

                        listFiles.Add(entries);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return await Task.Run(() => listFiles);
        }
    }
}
