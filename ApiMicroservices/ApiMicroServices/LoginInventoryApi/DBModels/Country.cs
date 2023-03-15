using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginInventoryApi.DBModels
{
    [Table("countries")]
    public class Country
    {
        [Key]
        public int Country_ID { get; set; }

        public string country { get; set; }

        public string country_code { get; set; }

        public string currency { get; set; }

        public bool? active { get; set; }

        public int createid { get; set; }

        public DateTime createdate { get; set; }

        public int? createprocess { get; set; }

        public int updateid { get; set; }

        public DateTime updatedate { get; set; }

        public int? updateprocess { get; set; }
    }
}
