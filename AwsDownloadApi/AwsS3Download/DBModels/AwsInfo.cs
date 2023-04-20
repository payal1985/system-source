using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AwsS3Download.DBModels
{
    [Table("AwsInfo")]
    public class AwsInfo
    {
        [Key]
        public int AwsKeyID { get; set; }
        public string AwsKey { get; set; }
        public string AwsKeyValue { get; set; }
        public bool IsActive { get; set; }
    }
}
