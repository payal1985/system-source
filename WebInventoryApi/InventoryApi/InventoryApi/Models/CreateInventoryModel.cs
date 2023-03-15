using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Models
{
    public class CreateInventoryModel
    {
        public int ClientId {get;set;}
        public string ItemCode { get; set; }
        public int Building { get; set; }
        public int  Floor { get; set; }
        public string Room { get; set; }
        public int ConditionId { get; set; }
        public int ItemTypeId { get; set; }
        public int Qty { get; set; }
        public string Description { get; set; }
        public int ManufacturerID { get; set; } = 0;
        public string ManufacturerName { get; set; }
        public string Fabric { get; set; }
       // public string Fabric2 { get; set; }
        public string Finish { get; set; }
       // public string Finish2 { get; set; }
        public string PartNumber { get; set; }
        public decimal Height { get; set; } = 0;
        public decimal Width { get; set; } = 0;
        public decimal Depth { get; set; } = 0;
        public decimal Diameter { get; set; } = 0;
        public string Top { get; set; }
        public string Edge { get; set; }
        public string Base { get; set; }
        public string Frame { get; set; }
        public string Seat { get; set; }
        public string Back { get; set; }
        public decimal? SeatHeight { get; set; }
        public string Modular { get; set; }
        //public string MainImage {get;set;}
        public string ImageName { get; set; }   
        public string? Tag { get; set; }     
        public string Unit { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int CreateID { get; set; }
        public string ProposalNumber { get; set; }
        public string PoOrderNo { get; set; }
    }
}
