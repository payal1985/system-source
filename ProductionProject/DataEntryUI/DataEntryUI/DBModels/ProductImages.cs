using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataEntryUI.DBModels
{
    [Table("ProductImages")]
    public class ProductImages
    {
        [Key]
        public int Microsoft_image_URL_ID{ get; set; }
        public int? Microsoft_product_ID{ get; set; }
        public string URL{ get; set; }
        public string Filename{ get; set; }
        public bool? active_fl{ get; set; }
        public DateTime? Load_DateTime{ get; set; }
    }
}
