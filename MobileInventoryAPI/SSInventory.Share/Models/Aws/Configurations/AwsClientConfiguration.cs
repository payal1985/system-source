namespace SSInventory.Share.Models.Aws.Configurations
{
    public class AwsClientConfiguration
    {
        public string BucketName { get; set; }
        public string Region { get; set; }
        public string AwsAccessKey { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public string Folder { get; set; }
        public string Type { get; set; }

    }
}
