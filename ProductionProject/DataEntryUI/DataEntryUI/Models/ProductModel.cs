using Microsoft.AspNetCore.Mvc.Rendering;
using DataEntryUI.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataEntryUI.Models
{
    public class ProductModel
    {
        public string Manufacturer      {get;set;}

        public string Rep               {get;set;}

        public string Mfg               {get;set;}

        public string Cat               {get;set;}

        public string Manufactured      {get;set;}

        public string PartNumber       {get;set;}

        public string Components        {get;set;}

        public string PartDescription  {get;set;}

        public string Series            {get;set;}

        public string Category          {get;set;}

        //public List<SelectListItem> Categories { get; private set; }


        public string SubCategory      {get;set;}

        //public List<SelectListItem> SubCategories { get; private set; }
        

        public string HeightDD      {get; set;}
        //public List<SelectListItem> Heights      {get;private set;}

        public string Misc              {get;set;}

        public string SeatActions      {get;set;}

        public string FabricDetail     {get;set;}

        public string Modular           {get;set;}

        public string Shape             {get;set;}

        public string Seats             {get;set;}

        //public List<SelectListItem> BaseAttributes { get; private set; }

        public string BaseAttribute { get; set; }


        public string BaseAttributeOther    {get;set; }

        public string  COM               {get;set;}

        public string GRADE3_C_List    {get;set;}

        public string LeadTime         {get;set;}

        public string Height                 {get;set;}

        public string Depth                 {get;set;}

        public string Width                 {get;set;}

        public string SeatHeight   {get;set;}

        public string ArmHeight    {get;set;}

        public string DIA               {get;set;}

        public string Designer          {get;set;}

        public string Yardage           {get;set;}

        public string LeatherSqFeet    {get;set;}

        public string leatherSqMeters  {get;set;}

        public string cf                {get;set;}

        public string Environmental_1	{get;set;}

        public string Environmental_2	{get;set;}

        public string Environmental_3	{get;set;}

        public string Environmental_4 { get; set; }

        //public List<SelectListItem> LeadTimeItems { get; private set; }

        public ProductModel()
        { }

        //public ProductModel(ICategoryRepository categoryRepository, ISubCategoryRepository subCategoryRepository)
        //{
        //    Heights = new List<SelectListItem>
        //                        {
        //                            new SelectListItem("Bar Height", "Bar Height"),
        //                            new SelectListItem("Counter Height", "Counter Height"),
        //                            new SelectListItem("Highback", "Highback"),
        //                            new SelectListItem("Low", "Low"),
        //                            new SelectListItem("Midback", "Midback")
        //                        }; // used to populate the list of options

        //    BaseAttributes = new List<SelectListItem>
        //    {
        //        new SelectListItem("4-Star","4-Star"),
        //        new SelectListItem("5-Star","5-Star"),
        //        new SelectListItem("Cantilever","Cantilever"),
        //        new SelectListItem("Disc Base","Disc Base"),
        //        new SelectListItem("Metal Legs","Metal Legs"),
        //        new SelectListItem("Other","Other"),
        //        new SelectListItem("Plinth","Plinth"),
        //        new SelectListItem("Rocker","Rocker"),
        //        new SelectListItem("Sled Base","Sled Base"),
        //        new SelectListItem("Upholstered","Upholstered"),
        //        new SelectListItem("Wire","Wire"),
        //        new SelectListItem("Wood","Wood"),
        //        new SelectListItem("Wood Legs","Wood Legs")
        //    };

        //    //var category = categoryRepository.GetCategory();     

        //    Categories = categoryRepository.GetCategory().Select(x => new SelectListItem {
        //     Text  = x.Description,Value =  x.Category_ID.ToString()
        //    }).ToList();

        //    SubCategories = subCategoryRepository.GetSubCategory().Select(x => new SelectListItem
        //    {
        //        Text = x.Description, Value = x.Subcategory_ID.ToString()
        //    }).ToList();

        //    //LeadTimeItems = leadTimeRepository.GetLeadTime().Select(x => new SelectListItem
        //    //{
        //    //    Text = x.lead_time_description,
        //    //    Value = x.lead_time_ID.ToString()
        //    //}).ToList();
        //}
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
