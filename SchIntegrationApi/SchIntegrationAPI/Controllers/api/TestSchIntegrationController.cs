using SchIntegrationAPI.ApiUtilities;
using SchIntegrationAPI.Models;
using SchIntegrationAPI.Repositories;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SchIntegrationAPI.Controllers.api
{    
    public class TestSchIntegrationController : ApiController
    {
        TestApiUtility apiUtility;
        AppConfigs appConfigs;
        Logger logger;
        //EmailNotificationUtility emailNotificationUtility = null;
        private ITestSchIntegrationRepository accruentRepository { get; set; }

        public TestSchIntegrationController()
        {
            //logger = LogManager.GetCurrentClassLogger();
            logger = LogManager.GetLogger("logfile");

            appConfigs = new AppConfigs();
           // emailNotificationUtility = new EmailNotificationUtility(appConfigs.EmailConfigs);
            apiUtility = new TestApiUtility(appConfigs);
            accruentRepository = new TestSchIntegrationRepository(appConfigs.ConnectionString);

            //var virDirPath = AppDomain.CurrentDomain.BaseDirectory  + appConfigs.NlogDirPath;

            //string PathToLogFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, appConfigs.NlogDirPath);

            //logger.Factory.Configuration.Variables["logroot"].Text = PathToLogFolder;

            // LogManager.Configuration.Variables["logDirectory"] = appConfigs.NlogDirPath;
             
            //logger.Factory.Configuration.Variables["logroot"].Text = AppDomain.CurrentDomain.BaseDirectory + appConfigs.NlogDirPath;
            
            //logger.Factory.Configuration.Variables["logroot"].Text = HttpContext.Current.Server.MapPath(appConfigs.NlogDirPath); 
            logger.Info("Initialize the classes");
        }

        [HttpGet]
        [Route("api/testsch/message")]
        public IHttpActionResult GetMessage()
        {
            return Ok("Hello Test World!!!!!!!");
        }

        // GET: api/TestAccruent
        // [Route("requests/{assignment_group}")]
        [HttpGet]
        [Route("api/testsch/requests/{assignment_group}")]
        public IHttpActionResult GetRequests(string assignment_group)
        {
            bool result = false;
            logger.Info($"Get Request=>{Request.RequestUri}");

            try
            {
                if (assignment_group == null)
                {
                    logger.Info($"Request Id is Null=>{assignment_group}");
                    return NotFound();

                }
                else
                {
                    var accruentApisList = apiUtility.GetRequestByAssignGroup(Request.Method.ToString(), assignment_group);
                    logger.Info($"Total Work Order reading from JSON => {accruentApisList.Count}");
                    //result = accruentRepository.AddRequestAsync(accruentApisList);
                    // logger.Info($"Assignment Group Reading and Insertion Result is =>{result}");
                    result = true;

                    return Ok(accruentApisList);
                }
            }
            catch (Exception ex)
            {
                logger.Info($"Exception Occured =>{ex.Message}");
                var msg = $"Api Crashed, {ex.Message}";
                //emailNotificationUtility?.SendEmail(msg);
                //emailNotificationUtility?.SendGmailEmail(msg);

                return BadRequest(msg);
            }

        }

        [HttpGet]
        [Route("api/testsch/mainrequest/{assignment_group}")]
        public IHttpActionResult GetMainRequest(string assignment_group)
        {
            //bool result = false;
            logger.Info($"Get Request=>{Request.RequestUri}");

            try
            {
                if (assignment_group == null)
                {
                    logger.Info($"Request Id is Null=>{assignment_group}");
                    return NotFound();

                }
                else
                {
                    var accruentApisList = apiUtility.GetMainRequestByAssignGroup(Request.Method.ToString(), assignment_group);
                    logger.Info($"Total Work Order reading from JSON => {accruentApisList.Count}");
                    //result = accruentRepository.AddRequestAsync(accruentApisList);
                    // logger.Info($"Assignment Group Reading and Insertion Result is =>{result}");
                    //result = true;

                    return Ok(accruentApisList);
                }
            }
            catch (Exception ex)
            {
                logger.Info($"Exception Occured =>{ex.Message}");
                var msg = $"Api Crashed, {ex.Message}";
                //emailNotificationUtility?.SendEmail(msg);
                //emailNotificationUtility?.SendGmailEmail(msg);

                return BadRequest(msg);
            }
        }

        [HttpGet]
        [Route("api/testsch/description")]
        public IHttpActionResult GetDescription()
        {

           // bool result = false;
            logger.Info($"Get Request=>{Request.RequestUri}");

            try
            {
           
                    var result = accruentRepository.GetDescription();
                    logger.Info($"Result => {result}");
                    //result = accruentRepository.AddRequestAsync(accruentApisList);
                    // logger.Info($"Assignment Group Reading and Insertion Result is =>{result}");
                   // result = true;

                    return Ok(result);
                
            }
            catch (Exception ex)
            {
                logger.Info($"Exception Occured =>{ex.Message}");
                var msg = $"Api Crashed, {ex.Message}";                

                return BadRequest(msg);
            }
        }

        [HttpGet]
        [Route("api/testsch/mainjson/{assignment_group}")]
        public IHttpActionResult GetMainJson(string assignment_group)
        {
            logger.Info($"Get Request=>{Request.RequestUri}");

            try
            {
                if (assignment_group == null)
                {
                    logger.Info($"Request Id is Null=>{assignment_group}");
                    return NotFound();

                }
                else
                {
                    var accruentApisjson = apiUtility.GetMainRequestJsonByAssignGroup(Request.Method.ToString(), assignment_group);
                    //logger.Info($"Total Work Order reading from JSON => {accruentApisList.Count}");
                    //result = accruentRepository.AddRequestAsync(accruentApisList);
                    // logger.Info($"Assignment Group Reading and Insertion Result is =>{result}");
                    //result = true;

                    return Ok(accruentApisjson);
                }
            }
            catch (Exception ex)
            {
                logger.Info($"Exception Occured =>{ex.Message}");
                var msg = $"Api Crashed, {ex.Message}";
                //emailNotificationUtility?.SendEmail(msg);
                //emailNotificationUtility?.SendGmailEmail(msg);

                return BadRequest(msg);
            }
        }




        //[HttpGet]
        //public IHttpActionResult Get(string table_sys_id, int request_id)
        //{
        //    // HttpResponseMessage result;
        //    bool result = false;
        //    logger.Info($"Get Request of File Controller=>{Request.RequestUri}");

        //    try
        //    {
        //        if (table_sys_id == null || request_id <= 0)
        //        {
        //            logger.Info($"table_sys_id is Null=>{table_sys_id} and request_id is => {request_id}");
        //            return NotFound();

        //        }
        //        else
        //        {
        //            var attachmentjson = apiUtility.GetAttachmentJsonRequestBySysId(Request.Method.ToString(), table_sys_id);

        //            if (attachmentjson != null && attachmentjson.Count >= 1)
        //            {
        //                foreach (var item in attachmentjson)
        //                {
        //                    if (!item.sys_created_by.Contains("svcSystemSource"))
        //                    {
        //                        logger.Info($"Download links is=>{item.download_link} for table_sys_id is=>{item.table_sys_id}");
        //                        logger.Info($"Start Downloading the files for table_sys_id is=>{item.table_sys_id} and filename is=>{item.file_name}");

        //                        var response = apiUtility.GetAttachmentRequestBySysId(Request.Method.ToString(), item.download_link);
        //                        if (response != null)
        //                        {
        //                            result = accruentRepository.CreateAttachment(item, response, appConfigs.AttachmentLocation, appConfigs.PortalLinkUrl, request_id);
        //                        }
        //                        logger.Info($"Attachment Process Result is =>{result}");
        //                    }

        //                }
        //            }
        //            else
        //            {
        //                logger.Info($"Attachment JSON is not available for these parameters =>table_sys_id is=> {table_sys_id} and request_id is=> {request_id} ");
        //            }
        //            return Ok(result);


        //            //var accruentApisList = apiUtility.GetRequestByAssignGroup(Request.Method.ToString(), assignment_group);
        //            //result = accruentRepository.AddRequestAsync(accruentApisList);
        //            //logger.Info($"Result is =>{result}");

        //            /*write to localfolder code
        //            string localpath = @"C:\ssi_upload\attachments\req1\reqfu1\";

        //            if (!Directory.Exists(localpath))
        //            {
        //                Directory.CreateDirectory(localpath);
        //            }

        //            File.WriteAllBytes(localpath + "mrpickle.jpg", response);
        //            */

        //            /*
        //              //Download file code
        //            //result = new HttpResponseMessage(HttpStatusCode.OK);
        //            ////// Byte[] bytes = File.ReadAllBytes(response);
        //            //result.Content = new ByteArrayContent(response);
        //            //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //            //result.Content.Headers.ContentDisposition.FileName = "mrpickle.jpg";
        //            */

        //            //result = new HttpResponseMessage(HttpStatusCode.OK);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Info($"Exception Occured =>{ex.Message} for sys id {table_sys_id}");
        //        var msg = $"Api Crashed, {ex.Message}";
        //        //emailNotificationUtility?.SendEmail(msg);
        //        //emailNotificationUtility?.SendGmailEmail(msg);

        //        //return BadRequest(ex.Message);
        //        return BadRequest(msg);
        //    }
        //    //return Ok(true);
        //}

        // POST: api/TestAccruent
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/TestAccruent/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TestAccruent/5
        public void Delete(int id)
        {
        }
    }
}
