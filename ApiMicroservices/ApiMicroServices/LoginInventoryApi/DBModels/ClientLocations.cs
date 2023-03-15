using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.DBModels
{
    [Table("client_locations")]
    public class ClientLocations
    {
        [Key]
       public int location_id{get; set;}
       public int client_id{get; set;}
       public string location_type{get; set;}
       public string? client_code{get; set;}
       public string location_name{get; set;}
       public string? addr1{get; set;}
       public string? addr2{get; set;}
       public string? city{get; set;}
       public string? state {get; set;}
       public string? zip{get; set;}
       public string? country{get; set;}
       public string? currency {get; set;}
       public string? client_code2 {get; set;}
       public int? property_manager_contact_ID{get; set;}
       public int createid{get; set;}
       public DateTime createdate{get; set;}
       public int createprocess{get; set;}
       public int updateid{get; set;}
       public DateTime updatedate{get; set;}
       public int updateprocess{get; set;}
    }
}
