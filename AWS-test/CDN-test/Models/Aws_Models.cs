using Amazon.S3;

namespace CDN_test.Models
{
    public class Aws_Models
    {
        internal class S3settings
        {
            public string BucketName { get; set; }
            public string AccessKey { get; set; }
            public string SecretKey { get; set; }
        }

        public class S3File
        {
            public string Filename { get; set; }
            public string MimeType { get; set; }
            public DateTime? LastModified { get; set; }
            public long? Size { get; set; }
            public S3StorageClass StorageClass { get; set; }
            public string SignedUrl { get; set; }
        }
    }
}
