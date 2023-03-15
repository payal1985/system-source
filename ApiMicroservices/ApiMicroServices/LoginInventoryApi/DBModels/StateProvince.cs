using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginInventoryApi.DBModels
{
    [Table("state_province")]
    public class StateProvince
    {
        [Key]
        public int State_Province_ID { get; set; }

        public string state_province { get; set; }

        public string state_province_code { get; set; }

        public int country_ID { get; set; }

        public bool? active { get; set; }

        public int createid { get; set; }

        public DateTime createdate { get; set; }

        public int? createprocess { get; set; }

        public int updateid { get; set; }

        public DateTime updatedate { get; set; }

        public int? updateprocess { get; set; }
    }
}
