using SchIntegrationAPI.ApiUtilities;
using SchIntegrationAPI.Models;
using SchIntegrationAPI.Repositories;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace SchIntegrationAPI.Controllers.api
{
    public class FilesController : ApiController
    {
        private Logger Logger { get; } = LogManager.GetLogger("logfileattachment");

        EmailNotificationUtility emailNotificationUtility = null;
        ApiUtility apiUtility;
        AppConfigs appConfigs;
        SchIntegrationRepository accruentRepository;
        public FilesController()
        {
            appConfigs = new AppConfigs();
            emailNotificationUtility = new EmailNotificationUtility(appConfigs.EmailConfigs);
            apiUtility = new ApiUtility(appConfigs, emailNotificationUtility);
            accruentRepository = new SchIntegrationRepository(appConfigs.ConnectionString, emailNotificationUtility);

            Logger.Info("Initialize the file classes");
        }

        //[HttpGet]
        //public IHttpActionResult Get(string table_sys_id, int request_id)
        //{
        //    bool result = false;

        //    try
        //    {
        //        Logger.Info($"Connecting the Shared Drive is!!!!!!!");
        //        //string path = ConfigurationManager.AppSettings["FolderPath"].ToString();
        //        string path = appConfigs.AttachmentLocation;

        //        var response = apiUtility.GetAttachmentRequestBySysId(Request.Method.ToString(), "https://accruentseattleoemdev.service-now.com/api/now/attachment/181791a31bbfec50594e0f6cdc4bcb1a/file");


        //        ////write to localfolder code
        //        //            string localpath = @"C:\ssi_upload\attachments\req1\reqfu1\";

        //        path = path + "reqpayal" + "/reqfupayal/";

        //        if (response != null)
        //        {
        //            if (!Directory.Exists(path))
        //            {
        //                Directory.CreateDirectory(path);
        //            }

        //            File.WriteAllBytes(path + "mrpickle.jpg", response);
        //        }

        //        Logger.Info($"File created at =>{path}");


        //        //bool exists = false;
        //        //DirectoryInfo d = new DirectoryInfo(path);
        //        //FileInfo[] Files = d.GetFiles("*.*", SearchOption.AllDirectories);
        //        //if (Files.Length > 0)                
        //        //{
        //        //    exists = true;
        //        //}
        //        // Logger.Info($"Directory created =>{exists}");


        //        result = true;


        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Info($"Exception is =>{ex.Message}");

        //    }



        //    return Ok(result);

        //}

        [HttpGet]
        public IHttpActionResult Get(string table_sys_id, int request_id)
        {
            // HttpResponseMessage result;
            bool result = false;
            Logger.Info($"Get Request of File Controller=>{Request.RequestUri}");

            try
            {
                if (table_sys_id == null || request_id <= 0)
                {
                    Logger.Info($"table_sys_id is Null=>{table_sys_id} and request_id is => {request_id}");
                    return NotFound();

                }
                else
                {
                    var attachmentjson = apiUtility.GetAttachmentJsonRequestBySysId(Request.Method.ToString(), table_sys_id);

                    if (attachmentjson != null && attachmentjson.Count >= 1)
                    {
                        foreach (var item in attachmentjson)
                        {
                            if(!item.sys_created_by.Contains("svcSystemSource"))
                            {
                                Logger.Info($"Download links is=>{item.download_link} for table_sys_id is=>{item.table_sys_id}");
                                Logger.Info($"Start Downloading the files for table_sys_id is=>{item.table_sys_id} and filename is=>{item.file_name}");

                                var response = apiUtility.GetAttachmentRequestBySysId(Request.Method.ToString(), item.download_link);
                                if (response != null)
                                {
                                    result = accruentRepository.AddAttachmentRequestAsync(item, response, appConfigs.AttachmentLocation, appConfigs.PortalLinkUrl, request_id);
                                }
                                Logger.Info($"Attachment Process Result is =>{result}");
                            }
                           
                        }
                    }
                    else
                    {
                        Logger.Info($"Attachment JSON is not available for these parameters =>table_sys_id is=> {table_sys_id} and request_id is=> {request_id} ");
                    }
                    return Ok(result);


                    //var accruentApisList = apiUtility.GetRequestByAssignGroup(Request.Method.ToString(), assignment_group);
                    //result = accruentRepository.AddRequestAsync(accruentApisList);
                    //logger.Info($"Result is =>{result}");

                    /*write to localfolder code
                    string localpath = @"C:\ssi_upload\attachments\req1\reqfu1\";

                    if (!Directory.Exists(localpath))
                    {
                        Directory.CreateDirectory(localpath);
                    }

                    File.WriteAllBytes(localpath + "mrpickle.jpg", response);
                    */

                    /*
                      //Download file code
                    //result = new HttpResponseMessage(HttpStatusCode.OK);
                    ////// Byte[] bytes = File.ReadAllBytes(response);
                    //result.Content = new ByteArrayContent(response);
                    //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    //result.Content.Headers.ContentDisposition.FileName = "mrpickle.jpg";
                    */

                    //result = new HttpResponseMessage(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                Logger.Info($"Exception Occured =>{ex.Message} for sys id {table_sys_id}");
                var msg = $"Api Crashed, {ex.Message}";
                emailNotificationUtility?.SendEmail(msg);
                //emailNotificationUtility?.SendGmailEmail(msg);

                //return BadRequest(ex.Message);
                return BadRequest(msg);
            }
            //return Ok(true);
        }

        [HttpPost]
        public IHttpActionResult Post(string table_sys_id, int request_id, string filepath)
        {
            bool result = false;
            Logger.Info($"POST Request of File Controller=>{Request.RequestUri}");

            var respone = new HttpResponseMessage();

            try
            {
                if (table_sys_id == null || request_id <= 0)
                {
                    Logger.Info($"POST Request sys_id is Null=>{table_sys_id} and request_id is => {request_id}");
                    return NotFound();

                }
                else
                {
                    result = apiUtility.PostRequestAttachmentBySysId(table_sys_id, request_id, filepath);
                    //result = true;
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Info($"POST Request Exception Occured =>{ex.Message} for sys id {table_sys_id}");
                var msg = $"POST Request Attachmnet Api Crashed, {ex.Message}";
                emailNotificationUtility?.SendEmail(msg);
              
                return BadRequest(msg);
            }

        }
    }
}
