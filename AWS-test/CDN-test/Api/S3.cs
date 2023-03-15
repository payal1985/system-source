using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.Collections.Specialized;
using System.IO.Compression;
using System.Net.Mime;
using static CDN_test.Models.Aws_Models;

namespace CDN_test.Api
{
    public class S3
    {
        private static string bucketName { get; set; }
        private static IAmazonS3 awsclient { get; set; }

        public S3()
        {
            S3settings s3settings = GetS3Settings();
            bucketName = s3settings.BucketName;
            awsclient = new AmazonS3Client(s3settings.AccessKey, s3settings.SecretKey, RegionEndpoint.USWest2);
        }

        static S3settings GetS3Settings()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            IConfigurationSection s3BucketSettings = configuration.GetSection("S3BucketSettings");

            S3settings s3settings = new S3settings()
            {
                BucketName = s3BucketSettings["bucketName"],
                AccessKey = s3BucketSettings["accesskey"],
                SecretKey = s3BucketSettings["secretkey"]
            };

            return s3settings;
        }

        public string GetSignedUrl(string Filename, int ValidMinutes)
        {
            return awsclient.GeneratePreSignedURL(bucketName, Filename, DateTime.Now.AddMinutes(ValidMinutes), null);
        }

        public async Task<(Stream FileStream, string ContentType)> ReadFileAsync(string fileName)
        {
            TransferUtility fileTransferUtility = new TransferUtility(awsclient);
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = fileName
            };

            try
            {
                var objectResponse = await fileTransferUtility.S3Client.GetObjectAsync(request);
                return (objectResponse.ResponseStream, objectResponse.Headers.ContentType);
            }
            catch (Exception ex)
            {
                return (null, "Error: " + ex.Message);
            }
        }

        public async Task<string> DownloadS3File(string fileName, string savePath)
        {
            if (File.Exists(savePath))
            {
                try
                {
                    File.Delete(savePath);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            if (fileName.StartsWith("/"))
            {
                fileName = fileName.Substring(1, fileName.Length - 1);
            }
            var s3stream = await ReadFileAsync(fileName);
            var filestream = File.Create(savePath);
            s3stream.FileStream.CopyTo(filestream);
            s3stream.FileStream.Close();
            filestream.Close();
            filestream.Dispose();
            return s3stream.ContentType;
        }

        public async Task<byte[]> GetFileBytes(string fileurl)
        {
            var response = await ReadFileAsync(fileurl);

            Stream s3file = response.FileStream;
            byte[] putFile = new byte[response.FileStream.Length];
            using (MemoryStream outStream = new MemoryStream())
            {
                int count;
                while ((count = s3file.Read(putFile, 0, putFile.Length)) > 0)
                {
                    outStream.Write(putFile, 0, count);
                }
                return outStream.ToArray();
            }
        }

        public async Task<bool> UploadS3Async(string Filename, Stream PostedFile, string Folder)
        {
            S3StorageClass storageclass = S3StorageClass.Standard;
            PutObjectRequest req = new PutObjectRequest
            {
                BucketName = bucketName,
                CannedACL = S3CannedACL.Private,
                Key = string.Format("{0}/{1}", Folder, Filename),
                StorageClass = storageclass,
                InputStream = PostedFile
            };

            PutObjectResponse response = await awsclient.PutObjectAsync(req);
            bool status = response.HttpStatusCode == System.Net.HttpStatusCode.OK ? true : false;
            return status;
        }

        public async Task<bool> DeleteS3File(string Filename, string Folder = "patientuploads")
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = string.Format("{0}/{1}", Folder, Filename)
            };

            try
            {
                await awsclient.DeleteObjectAsync(deleteObjectRequest);
            }
            catch { }
            return true;
        }

        public async Task<List<S3File>> ListS3Files(string Prefix, int SignedUrlValidMinutes)
        {
            ListObjectsV2Request request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                MaxKeys = 1000,
            };
            List<S3File> entries = new List<S3File>();
            ListObjectsV2Response response;
            do
            {
                response = await awsclient.ListObjectsV2Async(request);
                foreach (S3Object entry in response.S3Objects)
                {
                    if (Prefix != null)
                    {
                        if (entry.Key.StartsWith(Prefix) && entry.Size > 0)
                        {
                            string mimetype = MimeType(Path.GetExtension(entry.Key));
                            entries.Add(new S3File()
                            {
                                Filename = Path.GetFileName(entry.Key),
                                LastModified = entry.LastModified,
                                Size = entry.Size,
                                StorageClass = entry.StorageClass,
                                MimeType = mimetype,
                                SignedUrl = GetSignedUrl(entry.Key, SignedUrlValidMinutes)
                            });
                            Console.WriteLine("MimeType: " + MimeType(Path.GetExtension(entry.Key)));
                        }
                    }
                    else
                    {
                        entries.Add(new S3File()
                        {
                            Filename = Path.GetFileName(entry.Key),
                            LastModified = entry.LastModified,
                            Size = entry.Size,
                            StorageClass = entry.StorageClass,
                            MimeType = MimeType(Path.GetExtension(entry.Key)),
                            SignedUrl = GetSignedUrl(entry.Key, SignedUrlValidMinutes)
                        });
                    }
                }
            } while (response.IsTruncated);

            return entries;
        }

        public static string[] ImageTypes = { "image/jpeg", "image/gif", "image/png" };

        private static string MimeType(string ext)
        {
            string mimetype = "text/html";

            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    mimetype = "image/jpeg";
                    break;
                case ".doc":
                    mimetype = "application/msword";
                    break;
                case ".rtf":
                    mimetype = "application/rtf";
                    break;
                case ".docx":
                    mimetype = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".ppt":
                    mimetype = "application/vnd.ms-powerpoint";
                    break;
                case ".pptx":
                    mimetype = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
                case ".xls":
                    mimetype = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    mimetype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".pdf":
                    mimetype = "application/pdf";
                    break;
                case ".gif":
                    mimetype = "image/gif";
                    break;
                case ".txt":
                    mimetype = "text/plain";
                    break;
                case ".png":
                    mimetype = "image/png";
                    break;
                case ".zip":
                    mimetype = "application/zip";
                    break;
                case ".stl":
                    mimetype = "application/SLA";
                    break;
                case ".url":
                    mimetype = "application/octet-stream";
                    break;
                default:
                    mimetype = "text/html";
                    break;
            }
            return mimetype;
        }

        private ContentDisposition ContentDisposition(string filename, string MimeType)
        {
            ContentDisposition cd = new ContentDisposition
            {
                FileName = filename,
                Inline = false
            };

            switch (MimeType)
            {
                case "image/jpeg":
                case "image/gif":
                case "image/png":
                case "application/pdf":
                case "text/plain":
                    cd.Inline = true;
                    break;
            }

            return cd;
        }

    }
}
