using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.DBModels
{
    [Table("user_type")]
    public class UserTypes
    {
        [Key]
        public int user_type_id{get; set;}
        public string name{get; set;}
        public string? description{get; set;}
        public bool active{get; set;}
        public int createid{get; set;}
        public DateTime createdate{get; set;}
        public int createprocess{get; set;}
        public int updateid{get; set;}
        public DateTime updatedate {get; set;}
        public int updateprocess{get; set;}
    }
}
