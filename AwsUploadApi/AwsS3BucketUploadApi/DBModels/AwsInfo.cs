using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AwsS3BucketUploadApi.DBModels
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
