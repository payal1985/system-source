namespace AwsS3Download.Models
{
    public class AwsModel
    {
        public string Bucket { get; set; }
        public string? Prefix { get; set; }

        public int SignedUrlValidMinutes { get; set; }
        public List<string> ImageNames { get; set; }

    }
}
