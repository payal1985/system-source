using GoCanvasAPI.ApiUtilities;
using GoCanvasAPI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoCanvasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        private readonly IConfiguration _config;

        ApiUtility apiUtility;

        public FilesController(IConfiguration config)
        {
            //logger = LogManager.GetCurrentClassLogger();

            //appConfigs = new AppConfigs();
            _config = config;
            apiUtility = new ApiUtility(_config);

            //accruentRepository = new AccruentRepository(appConfigs.ConnectionString, emailNotificationUtility);

            //logger.Info("Initialize the classes");
        }

        [HttpGet]
        //public HttpResponseMessage Get(long image_id, string username, string password, int number=0)
        public async Task<HttpResponseMessage> Get(long image_id, int number=0)
        {
            //bool result = false;
            //Logger.Info($"Get Request of File Controller=>{Request.RequestUri}");

            //HttpResponseMessage

            try
            {
                //if (image_id <= 0  && string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                if (image_id <= 0)
                {
                    //Logger.Info($"table_sys_id is Null=>{table_sys_id} and request_id is => {request_id}");
                   return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
                else
                {
                   // var imageBinary = apiUtility.GetImageByImageId(image_id, username, password,number);
                    var imageBinary = await apiUtility.GetImageByImageId(image_id, number);

                   /*write to localfolder code*/
                    string localpath = @"C:\ssi_upload\attachments\GoCanvas\";

                    if (!Directory.Exists(localpath))
                    {
                        Directory.CreateDirectory(localpath);
                    }
                    
                    System.IO.File.WriteAllBytes(localpath + image_id + "_"+ number +".jpg", imageBinary);

                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                //Logger.Info($"Exception Occured =>{ex.Message} for sys id {table_sys_id}");
                var msg = $"Api Crashed, {ex.Message}";
                //emailNotificationUtility?.SendEmail(msg);
                //emailNotificationUtility?.SendGmailEmail(msg);

                //return BadRequest(ex.Message);
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet("renameimages")]
        public  IActionResult Renameimages(string img_id,string img_name)
        {
            bool result = false;
            try
            {
                result = apiUtility.MoveImageData(img_id, img_name);
                return Ok(result);
            }
            catch(Exception ex)
            {
                //var err = $"Exception : {ex.Message}\n{ex.InnerException}\n{ex.StackTrace}";
                return BadRequest(result);
            }            
        }
    }
}
