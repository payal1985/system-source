using DataEntryUI.DBModels;
using DataEntryUI.Helper;
using DataEntryUI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataEntryUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IProductImagesRepository _productImagesRepository;
        private readonly IProductRepository _productRepository;
        IConfiguration config;

        public ImagesController(IConfiguration _config, IProductRepository productRepository,IProductImagesRepository productImagesRepository)
        {
            config = _config;
            _productRepository = productRepository;
            _productImagesRepository = productImagesRepository;
        }

        // GET: api/<ImagesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ImagesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ImagesController>
        [HttpPost]
        //public void Post([FromBody] string value)
        public async Task<string> Post(IFormCollection collection)
        {
            string result = "";
            try
            {
                string path = string.Format(config.GetValue<string>("AttachmentLocation"));
                string fileName = "";

                var partNumber = collection["partnumber"].ToString();
                var subFolder = "";
                int imgCounter2D = 0;
                int imgCounter3D = 0;
                int imgCounter3Dstudiomax = 0;
                int imgCounterRevit = 0;
                int imgCounterSketchup = 0;
                int imgCounterDetailViews = 0;
                int imgCounterSceneImages = 0;
                int imgCounterAdditionalViews = 0;


                if (Request.Form.Files.Count > 0 && !string.IsNullOrEmpty(partNumber))                
                {
                    IFormFileCollection formFile = collection.Files;

                    for (int i = 0; i < formFile.Count; i++)
                    {
                        var ext = Path.GetExtension(formFile[i].FileName);

                        ////fileName = (i > 0) ? partNumber + "_" + i + ext : partNumber + ext;
                       
                        #region main location storage code
                        //if (formFile[i].Name == "imagefile")
                        //{
                        //    if (i == 0)
                        //        fileName = partNumber + ext;

                        //    ProductImages productImages = new ProductImages();

                        //    productImages.Microsoft_product_ID = _productRepository.GetProductId(partNumber);
                        //    var manufacturer = _productRepository.GetProductManufacturer(partNumber);
                        //    // https://dev.systemsource.com/ms/images/ancillary/Viccarbe/AC03B.JPG
                        //    productImages.URL = "https://dev.systemsource.com/ms/images/ancillary/" + manufacturer + "//" + fileName;
                        //    productImages.Filename = fileName;
                        //    productImages.active_fl = true;
                        //    productImages.Load_DateTime = System.DateTime.Now;

                        //    result = await _productImagesRepository.InsertProductImages(productImages);
                        //}
                        //else
                        //{
                        //    if (i > 0)
                        //        fileName = partNumber + "_" + i + ext;
                        //}

                        //if (!Directory.Exists(path))
                        //{
                        //    Directory.CreateDirectory(path);
                        //}

                        //using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        //{
                        //    await formFile[i].CopyToAsync(fileStream);
                        //}
                        #endregion

                        if (formFile[i].Name == ImageFileHelper.ImageType.primaryimagefile.ToString())
                        {
                           // subFolder = formFile[i].Name + "//";
                            if (i == 0)
                                fileName = partNumber + ext;

                            ProductImages productImages = new ProductImages();

                            productImages.Microsoft_product_ID = _productRepository.GetProductId(partNumber);
                            var manufacturer = _productRepository.GetProductManufacturer(partNumber);
                            // https://dev.systemsource.com/ms/images/ancillary/Viccarbe/AC03B.JPG
                            productImages.URL = string.Format(config.GetValue<string>("ProductImageUrlms")) + manufacturer + "/" + fileName;
                            productImages.Filename = fileName;
                            productImages.active_fl = true;
                            productImages.Load_DateTime = System.DateTime.Now;

                            result = await _productImagesRepository.InsertProductImages(productImages);

                        }
                        else if (formFile[i].Name == ImageFileHelper.ImageType.detailviewsfile.ToString())
                        {
                            subFolder = formFile[i].Name + "//";

                            if (imgCounterDetailViews == 0)
                                fileName = partNumber + ext;
                            else
                                fileName = partNumber + "_" + imgCounterDetailViews + ext;

                            imgCounterDetailViews++;
                        }
                        else if (formFile[i].Name == ImageFileHelper.ImageType.sceneimagesfile.ToString())
                        {
                            subFolder = formFile[i].Name + "//";

                            if (imgCounterSceneImages == 0)
                                fileName = partNumber + ext;
                            else
                                fileName = partNumber + "_" + imgCounterSceneImages + ext;

                            imgCounterSceneImages++;
                        }
                        else if (formFile[i].Name == ImageFileHelper.ImageType.additionalviewsfile.ToString())
                        {
                            subFolder = formFile[i].Name + "//";

                            if (imgCounterAdditionalViews == 0)
                                fileName = partNumber + ext;
                            else
                                fileName = partNumber + "_" + imgCounterAdditionalViews + ext;

                            imgCounterAdditionalViews++;
                        }
                        else if (formFile[i].Name == ImageFileHelper.ImageType.imagefile2d.ToString())
                        {
                            subFolder = formFile[i].Name + "//";

                            if (imgCounter2D == 0)
                                fileName = partNumber + ext;
                            else
                                fileName = partNumber + "_" + imgCounter2D + ext;

                            imgCounter2D++;
                        }
                        else if (formFile[i].Name == ImageFileHelper.ImageType.imagefile3d.ToString())
                        {
                            subFolder = formFile[i].Name + "//";

                            if (imgCounter3D == 0)
                                fileName = partNumber + ext;
                            else
                                fileName = partNumber + "_" + imgCounter3D + ext;

                            imgCounter3D++;
                        }
                        else if (formFile[i].Name == ImageFileHelper.ImageType.studiomax3d.ToString())
                        {
                            subFolder = formFile[i].Name + "//";

                            if (imgCounter3Dstudiomax == 0)
                                fileName = partNumber + ext;
                            else
                                fileName = partNumber + "_" + imgCounter3Dstudiomax + ext;

                            imgCounter3Dstudiomax++;
                        }
                        else if (formFile[i].Name == ImageFileHelper.ImageType.revit.ToString())
                        {
                            subFolder = formFile[i].Name + "//";

                            if (imgCounterRevit == 0)
                                fileName = partNumber + ext;
                            else
                                fileName = partNumber + "_" + imgCounterRevit + ext;

                            imgCounterRevit++;
                        }
                        else if (formFile[i].Name == ImageFileHelper.ImageType.sketchup.ToString())
                        {
                            subFolder = formFile[i].Name + "//";

                            if (imgCounterSketchup == 0)
                                fileName = partNumber + ext;
                            else
                                fileName = partNumber + "_" + imgCounterSketchup + ext;

                            imgCounterSketchup++;
                        }

                        if (string.IsNullOrEmpty(subFolder))
                        {
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                            {
                                await formFile[i].CopyToAsync(fileStream);
                            }
                        }
                        else
                        {
                            if (!Directory.Exists(path + subFolder))
                            {
                                Directory.CreateDirectory(path + subFolder);
                            }

                            using (var fileStream = new FileStream(Path.Combine(path, subFolder, fileName), FileMode.Create))
                            {
                                await formFile[i].CopyToAsync(fileStream);
                            }
                        }
                        
                    }

                    result = result + $"\n Images are saved successfully for the part number { partNumber } , location is : {path}";

                }
                else
                {
                    result = $"{Request.Form.Files.Count} files are attached \n Part Number is : {partNumber} ";
                }


            }
            catch (Exception ex)
            {
                throw new Exception($"Exception Occured due to uploading images {ex.Message} \n {ex.StackTrace}");                

            }

          
            return result;

        }

        // PUT api/<ImagesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ImagesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
