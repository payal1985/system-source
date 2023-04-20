using AwsS3Download.Models;

namespace AwsS3Download.Repository
{
    public interface IDownloadRepository
    {
        Task<List<string>> ListS3Files(AwsModel awsModel);
        Task<List<string>> ListS3FilesSample(string Prefix, int SignedUrlValidMinutes);

        Task<string> GetS3File(string bucket, string prefix, int signedUrlValidMinutes);

        Task<byte[]> GetFileBytes(string bucket, string prefix);
        Task<long> DownloadFile(string bucket, string prefix,string savepath);
        Task<string> DownloadBase64(string bucket, string prefix);

        Task DownloadDirectoryAsync(string bucket, string prefix);
        Task<List<List<string>>> ListS3FileNames(AwsModel awsModel);
    }
}
