using DataEntryUI.DBModels;
using DataEntryUI.Models;
using DataEntryUI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataEntryUI.Controllers
{
    [Route("api/[controller]")]
    //[Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILeadTimeRepository _leadTimeRepository;
        //ProductModel productModel;
        //private IHostingEnvironment environment;
        IConfiguration config;

        public ProductController(IConfiguration _config, IProductRepository productRepository, ICategoryRepository categoryRepository, ISubCategoryRepository subCategoryRepository, ILeadTimeRepository leadTimeRepository)
        {
            config = _config;
            // _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _leadTimeRepository = leadTimeRepository;
            //environment = _environment;
            // productModel = new ProductModel();
            // productModel = new ProductModel(_categoryRepository, _subCategoryRepository);
        }

        // GET: api/<ProductController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //[HttpGet]
        //[Route("getname")]
        //public string GetName()
        //{
        //    return "Payal";
        //}

        //[HttpGet]
        //[Route("getcate")]
        //[Route("GetCategory")]
        [HttpGet("getcategory")]

        public IEnumerable<Category> GetCategory()
        {
            try
            {
                var categories = _categoryRepository.GetCategory().Select(x => new Category
                {
                    Description = x.Description,
                    Category_ID = x.Category_ID
                }).ToList();

                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception occured to load category {ex.Message} \n {ex.StackTrace}");
            }
            // return new string[] { "value1", "value2" };
        }

        [HttpGet("getsubcategory")]
        public IEnumerable<SubCategory> GetSubCategory()
        {
            try
            {
                var subCategories = _subCategoryRepository.GetSubCategory().Select(x => new SubCategory
                {
                    Description = x.Description,
                    Subcategory_ID = x.Subcategory_ID
                }).ToList();

                return subCategories;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception occured to load subcategory {ex.Message} \n {ex.StackTrace}");
            }
            // return new string[] { "value1", "value2" };
        }
        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductController>
        [HttpPost]
        //public void Post([FromBody] string value)
        public async Task<string> Post([FromBody] ProductModel productModel)
        {
            int leadtimeid = _leadTimeRepository.GetLeadTimeId(!string.IsNullOrEmpty(productModel.LeadTime.Trim()) ? productModel.LeadTime.Trim() : "");
            Product product = new Product();
            string result = "";

            try
            {
                if (!string.IsNullOrEmpty(productModel.Manufacturer) && !string.IsNullOrEmpty(productModel.Mfg) && !string.IsNullOrEmpty(productModel.Cat) && !string.IsNullOrEmpty(productModel.Manufactured) 
                    && !string.IsNullOrEmpty(productModel.PartNumber) && !string.IsNullOrEmpty(productModel.PartDescription) && !string.IsNullOrEmpty(productModel.Series) 
                    && !string.IsNullOrEmpty(productModel.Category) && !string.IsNullOrEmpty(productModel.SubCategory) && !string.IsNullOrEmpty(productModel.GRADE3_C_List) && !string.IsNullOrEmpty(productModel.Designer))
                {
                    product.SourceSheet = $"{productModel.Manufacturer.Trim()}$";
                    product.Item_Number = productModel.PartNumber.Trim();
                    product.Category_ID = Convert.ToInt32(productModel.Category);
                    product.Subcategory_ID = Convert.ToInt32(productModel.SubCategory);
                    product.Lead_Time_ID = leadtimeid;
                    product.Manufacturer = productModel.Manufacturer.Trim();
                    product.Mfg_Rep = productModel.Rep.Trim();
                    product.Description = productModel.PartDescription.Trim();
                    product.ManFac = productModel.Mfg.Trim();
                    product.Cat = productModel.Cat.Trim();
                    product.Series = productModel.Series.Trim();
                    product.Base_Attribute = (productModel.BaseAttribute == "Other" ? productModel.BaseAttributeOther.Trim() : productModel.BaseAttribute);
                    product.Height_attr = productModel.HeightDD;
                    product.Misc = productModel.Misc.Trim();
                    product.Seat_Actions = productModel.SeatActions.Trim();
                    product.Fabric_Detail = productModel.FabricDetail.Trim();
                    product.Modular = productModel.Modular.Trim();
                    product.Shape = productModel.Shape.Trim();
                    product.Seats = productModel.Seats.Trim();
                    product.COM_Price = (!string.IsNullOrEmpty(productModel.COM.Trim()) ? decimal.Parse(productModel.COM.Trim()) : decimal.Parse("0"));
                    //product.COM_Price = decimal.Parse(productModel.COM.ToString());
                    product.Grade_3_or_C_Price = (!string.IsNullOrEmpty(productModel.GRADE3_C_List.Trim()) ? decimal.Parse(productModel.GRADE3_C_List.Trim()) : decimal.Parse("0"));
                    product.Height = (!string.IsNullOrEmpty(productModel.Height.Trim()) ? decimal.Parse(productModel.Height.Trim()) : decimal.Parse("0"));
                    product.Depth = (!string.IsNullOrEmpty(productModel.Depth.Trim()) ? decimal.Parse(productModel.Depth.Trim()) : decimal.Parse("0"));
                    product.Width = (!string.IsNullOrEmpty(productModel.Width.Trim()) ? decimal.Parse(productModel.Width.Trim()) : decimal.Parse("0"));
                    product.Diameter = (!string.IsNullOrEmpty(productModel.DIA.Trim()) ? decimal.Parse(productModel.DIA.Trim()) : decimal.Parse("0"));
                    product.Designer = productModel.Designer.Trim();
                    product.Yardage = (!string.IsNullOrEmpty(productModel.Yardage.Trim()) ? decimal.Parse(productModel.Yardage.Trim()) : decimal.Parse("0"));
                    product.Leather_SqFt = (!string.IsNullOrEmpty(productModel.LeatherSqFeet.Trim()) ? decimal.Parse(productModel.LeatherSqFeet.Trim()) : decimal.Parse("0"));
                    product.Leather_SqM = (!string.IsNullOrEmpty(productModel.leatherSqMeters.Trim()) ? decimal.Parse(productModel.leatherSqMeters.Trim()) : decimal.Parse("0"));
                    product.CF = (!string.IsNullOrEmpty(productModel.cf.Trim()) ? decimal.Parse(productModel.cf.Trim()) : decimal.Parse("0"));
                    product.Environmental_1 = productModel.Environmental_1.Trim();
                    product.Environmental_2 = productModel.Environmental_2.Trim();
                    product.Environmental_3 = productModel.Environmental_3.Trim();
                    product.Environmental_4 = productModel.Environmental_4.Trim();
                    product.Seat_Height = (!string.IsNullOrEmpty(productModel.SeatHeight.Trim()) ? decimal.Parse(productModel.SeatHeight.Trim()) : decimal.Parse("0"));
                    product.Arm_Height = (!string.IsNullOrEmpty(productModel.ArmHeight.Trim()) ? decimal.Parse(productModel.ArmHeight.Trim()) : decimal.Parse("0"));
                    product.active_fl = true;
                    product.Load_DateTime = System.DateTime.Now;


                    result = await _productRepository.InsertProduct(product);
                    //result = "success";

                }
                else
                {
                    throw new Exception($"Error in entered data, please correct and resubmit...");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception Occured while processing data for the part number {product.Item_Number} \n Error Message: {ex.Message} \n {ex.StackTrace}");
            }
            return result;
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
