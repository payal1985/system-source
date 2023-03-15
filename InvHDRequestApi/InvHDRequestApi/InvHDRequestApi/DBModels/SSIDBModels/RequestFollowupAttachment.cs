using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.DBModels.SSIDBModels
{
    [Table("rqfu_attachment")]
    public class RequestFollowupAttachment
    {
       // [Key,Column(Order =0)]        
        public int request_followup_id { get; set; }
        public int request_id { get; set; }
       // [Key, Column(Order = 1)]
        public int attachment_id { get; set; }
    }
}
