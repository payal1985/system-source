using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.SSIDBModels
{
    [Table("rq_attachment")]
    public class RequestAttachment
    {
        [Key]
        public int request_id { get; set; }
       // [Key]
        public int attachment_id { get; set; }
    }
}
