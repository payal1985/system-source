using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportUI.DBModels
{
    public class Product
    {
        [Key]
        public int Microsoft_product_ID { get; set; }
        public string SourceSheet { get; set; }
        public string Item_Number { get; set; }
        public int Category_ID { get; set; }
        public int Subcategory_ID { get; set; }
        public int Lead_Time_ID { get; set; }
        public string Manufacturer { get; set; }
        public string Mfg_Rep { get; set; }
        public string Description { get; set; }
        public string ManFac { get; set; }
        public string Cat { get; set; }
        public string Series { get; set; }
        public string Base_Attribute { get; set; }
        public string Height_attr { get; set; }
        public string Misc { get; set; }
        public string Seat_Actions { get; set; }
        public string Fabric_Detail { get; set; }
        public string Modular { get; set; }
        public string Shape { get; set; }
        public string Seats { get; set; }
        public decimal COM_Price { get; set; }
        public decimal Grade_3_or_C_Price { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public decimal Width { get; set; }
        public decimal Diameter { get; set; }
        public string Designer { get; set; }
        public decimal Yardage { get; set; }
        public decimal Leather_SqFt { get; set; }
        public decimal Leather_SqM { get; set; }
        public decimal CF { get; set; }
        public string Environmental_1 { get; set; }
        public string Environmental_2 { get; set; }
        public string Environmental_3 { get; set; }
        public string Environmental_4 { get; set; }
        public decimal Seat_Height { get; set; }
        public decimal Arm_Height { get; set; }
        public bool active_fl { get; set; }
        public DateTime Load_DateTime { get; set; }
    }
}
