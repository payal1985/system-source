using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.SSIDBModels
{
    [Table("attachments")]
    public class Attachments
    {
        [Key]
        public int attachment_id { get; set; }
        public string fname   { get; set; }
        public string? uname { get; set; }
        public int attachment_type_id { get; set; }
        public string data    { get; set; }
        public string? info    { get; set; }
        public string? sizef { get; set; }
        public bool deleted { get; set; }
        public int createid { get; set; }
        public DateTime createdate { get; set; }
        public int createprocess { get; set; }
        public int updateid { get; set; }
        public DateTime updatedate { get; set; }
        public int updateprocess { get; set; }
    }
}
