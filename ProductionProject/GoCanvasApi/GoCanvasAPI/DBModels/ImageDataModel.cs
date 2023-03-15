using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoCanvasAPI.DBModels
{
    [Table("ImageDataModel")]
    public class ImageDataModel
    {
        [Key]
        public Guid Image_table_Id { get; set; }
        public string ImageId { get; set; }
        public string ImageNumber { get; set; }
        public string SubmissionId { get; set; }

    }
}
