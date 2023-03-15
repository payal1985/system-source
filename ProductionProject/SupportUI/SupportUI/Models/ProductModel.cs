using Microsoft.AspNetCore.Mvc.Rendering;
using SupportUI.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportUI.Models
{
    public class ProductModel
    {
        [Required]
        [DisplayName("Manufacturer")]
        public string Manufacturer      {get;set;}

        [DisplayName("Rep")]
        public string Rep               {get;set;}

        [Required]
        [DisplayName("Mfg")]
        public string Mfg               {get;set;}

        [Required]
        [DisplayName("Cat")]
        public string Cat               {get;set;}

        [Required]
        [DisplayName("Manufactured Location")]
        public string Manufactured      {get;set;}

        [Required]
        [DisplayName("Part Number")]
        public string PartNumber       {get;set;}

        [DisplayName("Components")]
        public string Components        {get;set;}

        [Required]
        [DisplayName("Part Description")]
        public string PartDescription  {get;set;}

        [Required]
        [DisplayName("Series")]
        public string Series            {get;set;}

        [Required]
        [DisplayName("Category")]
        public string Category          {get;set;}

        public List<SelectListItem> Categories { get; private set; }


        [Required]
        [DisplayName("Sub Category")]
        public string SubCategory      {get;set;}

        public List<SelectListItem> SubCategories { get; private set; }
        

        [DisplayName("Height")]
        public string HeightDD      {get; set;}
        public List<SelectListItem> Heights      {get;private set;}

        [DisplayName("Misc")]
        public string Misc              {get;set;}

        [DisplayName("Seat Actions")]
        public string SeatActions      {get;set;}

        [DisplayName("Fabric Detail")]
        public string FabricDetail     {get;set;}

        [DisplayName("Modular")]
        public string Modular           {get;set;}

        [DisplayName("Shape")]
        public string Shape             {get;set;}

        [DisplayName("Seats")]
        public string Seats             {get;set;}

        public List<SelectListItem> BaseAttributes { get; private set; }

        [DisplayName("Base Attribute")]
        public string BaseAttribute { get; set; }


        public string BaseAttributeOther    {get;set; }

        [DisplayName("COM")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Enter Valid Price")]
        //[DataType(DataType.Currency)]
        //[DisplayFormat(DataFormatString = "{0:C0}")] 
        public string  COM               {get;set;}

        [Required]
        [DisplayName("GRADE 3/C/List")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Enter Valid Price")]
        public string GRADE3_C_List    {get;set;}

        [DisplayName("Lead Time")]
        public string LeadTime         {get;set;}

        [DisplayName("H")]
        //[Column(TypeName = "decimal(7,4)")]
        //[RegularExpression(@"^\d+\.\d{0,4}$", ErrorMessage = "Valid Decimal number")]
        [RegularExpression(@"\d{0,7}(\.\d{0,4})?", ErrorMessage = "Enter Valid Number")]
        //^[0-9] ([.,][0 - 9]{1,3})?$
        public string Height                 {get;set;}

        [DisplayName("D")]
        [RegularExpression(@"\d{0,7}(\.\d{0,4})?", ErrorMessage = "Enter Valid Number")]
        public string Depth                 {get;set;}

        [DisplayName("W")]
        [RegularExpression(@"\d{0,7}(\.\d{0,4})?", ErrorMessage = "Enter Valid Number")]
        public string Width                 {get;set;}

        [DisplayName("SH (Seat Height)")]
        [RegularExpression(@"\d{0,7}(\.\d{0,4})?", ErrorMessage = "Enter Valid Number")]
        public string SeatHeight   {get;set;}

        [DisplayName("AH (Arm Height)")]
        [RegularExpression(@"\d{0,7}(\.\d{0,4})?", ErrorMessage = "Enter Valid Number")]
        public string ArmHeight    {get;set;}

        [DisplayName("DIA")]
        [RegularExpression(@"\d{0,7}(\.\d{0,4})?", ErrorMessage = "Enter Valid Number")]
        public string DIA               {get;set;}

        [Required]
        [DisplayName("Designer")]
        public string Designer          {get;set;}

        [DisplayName("Yardage")]
        [RegularExpression(@"\d{0,7}(\.\d{0,4})?", ErrorMessage = "Enter Valid Number")]
        public string Yardage           {get;set;}

        [DisplayName("Leather SqFeet")]
        [RegularExpression(@"\d{0,7}(\.\d{0,4})?", ErrorMessage = "Enter Valid Number")]
        public string LeatherSqFeet    {get;set;}

        [DisplayName("Leather SqMeters")]
        [RegularExpression(@"\d{0,7}(\.\d{0,4})?", ErrorMessage = "Enter Valid Number")]
        public string leatherSqMeters  {get;set;}

        [DisplayName("cf")]
        [RegularExpression(@"\d{0,7}(\.\d{0,4})?", ErrorMessage = "Enter Valid Number")]
        public string cf                {get;set;}

        [DisplayName("Environmental 1")]
        public string Environmental_1	{get;set;}

        [DisplayName("Environmental 2")]
        public string Environmental_2	{get;set;}

        [DisplayName("Environmental 3")]
        public string Environmental_3	{get;set;}

        [DisplayName("Environmental 4")]
        public string Environmental_4 { get; set; }

        //public List<SelectListItem> LeadTimeItems { get; private set; }

        public ProductModel()
        { }

        public ProductModel(ICategoryRepository categoryRepository, ISubCategoryRepository subCategoryRepository)
        {
            Heights = new List<SelectListItem>
                                {
                                    new SelectListItem("Bar Height", "Bar Height"),
                                    new SelectListItem("Counter Height", "Counter Height"),
                                    new SelectListItem("Highback", "Highback"),
                                    new SelectListItem("Low", "Low"),
                                    new SelectListItem("Midback", "Midback")
                                }; // used to populate the list of options

            BaseAttributes = new List<SelectListItem>
            {
                new SelectListItem("4-Star","4-Star"),
                new SelectListItem("5-Star","5-Star"),
                new SelectListItem("Cantilever","Cantilever"),
                new SelectListItem("Disc Base","Disc Base"),
                new SelectListItem("Metal Legs","Metal Legs"),
                new SelectListItem("Other","Other"),
                new SelectListItem("Plinth","Plinth"),
                new SelectListItem("Rocker","Rocker"),
                new SelectListItem("Sled Base","Sled Base"),
                new SelectListItem("Upholstered","Upholstered"),
                new SelectListItem("Wire","Wire"),
                new SelectListItem("Wood","Wood"),
                new SelectListItem("Wood Legs","Wood Legs")
            };

            //var category = categoryRepository.GetCategory();     

            Categories = categoryRepository.GetCategory().Select(x => new SelectListItem {
             Text  = x.Description,Value =  x.Category_ID.ToString()
            }).ToList();

            SubCategories = subCategoryRepository.GetSubCategory().Select(x => new SelectListItem
            {
                Text = x.Description, Value = x.Subcategory_ID.ToString()
            }).ToList();

            //LeadTimeItems = leadTimeRepository.GetLeadTime().Select(x => new SelectListItem
            //{
            //    Text = x.lead_time_description,
            //    Value = x.lead_time_ID.ToString()
            //}).ToList();
        }
    }

    #region old code
    //public enum Height
    //{
    //    [Display(Name = "Bar Height")]
    //    BarHeight,
    //    [Display(Name = "Counter Height")]
    //    CounterHeight,
    //    [Display(Name = "Highback")]
    //    Highback,
    //    [Display(Name = "Low")]
    //    Low,
    //    [Display(Name = "Midback")]
    //    Midback
    //}

    //public enum BaseAttribute
    //{
    //    [Display(Name = "4-Star")]
    //    Star4,
    //    [Display(Name = "5-Star")]
    //    Star5,
    //    [Display(Name = "Cantilever")]
    //    Cantilever,
    //    [Display(Name = "Disc Base")]
    //    DiscBase,
    //    [Display(Name = "Metal Legs")]
    //    MetalLegs,
    //    [Display(Name = "Other")]
    //    Other,
    //    [Display(Name = "Plinth")]
    //    Plinth,
    //    [Display(Name = "Rocker")]
    //    Rocker,
    //    [Display(Name = "Sled Base")]
    //    SledBase,
    //    [Display(Name = "Upholstered")]
    //    Upholstered,
    //    [Display(Name = "Wire")]
    //    Wire,
    //    [Display(Name = "Wood")]
    //    Wood,
    //    [Display(Name = "Wood Legs")]
    //    WoodLegs,

    //}
    #endregion
}
