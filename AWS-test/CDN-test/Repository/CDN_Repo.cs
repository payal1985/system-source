using CDN_test.Api;
using static CDN_test.Models.Aws_Models;

namespace CDN_test.Repository
{
    public class CDN_Repo
    {
        private string _bucketname;
        private bool _imagesonly;

        public CDN_Repo(string BucketName, bool ImagesOnly)
        {
            _bucketname = BucketName;
            _imagesonly = ImagesOnly;
        }

        S3 s3 = new S3();

        private async Task<List<S3File>> getFiles()
        {
            List<S3File> s3files = await s3.ListS3Files(_bucketname, 15);

            if (_imagesonly)
            {
                List<S3File> s3images = s3files.Where(s => S3.ImageTypes.Contains(s.MimeType)).ToList();
                return s3images;
            }
            else
            {
                return s3files;
            }
        }

        public async Task<IEnumerable<S3File>> GetFiles()
        {
            IEnumerable<S3File> s3files = await getFiles();
            return s3files;
        }
    }
}
