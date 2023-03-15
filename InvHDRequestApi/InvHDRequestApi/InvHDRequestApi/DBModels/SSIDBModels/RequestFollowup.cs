using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.DBModels.SSIDBModels
{
    [Table("request_followup")]
    public class RequestFollowup
    {
        [Key]
        public int request_followup_id{get;set;}
        public int request_id{get;set;}
        public string? note{get;set;}
        public bool active{get;set;}
        public bool client_visible{get;set;}
        public int createid{get;set;}
        public DateTime createdate{get;set;}
        public int createprocess{get;set;}
        public int updateid{get;set;}
        public DateTime updatedate{get;set;}
        public int updateprocess{get;set;}
    }
}
