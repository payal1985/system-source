using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBModels
{
    [Table("Customers")]

    public class Customers
    {
        [Key]
        public int Customers_ID{get; set;}
       public string CompanyName{get; set;}
        //public string? Address1{get; set;}
        //public string? Address2{get; set;}
        //public string? City{get; set;}
        //public string? State{get; set;}
        //public string? Zip{get; set;}
        //public string? Phone1{get; set;}
        //public string? Phone2{get; set;}
        //public string? Fax{get; set;}
        //public bool Active{get; set;}
        //public DateTime CreateDate{get; set;}
        //public int? Salesperson_ID{get; set;}
        public int? CustID { get; set; }
        //public bool Approved{get; set;}
        //public bool Internal{get; set;}
        //public int? CompanyLocations_ID{get; set;}
        //public bool RequireProposal{get; set;}
        //public decimal? Latitude{get; set;}
        //public decimal? Longitude{get; set;}
        //public DbGeography? Geocode {get; set;}
        //public int? Users_SPMASTER_ID{get; set;}
        //public string? ShortName{get; set;}
        public bool HasInventory{get; set;}
    }
}
