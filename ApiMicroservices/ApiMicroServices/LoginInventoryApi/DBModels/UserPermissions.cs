using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.DBModels
{
    [Table("user_perms")]
    public class UserPermissions
    {
       public int user_id{get; set;}
       public int client_id{get; set;}
       public int permission{get; set;}
       public int createid{get; set;}
       public DateTime createdate{get; set;}
       public int createprocess{get; set;}
       public int updateid{get; set;}
       public DateTime updatedate {get; set;}
       public int updateprocess{get; set;}
    }
}
