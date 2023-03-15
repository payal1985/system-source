using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.DBModels
{
    [Table("users")]
    public class Users
    {
		[Key]
		public int user_id { get; set; }
		public int user_type_id { get; set; }
		public int? vendor_id { get; set; }
		public int? default_client_id { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public string? user_title { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string email { get; set; }

		public string? mobile { get; set; }
		public string? work_phone { get; set; }
		public string? ariba_id { get; set; }
		public string? company { get; set; }
        public int? company_id { get; set; }
        public string? job_title { get; set; }
		public string? addr1 { get; set; }
		public string? addr2 { get; set; }
		public string? city { get; set; }
		//public string? state { get; set; }
		public int? state_province_ID { get; set; }
		public string? zip { get; set; }

		public int? country_ID { get; set; }
		public bool active { get; set; }
		public int createid { get; set; }
		public DateTime createdate { get; set; }
		public int createprocess { get; set; }
		public int updateid { get; set; }
		public DateTime updatedate { get; set; }
		public int updateprocess { get; set; }
		public string inventory_user_accept_rules_reqd { get; set; }
	}
}
