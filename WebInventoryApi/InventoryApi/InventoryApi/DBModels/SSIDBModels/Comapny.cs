using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.DBModels.SSIDBModels
{
    [Table("company")]
    public class Comapny
    {
        public int? company_ID{get;set;}
        public string company_name{get;set;}
        public string? vendor_class{get;set;}
        public string? address_line_1{get;set;}
        public string? address_line_2{get;set;}
        public string? city{get;set;}
        public int? state_province_ID{get;set;}
        public string? postal_code{get;set;}
        public int? country_ID{get;set;}
        public int phone{get;set;}
        public string? FAX{get;set;}
        public string? email{get;set;}
        public string? contact_name{get;set;}
        public string? contact_line_1{get;set;}
        public string? contact_line_2{get;set;}
        public string? contact_line_3{get;set;}
        public string? contact_line_4{get;set;}
    }
}
