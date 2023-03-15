using SchIntegrationAPI.Models;
using SchIntegrationAPI.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace SchIntegrationAPI.ApiUtilities
{
    public class ApiUtility
    {
        private AppConfigs _appConfigs;
        private Logger Logger { get; } = LogManager.GetCurrentClassLogger();
        private Logger LoggerAttachment { get; } = LogManager.GetLogger("logfileattachment");

        private EmailNotificationUtility _emailNotificationUtility = null;

        public ApiUtility(AppConfigs appConfigs, EmailNotificationUtility emailNotificationUtility)
        {
            _appConfigs = appConfigs;
            _emailNotificationUtility = emailNotificationUtility;

            // logger = LogManager.GetCurrentClassLogger();
        }

        public HttpResponseMessage ApiCallMethod(string webApi, string requestType = null)
        {
            //var clientBaseAddress = ConfigurationManager.AppSettings["ClientBaseAddress"].ToString();
            var clientBaseAddress = _appConfigs.BaseAddressUrl;
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(clientBaseAddress);
                    //var authKey = ConfigurationManager.AppSettings["AuthenticationKey"];
                    var authKey = _appConfigs.AuthenticationKey;

                    var byteArray = Encoding.ASCII.GetBytes(authKey);
                    var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    client.DefaultRequestHeaders.Authorization = header;
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (requestType == "GET")
                    {
                        response = client.GetAsync(webApi).Result ?? response;
                    }
                }
            }
            catch(Exception ex)
            {
                var exMsg = $"Error Message due ApiCallMethod to connect service now url =>{ex.Message}";
                Logger.Info(exMsg);
                //_emailNotificationUtility.SendEmail(exMsg);
                //throw;
            }

            return response;
        }

        public JObject ApiCallForChildCollection(string webapiLink)
        {
            JObject jObject = new JObject();

            try
            {
                HttpResponseMessage response = ApiCallMethod(webapiLink, "GET") ?? new HttpResponseMessage();
               // HttpResponseMessage response = new HttpResponseMessage() ?? ApiCallMethod(webapiLink, "GET");

                Logger.Info($"response get from ApiCallMethod from ApiCallForChildCollection=>{response.StatusCode},{response.IsSuccessStatusCode},{response.Content}");

                if (response.Content !=null && response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    Logger.Info($"Get Child request Json=>{json}");
                    if(!string.IsNullOrEmpty(json))
                        jObject = JObject.Parse(json);
                }
                else
                {
                    jObject = null;
                }
            }
            catch(Exception ex)
            {
                var exMsg = $"Error Message due ApiCallForChildCollection method to connect service now url =>{ex.Message} \n for webapilink is=>{webapiLink}";
                Logger.Info(exMsg);
                _emailNotificationUtility.SendEmail(exMsg);
            }

            return jObject;
        }


        public List<SchApi> GetRequestByAssignGroup(string requestType, string assignment_group)
        {
            List<SchApi> accruentApisList = new List<SchApi>();

            try
            {
                if (assignment_group != null)
                {
                    //string webapiurl = ConfigurationManager.AppSettings["GetRequests"] + requestId;

                    //if (state == 0) state = 3;
                    string webapiurl = _appConfigs.WebApiUrl.GetRequests + assignment_group;
                    //string webapiurl = "";
                    //if (state == 3)
                    //    webapiurl = _appConfigs.WebApiUrl.GetRequests + requestId + "&state!=" + state;
                    //else
                    //    webapiurl = _appConfigs.WebApiUrl.GetRequests + requestId + "&state=" + state;


                    HttpResponseMessage response = ApiCallMethod(webapiurl, requestType);

                    response.EnsureSuccessStatusCode();

                    if (response.Content != null && response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;
                        json = json.Contains("\n") ? json.Replace("\n", "").ToString() : json;
                        Logger.Info($"Get Request Json=>{json}" );
                        
                        JObject o = JObject.Parse(json);

                        foreach (JProperty p in o.Properties())
                        {
                            JArray value = (JArray)p.Value;

                            accruentApisList = value.ToObject<List<SchApi>>();

                            foreach (JObject item in value)
                            {
                                CtvScCallerModel ctvScCallerModel = new CtvScCallerModel();
                                UFloorModel uFloorModel = new UFloorModel();
                                URoomModel uRoomModel = new URoomModel();
                                List<CommentModel> commentModel = new List<CommentModel>();

                                if (!string.IsNullOrEmpty(item["x_ctv_sc_caller"].ToString()))
                                {
                                    var x_ctv_sc_caller_link = (!string.IsNullOrEmpty(item["x_ctv_sc_caller"]["link"].ToString()) ? item["x_ctv_sc_caller"]["link"].ToString() : "");
                                    var ctvScCallerObject = (!string.IsNullOrEmpty(x_ctv_sc_caller_link) ? ApiCallForChildCollection(x_ctv_sc_caller_link) : null);
                                    if (ctvScCallerObject != null)
                                    {
                                        foreach (JProperty pctvsccaller in ctvScCallerObject.Properties())
                                        {

                                            ctvScCallerModel.name = pctvsccaller.Value["name"].ToString();
                                            ctvScCallerModel.email = pctvsccaller.Value["email"].ToString();
                                            ctvScCallerModel.title = pctvsccaller.Value["title"].ToString();
                                            ctvScCallerModel.phone = pctvsccaller.Value["phone"].ToString();
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(item["u_floor"].ToString()))
                                {
                                    var u_floor_link = (!string.IsNullOrEmpty(item["u_floor"]["link"].ToString()) ? item["u_floor"]["link"].ToString() : "");
                                    var uFloorObject = ApiCallForChildCollection(u_floor_link);

                                    if (uFloorObject != null)
                                    {
                                        foreach (JProperty pFloor in uFloorObject.Properties())
                                        {
                                            uFloorModel.ualtfullname = pFloor.Value["u_alt_full_name"].ToString();
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(item["u_room"].ToString()))
                                {
                                    var u_room_link = (!string.IsNullOrEmpty(item["u_room"]["link"].ToString()) ? item["u_room"]["link"].ToString() : "");
                                    var uRoomObject = (!string.IsNullOrEmpty(u_room_link) ? ApiCallForChildCollection(u_room_link) : null);
                                    if (uRoomObject != null)
                                    {
                                        foreach (JProperty pRoom in uRoomObject.Properties())
                                        {
                                            uRoomModel.ualtfullname = pRoom.Value["u_alt_full_name"].ToString();
                                        }
                                    }
                                }

                                //var comment_link = ConfigurationManager.AppSettings["GetComments"] + item["sys_id"].ToString() + "&element=comments";
                                //var comment_link = _appConfigs.WebApiUrl.GetComments + item["sys_id"].ToString() + "&element=comments";//commented due to new implementation comes form Lisa on Monday Apr, 08, 2022 for the subject RE: WO-00049609 question
                                var comment_link = _appConfigs.WebApiUrl.GetComments + item["sys_id"].ToString();
                                
                                var commentObject = ApiCallForChildCollection(comment_link);


                                if (commentObject != null)
                                {
                                    //var commentjson = commentObject.Content.ReadAsStringAsync().Result;
                                    string commentjson = JsonConvert.SerializeObject(commentObject);

                                    if(!string.IsNullOrEmpty(commentjson))
                                        accruentApisList.Where(m => m.number == item["number"].ToString()).Select(r => r.commentjson = commentjson).ToList();


                                    foreach (JProperty pComment in commentObject.Properties())
                                    {
                                        JArray commentValue = (JArray)pComment.Value;

                                        commentModel = commentValue.ToObject<List<CommentModel>>();
                                    }
                                }

                                accruentApisList.Where(m => m.number == item["number"].ToString()).Select(r => r.CtvScCallerModel = ctvScCallerModel).ToList();
                                accruentApisList.Where(m => m.number == item["number"].ToString()).Select(r => r.UFloorModel = uFloorModel).ToList();
                                accruentApisList.Where(m => m.number == item["number"].ToString()).Select(r => r.URoomModel = uRoomModel).ToList();
                                accruentApisList.Where(m => m.number == item["number"].ToString()).Select(r => r.CommentModels = commentModel).ToList();
                            }

                        }

                    }


                }
            }
            catch(Exception ex)
            {
                var exMsg = $"Exception occured due to process Json in GetRequestByAssignGroup method =>{ex.Message}";
                Logger.Info(exMsg);
                _emailNotificationUtility.SendEmail(exMsg);
                //throw ex;
            }


            return accruentApisList;
        }

        public async Task<HttpResponseMessage> PatchRequestBySysId(string sysId, string value)
        {
            var clientBaseAddress = _appConfigs.BaseAddressUrl;
            HttpResponseMessage response = new HttpResponseMessage();
           // bool result = false;
           try
            {
                if (sysId != null)
                {
                    string webapiurl = _appConfigs.WebApiUrl.PostRequests + sysId;

                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(clientBaseAddress);
                        var authKey = _appConfigs.AuthenticationKey;

                        var byteArray = Encoding.ASCII.GetBytes(authKey);
                        var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                        client.DefaultRequestHeaders.Authorization = header;
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                        if (value != null)
                        {
                            var Content = new StringContent(value, Encoding.UTF8, "application/xml");
                            response = await Extension.PatchAsJsonAsync(client, webapiurl, Content);
                            var jsondata = response.Content.ReadAsStringAsync().Result.ToString();
                        }
                    }

                    //response.EnsureSuccessStatusCode();

                    //result = (response.IsSuccessStatusCode) ? true : false;
                }
            }
            catch(Exception ex)
            {
                var exMsg = $"Exception occured due to process patch request in PatchRequestBySysId method =>{ex.Message}";
                Logger.Info(exMsg);
                _emailNotificationUtility.SendEmail(exMsg);
            }

            return response;
            //return result;
        }

        public List<AttachmentModel> GetAttachmentJsonRequestBySysId(string requestType, string sysId)
        {
           //var clientBaseAddress = _appConfigs.BaseAddressUrl;
            HttpResponseMessage response = new HttpResponseMessage();
            List<AttachmentModel> attachmentModel = new List<AttachmentModel>();
            try
            {
                if (sysId != null)
                {
                    string webapiurl = _appConfigs.WebApiUrl.GetFileRequests + sysId + "&sysparm_limit=1";

                    response = ApiCallMethod(webapiurl, requestType) ?? new HttpResponseMessage();

                    response.EnsureSuccessStatusCode();

                    if (response.Content != null && response.IsSuccessStatusCode)
                    {
                       var json = response.Content.ReadAsStringAsync().Result;
                        if (!string.IsNullOrEmpty(json))
                        {
                            JObject o = JObject.Parse(json);
                            LoggerAttachment.Info(o);

                            foreach (JProperty p in o.Properties())
                            {
                                JArray value = (JArray)p.Value;

                                attachmentModel = value.ToObject<List<AttachmentModel>>();

                                // attachmentModel = p.Value.ToObject<AttachmentModel>(); //When need to get single json return response need to use this line //important don't remove it.
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var exMsg = $"Exception occured due to process Get request in GetAttachmentJsonRequestBySysId method =>{ex.Message}";
                LoggerAttachment.Info(exMsg);
                _emailNotificationUtility.SendEmail(exMsg);
            }

            return attachmentModel;
        }
        public Byte[] GetAttachmentRequestBySysId(string requestType,string download_link)
        {
            //var clientBaseAddress = _appConfigs.BaseAddressUrl;
            HttpResponseMessage response = new HttpResponseMessage();
            byte[] result = null;
            try
            {
                if (!string.IsNullOrEmpty(download_link))
                {
                    //string webapiurl = _appConfigs.WebApiUrl.GetFileRequests + sysId + "/file";

                    response = ApiCallMethod(download_link, requestType) ?? response;

                    response.EnsureSuccessStatusCode();

                    if (response.Content != null && response.IsSuccessStatusCode) 
                    {                       
                        // var json = response.Content.ReadAsStreamAsync().Result;                       
                        result = response.Content.ReadAsByteArrayAsync().Result;
                    }
                    LoggerAttachment.Info($"Byte response result is=>{response.StatusCode}");

                    //return result;
                }
            }
            catch (Exception ex)
            {
                var exMsg = $"Exception occured due to process Get request in GetAttachmentRequestBySysId method =>{ex.Message}";
                LoggerAttachment.Info(exMsg);
                _emailNotificationUtility.SendEmail(exMsg);
            }

            return result;

        }

        public bool PostRequestAttachmentBySysId(string sysId, int request_id, string filepath)
        {
            var clientBaseAddress = _appConfigs.BaseAddressUrl;
            HttpResponseMessage response = new HttpResponseMessage();
            bool result = false;
            try
            {
                if (sysId != null)
                {
                    //string filename = Path.GetFileNameWithoutExtension(filepath.Split('/').Last());
                    string filename = filepath.Split('/').Last();
                    string webapiurl = _appConfigs.WebApiUrl.PostFileRequests + sysId + "&file_name="+ filename;
                    //string webapiurl = _appConfigs.WebApiUrl.PostFileRequests;

                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(clientBaseAddress);
                        var authKey = _appConfigs.AuthenticationKey;

                        var byteArray = Encoding.ASCII.GetBytes(authKey);
                        var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                        client.DefaultRequestHeaders.Authorization = header;
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        //dev production code for single attachment process  
                        string fullPath = _appConfigs.AttachmentLocation + filepath;
                        LoggerAttachment.Info($"Attachment POST request reading file from location is=>{fullPath}");

                        if (File.Exists(fullPath))
                        {
                            FileStream fs = File.OpenRead(fullPath);
                            var streamContent = new StreamContent(fs);

                            var FileContent = new ByteArrayContent(streamContent.ReadAsByteArrayAsync().Result);
                            FileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                            response = client.PostAsync(webapiurl, FileContent).Result;
                            result = (response.IsSuccessStatusCode) ? true : false;

                        }
                        else
                        {
                            LoggerAttachment.Info($"Attachment POST request file not found on location =>{fullPath}");
                        }


                        LoggerAttachment.Info($"Attachment POST request processed and result is=>{result} and response code is=>{response.StatusCode}");


                        #region multidocument process code
                        //// dev production code for multi attachment
                        ////filepath = "req9/reqfu9/triangle.jpg";
                        //string fullPath = _appConfigs.AttachmentLocation + filepath;

                        //LoggerAttachment.Info($"Attachment POST request reading file from location is=>{fullPath}");

                        ////string fullPath = _appConfigs.AttachmentLocation + "triangle.jpg";
                        //var fileStream = new ByteArrayContent(File.ReadAllBytes(fullPath));
                        //fileStream.Headers.Remove("Content-Type");
                        //fileStream.Headers.Add("Content-Type", "application/octet-stream");
                        ////fileStream.Headers.Add("Content-Type", "*/*");
                        //fileStream.Headers.Add("Content-Transfer-Encoding", "binary");
                        //fileStream.Headers.Add("Content-Disposition", $"form-data;name=\"uploadFile\"; filename=\"{fullPath.Split('/').Last()}\"");

                        //var multipartContent = new MultipartFormDataContent();
                        //multipartContent.Add(new StringContent("x_ctv_fac_work_order"), "\"table_name\"");
                        //multipartContent.Add(new StringContent(sysId), "\"table_sys_id\"");
                        //multipartContent.Add(fileStream, "uploadFile");

                        //response = client.PostAsync(webapiurl, multipartContent).Result;
                        //result = (response.IsSuccessStatusCode) ? true : false;
                        //LoggerAttachment.Info($"Attachment POST request processed and result is=>{result} and response code is=>{response.StatusCode}");



                        //var filePaths = new DirectoryInfo(_appConfigs.AttachmentLocation).GetFiles().Select(f => f.FullName);
                        //foreach (string filePath in filePaths)
                        //{
                        //    //string fullPath = _appConfigs.AttachmentLocation + "triangle.jpg";
                        //    var fileStream = new ByteArrayContent(File.ReadAllBytes(filePath));
                        //    fileStream.Headers.Remove("Content-Type");
                        //    fileStream.Headers.Add("Content-Type", "application/octet-stream");
                        //    fileStream.Headers.Add("Content-Transfer-Encoding", "binary");
                        //    fileStream.Headers.Add("Content-Disposition", $"form-data;name=\"uploadFile\"; filename=\"{filePath.Split('/').Last()}\"");

                        //    var multipartContent = new MultipartFormDataContent();
                        //    multipartContent.Add(new StringContent("x_ctv_fac_work_order"), "\"table_name\"");
                        //    multipartContent.Add(new StringContent("bbb6d9631bbfec50594e0f6cdc4bcbb6"), "\"table_sys_id\"");
                        //    multipartContent.Add(fileStream, "uploadFile");

                        //    response = client.PostAsync(webapiurl, multipartContent).Result;
                        //}                       
                        #endregion

                    }
                }
            }
            catch (Exception ex)
            {
                var exMsg = $"Exception occured due to process post request for attachment in PostRequestAttachmentBySysId method =>{ex.Message}";
                LoggerAttachment.Info(exMsg);
                _emailNotificationUtility.SendEmail(exMsg);
            }

            return result;
        }

    }
}