using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SupportUI.DBModels;
using SupportUI.Models;
using SupportUI.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SupportUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILeadTimeRepository _leadTimeRepository;
        ProductModel productModel;
        //private IHostingEnvironment environment;
        IConfiguration config;

        public HomeController(ILogger<HomeController> logger, IConfiguration _config, ICategoryRepository categoryRepository, ISubCategoryRepository subCategoryRepository, IProductRepository productRepository, ILeadTimeRepository leadTimeRepository)
        {
            config = _config;
            _logger = logger;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _productRepository = productRepository;
            _leadTimeRepository = leadTimeRepository;
            //environment = _environment;
            productModel = new ProductModel();
           // productModel = new ProductModel(_categoryRepository, _subCategoryRepository);
        }

        public IActionResult Index(string? msg)
        {
            if(msg != null)
                ViewBag.Message = msg;

            ProductModel productModel = new ProductModel(_categoryRepository, _subCategoryRepository);
            return View(productModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProductModel productModel)
        //public IActionResult Index(ProductModel productModel)
        {
            int leadtimeid = _leadTimeRepository.GetLeadTimeId(!string.IsNullOrEmpty(productModel.LeadTime) ? productModel.LeadTime : "");
            Product product = new Product();
            string result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    product.SourceSheet = $"{productModel.Manufacturer}$";
                    product.Item_Number = productModel.PartNumber;
                    product.Category_ID = Convert.ToInt32(productModel.Category);
                    product.Subcategory_ID = Convert.ToInt32(productModel.SubCategory);
                    product.Lead_Time_ID = leadtimeid;
                    product.Manufacturer = productModel.Manufacturer;
                    product.Mfg_Rep = productModel.Rep;
                    product.Description = productModel.PartDescription;
                    product.ManFac = productModel.Mfg;
                    product.Cat = productModel.Cat;
                    product.Series = productModel.Series;
                    product.Base_Attribute = (productModel.BaseAttribute == "Other" ? productModel.BaseAttributeOther : productModel.BaseAttribute);
                    product.Height_attr = productModel.HeightDD;
                    product.Misc = productModel.Misc;
                    product.Seat_Actions = productModel.SeatActions;
                    product.Fabric_Detail = productModel.FabricDetail;
                    product.Modular = productModel.Modular;
                    product.Shape = productModel.Shape;
                    product.Seats = productModel.Seats;
                    product.COM_Price = (!string.IsNullOrEmpty(productModel.COM) ? decimal.Parse(productModel.COM) : decimal.Parse("0"));
                    //product.COM_Price = decimal.Parse(productModel.COM.ToString());
                    product.Grade_3_or_C_Price = (!string.IsNullOrEmpty(productModel.GRADE3_C_List) ? decimal.Parse(productModel.GRADE3_C_List) : decimal.Parse("0"));
                    product.Height = (!string.IsNullOrEmpty(productModel.Height) ? decimal.Parse(productModel.Height) : decimal.Parse("0"));
                    product.Depth = (!string.IsNullOrEmpty(productModel.Depth) ? decimal.Parse(productModel.Depth) : decimal.Parse("0"));
                    product.Width = (!string.IsNullOrEmpty(productModel.Width) ? decimal.Parse(productModel.Width) : decimal.Parse("0"));
                    product.Diameter = (!string.IsNullOrEmpty(productModel.DIA) ? decimal.Parse(productModel.DIA) : decimal.Parse("0"));
                    product.Designer = productModel.Designer;
                    product.Yardage = (!string.IsNullOrEmpty(productModel.Yardage) ? decimal.Parse(productModel.Yardage) : decimal.Parse("0"));
                    product.Leather_SqFt = (!string.IsNullOrEmpty(productModel.LeatherSqFeet) ? decimal.Parse(productModel.LeatherSqFeet) : decimal.Parse("0"));
                    product.Leather_SqM = (!string.IsNullOrEmpty(productModel.leatherSqMeters) ? decimal.Parse(productModel.leatherSqMeters) : decimal.Parse("0"));
                    product.CF = (!string.IsNullOrEmpty(productModel.cf) ? decimal.Parse(productModel.cf) : decimal.Parse("0"));
                    product.Environmental_1 = productModel.Environmental_1;
                    product.Environmental_2 = productModel.Environmental_2;
                    product.Environmental_3 = productModel.Environmental_3;
                    product.Environmental_4 = productModel.Environmental_4;
                    product.Seat_Height = (!string.IsNullOrEmpty(productModel.SeatHeight) ? decimal.Parse(productModel.SeatHeight) : decimal.Parse("0"));
                    product.Arm_Height = (!string.IsNullOrEmpty(productModel.ArmHeight) ? decimal.Parse(productModel.ArmHeight) : decimal.Parse("0"));
                    product.active_fl = true;
                    product.Load_DateTime = System.DateTime.Now;


                    result = await _productRepository.InsertProduct(product);
                    //result = true;

                    //ViewBag.Message = result;
                }
                else
                {
                    result = $"Error in entered data, please correct and resubmit...";
                }
            }
            catch (Exception ex)
            {
                result = $"Exception Occured while processing data for the part number {product.Item_Number} \n Error Message: {ex.Message} \n {ex.StackTrace}";
            }
            //ViewBag.Message = result;
            //ModelState.Clear();
            //if(result)
            // productModel = new ProductModel(_categoryRepository, _subCategoryRepository);
            return RedirectToAction("Index",new { msg = result });
           // return RedirectToRoute("default",new { msg = result.ToString() });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult ImageUpload()
        {
            //ImageModel imageModel = new ImageModel();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImageUpload(IFormFile collection)
        {            
            string result = "";
            try
            {
                string path = string.Format(config.GetValue<string>("AttachmentLocation"));
                string fileName = "";
                int imgCounter2D = 0;
                int imgCounter3D = 0;

                //var partNumber = collection["partnumber"].ToString();
                var partNumber = "123";

                var subFolder = "";
                var files = collection;

               // if (Request.Form.Files.Count > 0)
                if (1>0)
                {
                    //IFormFileCollection formFile = (IFormFileCollection)collection.Files.OrderBy(f => f.Name);
                   // IFormFileCollection formFile = collection.Files;

                    //for (int i = 0; i < formFile.Count; i++)
                    //{
                    //    //var ext = Path.GetExtension(formFile[i].FileName);

                    //    //if (formFile[i].Name == "imagefile")
                    //    //{
                    //    //    subFolder = formFile[i].Name + "//";
                    //    //    if (i == 0)
                    //    //        fileName = partNumber + ext;
                    //    //    else
                    //    //        fileName = partNumber + "_" + i + ext;
                    //    //}
                    //    //else if (formFile[i].Name == "2dimagefile")
                    //    //{
                    //    //    subFolder = formFile[i].Name + "//";

                    //    //    if (imgCounter2D == 0)
                    //    //        fileName = partNumber + "_" + "2D" + ext;
                    //    //    else
                    //    //        fileName = partNumber + "_" + "2D_" + imgCounter2D + ext;

                    //    //    imgCounter2D++;
                    //    //}
                    //    //else if (formFile[i].Name == "3dimagefile")
                    //    //{
                    //    //    subFolder = formFile[i].Name + "//";

                    //    //    if (imgCounter3D == 0)
                    //    //        fileName = partNumber + "_" + "3D" + ext;
                    //    //    else
                    //    //        fileName = partNumber + "_" + "3D_" + imgCounter3D + ext;

                    //    //    imgCounter3D++;
                    //    //}

                    //    if (!Directory.Exists(path + subFolder))
                    //    {
                    //        Directory.CreateDirectory(path + subFolder);
                    //    }

                    //    using (var fileStream = new FileStream(Path.Combine(path, subFolder, fileName), FileMode.Create))
                    //    {
                    //        await formFile[i].CopyToAsync(fileStream);
                    //    }

                    //}
                   
                }

                result = $"Images are saved successfully for the part number { partNumber }";               
            }
            catch(Exception ex)
            {
                result = $"Exception Occured due to uploading images {ex.Message} \n {ex.StackTrace}";
            }

            //TempData["ImageModelMessage"] = result;
            //return RedirectToAction("ImageUpload", new { msg = result });
            //return View();

            return Json(new { status = "success", message = result });
        }
    }
}
